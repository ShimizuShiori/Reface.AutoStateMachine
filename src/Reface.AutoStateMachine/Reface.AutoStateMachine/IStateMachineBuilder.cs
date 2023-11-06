namespace Reface.AutoStateMachine
{
	public interface IStateMachineBuilder<TState, TAction>
		where TState : notnull
		where TAction : notnull
	{
		IStateMachine<TState, TAction> Build();
	}
}
