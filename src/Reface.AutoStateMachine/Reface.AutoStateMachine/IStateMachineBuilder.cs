namespace Reface.AutoStateMachine
{
	public interface IStateMachineBuilder<TState, TAction>
		where TState : struct
		where TAction : struct
	{
		IStateMachine<TState, TAction> Build();
	}
}
