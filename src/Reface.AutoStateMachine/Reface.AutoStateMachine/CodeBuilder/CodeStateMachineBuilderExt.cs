namespace Reface.AutoStateMachine.CodeBuilder
{
	public static class CodeStateMachineBuilderExt
	{
		public static FromStep<TState, TAction> From<TState, TAction>(this CodeStateMachineBuilder<TState, TAction> builder, TState state)
			where TState : notnull
			where TAction : notnull
		{
			return new FromStep<TState, TAction>(builder, state);
		}
	}
}
