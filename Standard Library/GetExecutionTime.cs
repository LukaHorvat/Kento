namespace Kento
{
	class GetExecutionTime : ExternalFunction
	{
		public GetExecutionTime ()
			: base( "GetTime" ) { }
		public override Value Invoke ( Array Args )
		{
			return new Number( Compiler.RunningTime );
		}
	}
}
