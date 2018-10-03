using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Singleton;

namespace UniRxSceneTransition
{
    /// <summary>
    /// Transition manager.
    /// </summary>
    public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
    {
        [SerializeField]
        protected Fade fade;

        // Is running transition.
        private bool isRunning = false;

        public bool IsRunning { get { return isRunning; } }

        // Can go next scene.
        private ReactiveProperty<bool> CanEndTransition = new ReactiveProperty<bool>(false);

        private EScenes currentGameScene;
        public EScenes CurrentGameScene
        {
            get { return currentGameScene; }
        }

        // Finish notification of transition animation.
        // Finish to open or to close.
        private Subject<Unit> onTransitionFinishedInternal = new Subject<Unit>();

        // Notification of begining scene and finished transition.
        private Subject<Unit> onTransitionAnimationFinishedSubject = new Subject<Unit>();
        // And publish OnCompleted at the same time.
        public IObservable<Unit> OnTransitionAnimationFinished
        {
            get
            {
                if (isRunning)
                {
                    return onTransitionAnimationFinishedSubject.FirstOrDefault();
                }
                else
                {
                    return Observable.Return(Unit.Default);
                }
            }
        }

        // Notification to completed to load all additive scene.
        private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();
        public IObservable<Unit> OnAllScenesLoaded
        {
            get { return onAllSceneLoaded; }
        }

        /// <summary>
        /// Opens the loaded next scene.
        /// If you selected autoTransition = false, you have to invoke this function.
        /// </summary>
        public void OpenLoadedNextScene()
        {
            CanEndTransition.Value = true;
        }

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            try
            {
                currentGameScene = (EScenes)SceneManager.GetActiveScene().buildIndex;
            }
            catch
            {
                Debug.Log("Failed get current scene");
                // Fill any scene.
                currentGameScene = EScenes.Title;
            }
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Start()
        {
            Initialize();

            // Publish scene transition complete notification after Initialize.
            onAllSceneLoaded.OnNext(Unit.Default);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        private void Initialize()
        {
            //トランジションアニメーションが終了したイベントをObservableに変換する
            fade.onFinishFadeOut.AddListener(
                () => onTransitionFinishedInternal.OnNext(Unit.Default));
            fade.onFinishFadeIn.AddListener(
                () => onTransitionFinishedInternal.OnNext(Unit.Default));
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        /// <param name="nextScene">Next scene.</param>
        /// <param name="sceneData">Scene data.</param>
        /// <param name="additiveLoadScenes">Additive load scenes.</param>
        /// <param name="autoTransition">If set to <c>true</c> auto transition.</param>
        public void StartTransition(
            EScenes nextScene,
            SceneDataPackBase sceneData,
            EScenes[] additiveLoadScenes,
            bool autoTransition
            )
        {
            if (isRunning) return;

            StartCoroutine(TransitionCoroutine(nextScene, sceneData, additiveLoadScenes, autoTransition));
        }

        /// <summary>
        /// Transition coroutine.
        /// </summary>
        /// <returns>The coroutine.</returns>
        /// <param name="nextScene">Next scene.</param>
        /// <param name="sceneData">Scene data.</param>
        /// <param name="additiveLoadScenes">Additive load scenes.</param>
        /// <param name="autoTransition">If set to <c>true</c> auto transition.</param>
        private IEnumerator TransitionCoroutine(
            EScenes nextScene,
            SceneDataPackBase sceneData,
            EScenes[] additiveLoadScenes,
            bool autoTransition
            )
        {
            isRunning = true;

            CanEndTransition.Value = autoTransition;

            if (fade == null)
            {
                Initialize();
                yield return null;
            }

            // Start transition.
            fade.FadeIn(1.0f);

            // Wait for transition animation.
            yield return onTransitionFinishedInternal.FirstOrDefault().ToYieldInstruction();

            // Set data what gave by previous scene.
            SceneLoader.PreviousSceneData = sceneData;

            // Load main scene as single scene.
            yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            // Load sub scenes.
            if (additiveLoadScenes != null)
            {
                yield return additiveLoadScenes.Select(scene =>
                    SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive)
                    .AsObservable()).WhenAll().ToYieldInstruction();
            }
            yield return null;

            // Release unused assets memories.
            Resources.UnloadUnusedAssets();
            GC.Collect();

            yield return null;

            // Set current scene.
            currentGameScene = nextScene;

            // Publish notification of scene loaded.
            onAllSceneLoaded.OnNext(Unit.Default);

            if (!autoTransition)
            {
                // Wait for OpenLoadedNextScene function.
                yield return CanEndTransition.FirstOrDefault(x => x).ToYieldInstruction();
            }

            CanEndTransition.Value = false;


            // Start fadeout animation.
            fade.FadeOut(1.0f);

            // Wait for completed to open fade.
            yield return onTransitionFinishedInternal.FirstOrDefault().ToYieldInstruction();

            // Publish notification of finished scene transition.
            onTransitionAnimationFinishedSubject.OnNext(Unit.Default);

            isRunning = false;
        }
    }
}