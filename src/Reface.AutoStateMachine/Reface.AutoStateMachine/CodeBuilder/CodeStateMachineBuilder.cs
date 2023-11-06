using Reface.AutoStateMachine.Attributes;
using Reface.AutoStateMachine.Errors;

namespace Reface.AutoStateMachine.CodeBuilder
{
	public class CodeStateMachineBuilder<TState, TAction> : IStateMachineBuilder<TState, TAction>
		where TState : notnull
		where TAction : notnull
	{
		private readonly IList<StateMoveInfo<TState, TAction>> stateMoveInfos
			= new List<StateMoveInfo<TState, TAction>>();
		private TState? startState;
		private HashSet<TState>? stopStateSet;

		public CodeStateMachineBuilder<TState, TAction> Move(TState from, TAction action, TState to)
		{
			stateMoveInfos.Add(new StateMoveInfo<TState, TAction>(from, action, to));
			return this;
		}

		public CodeStateMachineBuilder<TState, TAction> StartWith(TState state)
		{
			startState = state;
			return this;
		}

		public CodeStateMachineBuilder<TState, TAction> StopWith(params TState[] states)
		{
			stopStateSet = new HashSet<TState>(states);
			return this;
		}

		public IStateMachine<TState, TAction> Build()
		{
			var stateMoveInfoSearcher = new DefaultStateMoveInfoSearcher<TState, TAction>(stateMoveInfos);

			if (startState == null)
				startState = GetDefaultState();

			if (startState == null)
				throw new CodeStateMachineBuilderBuildException("Failed to Build as there is no default State.");

			if (stopStateSet == null)
				stopStateSet = new HashSet<TState>(GetStopStates());

			if (stopStateSet == null)
				stopStateSet = new HashSet<TState>();

			return new CodeStateMachine<TState, TAction>(stateMoveInfoSearcher, startState, stopStateSet);
		}

		private TState? GetDefaultState()
		{
			var fields = EnumHelper.GetItemsByAttribute<TState, StartStateAttribute>();
			if (fields.Count != 1) return default;
			return (TState)Enum.Parse(typeof(TState), fields[0].Name);
		}

		private IEnumerable<TState> GetStopStates()
		{
			var fields = EnumHelper.GetItemsByAttribute<TState, StopStateAttribute>();
			return fields.Select(x => (TState)Enum.Parse(typeof(TState), x.Name));
		}
	}
}
