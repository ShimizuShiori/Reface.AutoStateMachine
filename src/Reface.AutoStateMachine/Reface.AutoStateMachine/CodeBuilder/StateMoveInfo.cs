namespace Reface.AutoStateMachine.CodeBuilder
{
	public class StateMoveInfo<TState, TAction>
	{
		public TState From { get; private set; }
		public TAction Action { get; private set; }
		public TState To { get; private set; }

		public StateMoveInfo(TState from, TAction action, TState to)
		{
			From = from;
			Action = action;
			To = to;
		}

		public override string ToString()
		{
			return $"{From} --> {To} : {Action}";
		}
	}
}
