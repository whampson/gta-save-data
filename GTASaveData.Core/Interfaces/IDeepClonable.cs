namespace GTASaveData.Interfaces
{
    /// <summary>
    /// Interface for an object that can be copied without sharing references.
    /// </summary>
    /// <typeparam name="T">The type of object to make deeply clonable.</typeparam>
    public interface IDeepClonable<T>
    {
        /// <summary>
        /// Create a copy of this object that does not share references.
        /// </summary>
        T DeepClone();
    }
}
