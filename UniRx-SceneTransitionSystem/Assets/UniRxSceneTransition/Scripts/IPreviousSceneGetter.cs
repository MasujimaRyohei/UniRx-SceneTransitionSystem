namespace UniRxSceneTransition
{
    /// <summary>
    /// Previous scene getter.
    /// </summary>
    public interface IPreviousSceneGetter
    {
        /// <summary>
        /// Get the previous game scene.
        /// </summary>
        /// <value>The previous game scene.</value>
        EScenes PreviousGameScene { get; }

        /// <summary>
        /// Get the previous additive scenes.
        /// </summary>
        /// <value>The previous additive scenes.</value>
        EScenes[] PreviousAdditiveScenes { get; }
    }
}