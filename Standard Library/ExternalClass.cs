namespace Kento
{
	public class ExternalClass : ExternalBinding, IClass
	{
		public InstanceFlags Flags { get; set; }
		public Scope Identifiers { get; set; }
		public ExternalClass ( string Name, InstanceFlags Flags, params ExternalMember[] Members )
		{
			this.Flags = Flags;
			Identifiers = new Scope();
			Representation = Name;
			foreach ( var externalMember in Members )
			{
				Identifiers[ externalMember.Representation ] = new Reference( externalMember );
			}
		}
		public virtual Instance MakeInstance ()
		{
			return new Instance( this );
		}
	}
}
