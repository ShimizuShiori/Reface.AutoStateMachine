namespace Reface.AutoStateMachine.CodeBuilder
{
	public interface IStateMoveInfoSearcher<TState, TAction>
	{
		StateMoveInfo<TState, TAction> Search(TState from, TAction when);
	}
}
