using AutSoft.Common.Exceptions;
using AutSoft.Common.Time;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Stateless;

using System.Linq.Expressions;

namespace AutSoft.Common.StateMachines;

/// <summary>
/// Abstract base class of state machines, which add functionalities for Stateless StateMachine class
/// </summary>
/// <typeparam name="TState">Type of state machine's states</typeparam>
/// <typeparam name="TTrigger">Type of state machine's triggers</typeparam>
/// <typeparam name="TEntity">The type of entity whose state is described by the state machine</typeparam>
public abstract class EntityStateMachineBase<TState, TTrigger, TEntity> : StateMachine<TState, TTrigger>
        where TState : struct, Enum
        where TTrigger : Enum
        where TEntity : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityStateMachineBase{TState, TTrigger, TEntity}"/> class.
    /// </summary>
    /// <param name="entity">The entity whose state is described by the state machine</param>
    /// <param name="statePropertySelector">The state property of the entity described by the state machine</param>
    /// <param name="stateModifiedDatePropertySelector">The property of entity, which contains the time of the last change of state</param>
    /// <param name="timeProvider">An instance of <see cref="ITimeProvider"/></param>
    /// <param name="dbContext">An instance of <see cref="DbContext"/></param>
    /// <param name="exceptionMessage">Message of exception</param>
    /// <exception cref="ArgumentNullException">This exception is thrown if the added properties aren't available</exception>
    /// <exception cref="BusinessException">This exception is thrown if a state transition does not permitted</exception>
    protected EntityStateMachineBase(
        TEntity entity,
        Expression<Func<TEntity, TState>> statePropertySelector,
        Expression<Func<TEntity, DateTimeOffset>>? stateModifiedDatePropertySelector = null,
        ITimeProvider? timeProvider = null,
        DbContext? dbContext = null,
        string? exceptionMessage = null)
        : base(
              stateAccessor: () => (TState)statePropertySelector.GetPropertyAccess().GetValue(entity)!,
              stateMutator: state => statePropertySelector.GetPropertyAccess().SetValue(entity, state))
    {
        if (stateModifiedDatePropertySelector != null && timeProvider == null)
            throw new ArgumentNullException(nameof(timeProvider), $"If {nameof(stateModifiedDatePropertySelector)} is specified, the {nameof(timeProvider)} parameter cannot be null");

        if (stateModifiedDatePropertySelector != null && dbContext == null)
            throw new ArgumentNullException(nameof(timeProvider), $"If {nameof(stateModifiedDatePropertySelector)} is specified, the {nameof(dbContext)} parameter cannot be null");

        OnTransitionCompletedAsync(async _ =>
        {
            stateModifiedDatePropertySelector?.GetPropertyAccess().SetValue(entity, timeProvider?.Now);

            if (dbContext != null)
                await dbContext.SaveChangesAsync();
        });

        OnUnhandledTrigger((s, t) =>
        {
            var id = entity.GetType().GetProperty("Id")?.GetValue(entity);
            throw new BusinessException(
                exceptionMessage ?? $"The desired state transition is not allowed! (entity: {entity.GetType().Name} id: {id} state: {s} trigger: {t})", "Incorrect operation!");
        });
    }

    /// <summary>
    /// Gives that an trigger can be fired with specified arguments or not
    /// </summary>
    /// <param name="trigger">The trigger object</param>
    /// <param name="arguments">The specified arguments</param>
    /// <returns>The trigger can be fired with specified arguments or not</returns>
    public bool CanFire(TTrigger trigger, params object[] arguments)
    {
        if (arguments == null || arguments.Length == 0)
            return base.CanFire(trigger);

        return GetPermittedTriggers(arguments).Any(x => Equals(x, trigger));
    }

    /// <summary>
    /// Base abstract class of state machine factories
    /// </summary>
    /// <typeparam name="TStateMachine">Type of state machine to be created</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FactoryBase{TStateMachine}"/> class
    /// </remarks>
    /// <param name="serviceProvider">Instance of an IServiceProvider</param>
    public abstract class FactoryBase<TStateMachine>(IServiceProvider serviceProvider)
        where TStateMachine : EntityStateMachineBase<TState, TTrigger, TEntity>
    {
        /// <summary>
        /// Create a state machine with specified type
        /// </summary>
        /// <param name="entity">The entity whose state is described by the state machine</param>
        /// <returns>The created state machine</returns>
        public TStateMachine CreateStateMachine(TEntity entity)
        {
            return ActivatorUtilities.CreateInstance<TStateMachine>(serviceProvider, entity);
        }
    }
}
