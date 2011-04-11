namespace Kento
{
	delegate Value ProcessInvoke ( Array Arguments );

	class ExternalFunction : ExternalMember, IFunction
	{
		ProcessInvoke function;
		public ExternalFunction ( string Name, bool Static, ProcessInvoke Function )
		{
			this.Static = Static;
			Representation = Name;
			function = Function;
		}
		public Value Invoke ( Array Arguments )
		{
			return function( Arguments );
		}
	}
}
