namespace Kento
{
	public interface IClass : IHasMembers, INamable
	{
		Instance MakeInstance ();
	}
}