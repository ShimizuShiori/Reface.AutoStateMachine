using Reface.AutoStateMachine.Attributes;
using Reface.AutoStateMachine.Errors;

namespace Reface.AutoStateMachine.CodeBuilder
{
	public class CodeStateMachineBuilder<TState, TAction> : IStateMachineBuilder<TState, TAction>
		where TState : struct
		where TAction : struct
	{
		private readonly IList<StateMoveInfo<TState, TAction>> stateMoveInfos
			= new List<StateMoveInfo<TState, TAction>>();
		private TState? startState;
		private IStateMoveInfoSearcher<TState, TAction> stateMoveInfoSearcher;
		private HashSet<TState> stopStateSet;

		public CodeStateMachineBuilder<TState, TAction> Move(TState from, TAction action, TState to)
		{
			if (stateMoveInfoSearcher != null)
				stateMoveInfoSearcher = null;
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
			if (stateMoveInfoSearcher == null)
				stateMoveInfoSearcher = new DefaultStateMoveInfoSearcher<TState, TAction>(stateMoveInfos);

			if (!IsDefaultStateExists())
				startState = GetDefaultState();

			if (!IsDefaultStateExists())
				throw new CodeStateMachineBuilderBuildException("没有指定默认状态，无法构建");

			if (!IsStopStateExists())
				stopStateSet = new HashSet<TState>(GetStopStates());
			if (!IsStopStateExists())
				stopStateSet = new HashSet<TState>();

			return new CodeStateMachine<TState, TAction>(stateMoveInfoSearcher, startState.Value, stopStateSet);
		}

		private TState GetDefaultState()
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

		private bool IsDefaultStateExists()
		{
			return startState != null && startState.HasValue;
		}
		private bool IsStopStateExists()
		{
			return stopStateSet != null;
		}
	}
}
