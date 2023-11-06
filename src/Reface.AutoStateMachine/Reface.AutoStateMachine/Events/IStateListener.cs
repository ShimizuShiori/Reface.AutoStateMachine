namespace Reface.AutoStateMachine.Events
{
	public interface IStateListener<TState, TAction>
	{
		event EventHandler<StateLeavingEventArgs<TState, TAction>> Leaving;
		event EventHandler<StateEnteredEventArgs<TState, TAction>> Entered;
	}
}
