using Stateless;
using Stateless.Reflection;

namespace AutSoft.Common.StateMachines;

/// <summary>
/// Extension methods for state configuration with Stateless package
/// </summary>
public static class StateConfigurationExtensions
{
    /// <summary>
    /// Permit manually changes in state machine
    /// </summary>
    /// <typeparam name="TState">Type of state machine's states</typeparam>
    /// <typeparam name="TTrigger">Type of state machine's triggers</typeparam>
    /// <param name="config">Configuration instance of a state of the state machine</param>
    /// <param name="trigger">The trigger, which triggers the state transition</param>
    /// <param name="permittedDestinationStates">The permitted target states</param>
    /// <returns>The configured configuration instance</returns>
    public static StateMachine<TState, TTrigger>.StateConfiguration PermitManualChange<TState, TTrigger>(
        this StateMachine<TState, TTrigger>.StateConfiguration config,
        TTrigger trigger,
        TState[] permittedDestinationStates)
    {
        var dynamicStateInfos = new DynamicStateInfos();
        dynamicStateInfos.AddRange(permittedDestinationStates.Select(s => new DynamicStateInfo(s.ToString(), string.Empty)));

        return config.PermitDynamicIf(
            new StateMachine<TState, TTrigger>.TriggerWithParameters<TState>(trigger),
            newState => newState,
            newState => permittedDestinationStates.Contains(newState),
            possibleDestinationStates: dynamicStateInfos);
    }
}
