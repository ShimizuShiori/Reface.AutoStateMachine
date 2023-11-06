using Reface.AutoStateMachine.Events;

namespace Reface.AutoStateMachine
{
	public interface IStateMachine<TState, TAction>
		where TState : notnull
		where TAction : notnull
	{
		event EventHandler<StateMachinePushedEventArgs<TState, TAction>> Pushed;
		event EventHandler<StateMachineStopedEventArgs<TState, TAction>> Stopped;
		IStateListener<TState, TAction> GetStateListener(TState state);
		void Push(TAction action);
		bool TryPush(TAction action);
	}
}
