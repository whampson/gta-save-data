namespace GTASaveData
{
    /// <summary>
    /// Defines an object that can be copied without sharing references.
    /// </summary>
    /// <typeparam name="T">The type of object to make deeply clonable.</typeparam>
    public interface IDeepClonable<T>
    {
        /// <summary>
        /// Create a deep copy of this object.
        /// </summary>
        T DeepClone();
    }
}
