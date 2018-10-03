using UnityEngine;

namespace UniRxSceneTransition
{
    /// <summary>
    /// Scene changer.
    /// For example.
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {
        public static SceneChanger instance;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SceneLoader.LoadScene(EScenes.Logo);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneLoader.LoadScene(EScenes.Title, null, new EScenes[] { EScenes.TitleUI });
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SceneLoader.LoadScene(EScenes.Main, null, new EScenes[] { EScenes.MainUI, EScenes.MainPause });
            }
        }
    }
}