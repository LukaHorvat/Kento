namespace Kento
{
	class SLConvert : ILibrarySegment
	{
		public ExternalClass Load ()
		{
			var toNumber = new ExternalFunction( "ToNumber", true, ToNumber );

			return new ExternalClass( "Convert", InstanceFlags.NoFlags, toNumber );
		}

		public Value ToNumber ( Array Arguments )
		{
			var value = Arguments.GetReferenceAtIndex( 0 ).ReferencingValue;
			if ( value is Number )
			{
				return value as Number;
			}
			if ( value is String )
			{
				return new Number( double.Parse( ( value as String ).Val ) );
			}
			if ( value is Boolean )
			{
				return new Number( ( value as Boolean ).Val ? 1 : 0 );
			}
			return NoValue.Value;
		}
	}
}
