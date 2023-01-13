using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// State storage for component.
/// </summary>
public interface IComponentStateStorage
{
    /// <summary>
    /// Saves the state of the component to the specified instance key.
    /// </summary>
    /// <param name="instanceKey">Key of the instance where to save.</param>
    /// <param name="component">Component to save.</param>
    void SaveStateForComponent(string instanceKey, ComponentBase component);

    /// <summary>
    /// Restores the state of the component from the specified instance key.
    /// </summary>
    /// <param name="instanceKey">Key of the instance from which to reload.</param>
    /// <param name="component">Component to restore.</param>
    void RestoreStateForComponent(string instanceKey, ComponentBase component);

    /// <summary>
    /// Clear all component states.
    /// </summary>
    void ClearComponentStates();
}
