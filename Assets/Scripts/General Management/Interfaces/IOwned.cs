public interface IOwned<T> where T : IOwner
{
    /// <summary>
    /// The "owner" or hub that links this instance to a greater whole.
    /// </summary>
    T Owner { get; }
}

/// <summary>
/// Marker interface for classes that are "owners". 
/// IOwners typically only consist of references to their components.
/// </summary>
public interface IOwner {}