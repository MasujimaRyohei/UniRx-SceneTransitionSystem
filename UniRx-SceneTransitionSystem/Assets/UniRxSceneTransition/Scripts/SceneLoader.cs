using UniRx;
using UnityEngine;

namespace UniRxSceneTransition
{
    /// <summary>
    /// Scene loader.
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>
        /// The previous scene data.
        /// </summary>
        public static SceneDataPackBase PreviousSceneData;

        /// <summary>
        /// The transition manager.
        /// </summary>
        private static TransitionManager transitionManager;

        /// <summary>
        /// Gets the transition manager.
        /// If don't exist, create new one.
        /// </summary>
        /// <value>The transition manager.</value>
        private static TransitionManager TransitionManagerInstance
        {
            get
            {
                if (transitionManager != null) return transitionManager;
                Initialize();
                return transitionManager;
            }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public static void Initialize()
        {
            if (TransitionManager.Instance == null)
            {
                GameObject resource = Resources.Load("TransitionManager") as GameObject;
                Object.Instantiate(resource);
            }
            transitionManager = TransitionManager.Instance;
        }

        /// <summary>
        /// Notice transition finished.
        /// </summary>
        /// <value>The on transition finished.</value>
        public static IObservable<Unit> OnTransitionFinished
        {
            get { return TransitionManagerInstance.OnTransitionAnimationFinished; }
        }


        /// <summary>
        /// Notice scenes loaded.
        /// </summary>
        /// <value>The on scenes loaded.</value>
        public static IObservable<Unit> OnScenesLoaded
        {
            get { return TransitionManagerInstance.OnAllScenesLoaded.FirstOrDefault(); }
        }

        /// <summary>
        /// Open next scene.
        /// If you don't use autoTransition, you have to use the function for transition.
        /// </summary>
        public static void Open()
        {
            TransitionManagerInstance.OpenLoadedNextScene();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SceneLoader"/> is transition running.
        /// </summary>
        /// <value><c>true</c> if is transition running; otherwise, <c>false</c>.</value>
        public static bool IsTransitionRunning
        {
            get { return TransitionManagerInstance.IsRunning; }
        }

        /// <summary>
        /// Load the scene.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="sceneData">Data.</param>
        /// <param name="additiveLoadScenes">Additive load scenes.</param>
        /// <param name="autoTransition">If set to <c>true</c> auto move.</param>
        public static void LoadScene(EScenes scene, SceneDataPackBase sceneData = null, EScenes[] additiveLoadScenes = null, bool autoTransition = true)
        {
            // Send default scene data.
            if (sceneData == null)
                sceneData = new SceneDataPackBase(TransitionManagerInstance.CurrentGameScene, additiveLoadScenes);

            TransitionManagerInstance.StartTransition(scene, sceneData, additiveLoadScenes, autoTransition);
        }
    }
}