namespace UniRxSceneTransition
{
    /// <summary>
    /// Scene data pack base.
    /// </summary>
    public class SceneDataPackBase : IPreviousSceneGetter
    {
        private readonly EScenes previousScene;
        private readonly EScenes[] previousAdditiveScenes;

        public EScenes PreviousGameScene
        {
            get { return previousScene; }
        }

        public EScenes[] PreviousAdditiveScenes
        {
            get { return previousAdditiveScenes; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SceneDataPackBase"/> class.
        /// </summary>
        /// <param name="previousScene">Previous scene.</param>
        /// <param name="previousAdditiveScenes">Previous additive scenes.</param>
        /// <param name="additiveScenes">Additive scenes.</param>
        public SceneDataPackBase(EScenes previousScene, EScenes[] previousAdditiveScenes)
        {
            this.previousScene = previousScene;
            this.previousAdditiveScenes = previousAdditiveScenes;
        }
    }
}