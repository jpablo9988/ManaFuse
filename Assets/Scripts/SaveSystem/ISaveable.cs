namespace SaveSystem
{
    /// <summary>
    /// Implemented by any component that wants to participate in save/load.
    /// CaptureState/RestoreState should use small serializable DTOs (no direct scene refs).
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Stable identifier for this saveable object. Typically supplied by a PersistentId component.
        /// </summary>
        string SaveId { get; }

        /// <summary>
        /// Returns a serializable data object describing the current state.
        /// </summary>
        object CaptureState();

        /// <summary>
        /// Applies a previously captured state object.
        /// </summary>
        void RestoreState(object state);
    }
}
