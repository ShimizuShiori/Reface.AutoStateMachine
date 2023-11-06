using Reface.AutoStateMachine.Events;

namespace Reface.AutoStateMachine.CodeBuilder
{
	public class CodeStateMachine<TState, TAction> : IStateMachine<TState, TAction>
		where TState : notnull
		where TAction : notnull
	{
		private readonly IStateMoveInfoSearcher<TState, TAction> stateMoveInfoSearcher;
		private TState currentState;
		private readonly Dictionary<TState, DefaultStateListener<TState, TAction>> listeners = new Dictionary<TState, DefaultStateListener<TState, TAction>>();
		private readonly HashSet<TState> stopStates;

		public CodeStateMachine(IStateMoveInfoSearcher<TState, TAction> stateMoveInfoSearcher, TState startState, HashSet<TState> stopState)
		{
			this.stateMoveInfoSearcher = stateMoveInfoSearcher;
			currentState = startState;
			stopStates = stopState;
		}

		public event EventHandler<StateMachinePushedEventArgs<TState, TAction>>? Pushed;
		public event EventHandler<StateMachineStopedEventArgs<TState, TAction>>? Stopped;

		public IStateListener<TState, TAction> GetStateListener(TState state)
		{
			return GetStateListenerAsDefaultStateListener(state);
		}

		private DefaultStateListener<TState, TAction> GetStateListenerAsDefaultStateListener(TState state)
		{
			DefaultStateListener<TState, TAction>? result;
			if (!listeners.TryGetValue(state, out result))
			{
				result = new DefaultStateListener<TState, TAction>();
				listeners[state] = result;
			}
			return result;
		}

		public void Push(TAction action)
		{
			var nextInfo = stateMoveInfoSearcher.Search(currentState, action);

			GetStateListenerAsDefaultStateListener(currentState).OnLeaving(this, new StateLeavingEventArgs<TState, TAction>(action, nextInfo.To));
			currentState = nextInfo.To;
			GetStateListenerAsDefaultStateListener(currentState).OnEntered(this, new StateEnteredEventArgs<TState, TAction>(action, nextInfo.From));

			Pushed?.Invoke(this, new StateMachinePushedEventArgs<TState, TAction>(nextInfo.From, action, currentState));
			if (stopStates.Contains(currentState))
				Stopped?.Invoke(this, new StateMachineStopedEventArgs<TState, TAction>(nextInfo.From, action, currentState));
		}

		public bool TryPush(TAction action)
		{
			try
			{
				Push(action);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
