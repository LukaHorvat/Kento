namespace Kento
{
	public interface IClass : IHasMembers
	{
		Instance MakeInstance ();
		string Name { get; set; }
	}
}