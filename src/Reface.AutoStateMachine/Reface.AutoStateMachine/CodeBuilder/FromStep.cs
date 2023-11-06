namespace Reface.AutoStateMachine.CodeBuilder
{
	public class FromStep<TState, TAction>
		where TState : struct
		where TAction : struct
	{
		private readonly CodeStateMachineBuilder<TState, TAction> codeStateMachineBuilder;
		private readonly TState from;

		public FromStep(CodeStateMachineBuilder<TState, TAction> codeStateMachineBuilder, TState from)
		{
			this.codeStateMachineBuilder = codeStateMachineBuilder;
			this.from = from;
		}

		public ToStep<TState, TAction> When(TAction action)
		{
			return new ToStep<TState, TAction>(
					codeStateMachineBuilder,
					from,
					action,
					this
				);
		}

		public FromStep<TState, TAction> From(TState state)
		{
			return codeStateMachineBuilder.From(state);
		}
	}
}
