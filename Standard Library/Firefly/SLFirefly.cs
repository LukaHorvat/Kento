using FireflyGL;

namespace Kento
{
	class SLFirefly : ILibrarySegment
	{
		public ExternalClass Load ()
		{
			return new ExternalClass( "Firefly", InstanceFlags.NoFlags
				, new ExternalFunction( "Initialize", true, Initialize )
				, new ExternalProperty( "FrameTime", true, () => new Number( Firefly.TotalTime ) )
				, new ExternalProperty( "BackgroundColor", true, SetBackgroundColor, new SLColor( 0, 0, 0 ) ) );
		}
		public Value Initialize ( List Arguments )
		{
			return new SLWindow().Invoke( Arguments );
		}
		public void SetBackgroundColor ( Value NewValue )
		{
			if ( NewValue is SLColor )
			{
				Firefly.Window.ClearColor = ( NewValue as SLColor ).GetColor();
			}
		}
	}
}
