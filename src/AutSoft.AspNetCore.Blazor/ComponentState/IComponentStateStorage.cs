using Microsoft.AspNetCore.Components;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <summary>
/// State storage for component.
/// </summary>
public interface IComponentStateStorage
{
    /// <summary>
    /// Saves the component state.
    /// </summary>
    /// <param name="instanceKey">Key of the instance.</param>
    /// <param name="component">Component to save.</param>
    void SaveStateForComponent(string instanceKey, ComponentBase component);

    /// <summary>
    /// Restores the component state.
    /// </summary>
    /// <param name="instanceKey">Key of the instance.</param>
    /// <param name="component">Component to restore.</param>
    void RestoreStateForComponent(string instanceKey, ComponentBase component);

    /// <summary>
    /// Clears the component state.
    /// </summary>
    void ClearComponentStates();
}
