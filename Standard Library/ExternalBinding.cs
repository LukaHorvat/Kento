namespace Kento
{
	public class ExternalBinding : Value, INamable
	{
		public string Name { get; set; }

		public override string ToString ()
		{
			return Name;
		}
	}
}
