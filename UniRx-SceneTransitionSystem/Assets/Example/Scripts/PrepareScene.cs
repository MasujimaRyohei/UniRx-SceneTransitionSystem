using UnityEngine;

namespace UniRxSceneTransition
{
    /// <summary>
    /// Prepare scene.
    /// For use TransitionManager.
    /// </summary>
    public class PrepareScene : MonoBehaviour
    {
        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Start()
        {
            SceneLoader.LoadScene(EScenes.Logo);
        }
    }
}