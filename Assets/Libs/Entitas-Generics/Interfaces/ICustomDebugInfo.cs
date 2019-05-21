namespace Entitas.Generics
{
    /// <summary>
    /// An object that provides friendly display information for debugging/the inspector.
    /// </summary>
    public interface ICustomDebugInfo
    {
        string DisplayName { get; }
    }
}