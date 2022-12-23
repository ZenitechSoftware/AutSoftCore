using Microsoft.AspNetCore.Components;

using System.Reflection;

namespace AutSoft.AspNetCore.Blazor.ComponentState;

/// <inheritdoc />
public class ComponentStateStorage : IComponentStateStorage
{
    private readonly Dictionary<string, List<StateEntry>> _currentComponentStates = new();

    /// <inheritdoc />
    public void SaveStateForComponent(string instanceKey, ComponentBase component)
    {
        var componentType = component.GetType();

        var componentStates = new List<StateEntry>();

        var propertiesToSave = GetPropertiesFromHierarchy(componentType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(prop => prop.IsDefined(typeof(PreserveStateAttribute), false))
            .ToList();

        foreach (var property in propertiesToSave)
        {
            var value = property.GetValue(component);

            if (value != null)
                componentStates.Add(new StateEntry(property, value));
        }

        var fieldsToSave = GetFieldsFromHierarchy(componentType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(prop => prop.IsDefined(typeof(PreserveStateAttribute), false))
            .ToList();

        foreach (var field in fieldsToSave)
        {
            var value = field.GetValue(component);

            if (value != null)
                componentStates.Add(new StateEntry(field, value));
        }

        _currentComponentStates[instanceKey] = componentStates;
    }

    /// <inheritdoc />
    public void RestoreStateForComponent(string instanceKey, ComponentBase component)
    {
        if (!_currentComponentStates.ContainsKey(instanceKey))
            return;

        foreach (var state in _currentComponentStates[instanceKey])
        {
            if (state.Member is PropertyInfo pi)
                pi.SetValue(component, state.Value);
            else if (state.Member is FieldInfo fi)
            {
                fi.SetValue(component, state.Value);
            }
            else
            {
                throw new NotSupportedException("Field type not supported!");
            }
        }
    }

    /// <inheritdoc />
    public void ClearComponentStates()
    {
        _currentComponentStates.Clear();
    }

    private IEnumerable<PropertyInfo> GetPropertiesFromHierarchy(Type type, BindingFlags bindingFlags)
    {
        foreach (var property in type.GetProperties(bindingFlags))
            yield return property;

        if (type.BaseType == null)
            yield break;

        foreach (var property in GetPropertiesFromHierarchy(type.BaseType, bindingFlags))
            yield return property;
    }

    private IEnumerable<FieldInfo> GetFieldsFromHierarchy(Type type, BindingFlags bindingFlags)
    {
        foreach (var field in type.GetFields(bindingFlags))
            yield return field;

        if (type.BaseType == null)
            yield break;

        foreach (var field in GetFieldsFromHierarchy(type.BaseType, bindingFlags))
            yield return field;
    }
}
