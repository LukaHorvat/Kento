namespace Kento
{
	public class ExternalBinding : Value
	{
		public string Representation { get; set; }

		public override string ToString ()
		{
			return Representation;
		}
	}
}
