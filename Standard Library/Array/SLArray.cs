namespace Kento
{
	class SLArray : ExternalClass, ILibrarySegment
	{
		public SLArray ( params ExternalMember[] Members )
			: base( "Array", InstanceFlags.Indexable, Members ) { }

		public SLArray ()
			: base( "Array", InstanceFlags.Indexable ) { }

		public ExternalClass Load ()
		{

			return new SLArray();
		}

		public override Instance MakeInstance ()
		{
			return new Array();
		}
	}
}
