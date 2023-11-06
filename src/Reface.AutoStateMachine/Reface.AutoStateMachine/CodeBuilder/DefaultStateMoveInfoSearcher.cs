using Reface.AutoStateMachine.Errors;


namespace Reface.AutoStateMachine.CodeBuilder
{
	public class DefaultStateMoveInfoSearcher<TState, TAction> : IStateMoveInfoSearcher<TState, TAction>
		where TState : notnull
		where TAction : notnull
	{
		private Dictionary<TState, Dictionary<TAction, StateMoveInfo<TState, TAction>>> moveInfoDictionary
			= new Dictionary<TState, Dictionary<TAction, StateMoveInfo<TState, TAction>>>();

		public DefaultStateMoveInfoSearcher(IEnumerable<StateMoveInfo<TState, TAction>> infos)
		{
			foreach (var info in infos)
			{
				if (!moveInfoDictionary.TryGetValue(info.From, out var actionMap))
				{
					actionMap = new Dictionary<TAction, StateMoveInfo<TState, TAction>>();
					moveInfoDictionary[info.From] = actionMap;
				}

				if (!actionMap.TryGetValue(info.Action, out var moveInfo))
				{
					actionMap[info.Action] = info;
					continue;
				}

				if (!moveInfo.To.Equals(info.To))
					throw new CodeStateMachineBuilderBuildException($"Duplicated Transformation: [{info.From}]--[{info.Action}]-->[{info.To} , {moveInfo.To}]");
			}
		}

		public StateMoveInfo<TState, TAction> Search(TState from, TAction when)
		{
			if (!moveInfoDictionary.TryGetValue(from, out var actionMap))
			{
				throw SearchMoveInfoException.CreateByNoMoveInfo();
			}
			if (!actionMap.TryGetValue(when, out var info))
			{
				throw SearchMoveInfoException.CreateByNoMoveInfo();
			}
			return info;

		}
	}
}
