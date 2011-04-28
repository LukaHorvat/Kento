namespace Kento
{
	internal delegate Value ProcessInvoke ( List Arguments );

	internal class ExternalFunction : ExternalMember, IInvokable
	{
		private readonly ProcessInvoke function;

		public ExternalFunction ( string Name, bool Static, ProcessInvoke Function )
		{
			this.Static = Static;
			this.Name = Name;
			function = Function;
		}

		#region IInvokable Members

		public Value Invoke ( List Arguments )
		{
			var toReturn = function( Arguments );
			return toReturn;
		}
		#endregion
	}
}