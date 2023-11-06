namespace Reface.AutoStateMachine.CodeBuilder
{
	public class ToStep<TState, TAction>
			where TState : notnull
			where TAction : notnull
	{
		private readonly CodeStateMachineBuilder<TState, TAction> codeStateMachineBuilder;
		private readonly TState from;
		private readonly TAction action;
		private readonly FromStep<TState, TAction> step1;

		public ToStep(CodeStateMachineBuilder<TState, TAction> codeStateMachineBuilder, TState from, TAction action, FromStep<TState, TAction> step1)
		{
			this.codeStateMachineBuilder = codeStateMachineBuilder;
			this.from = from;
			this.action = action;
			this.step1 = step1;
		}

		public FromStep<TState, TAction> To(TState to)
		{
			codeStateMachineBuilder.Move(from, action, to);
			return step1;
		}
	}
}
