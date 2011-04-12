namespace Kento
{
	public interface IHasMembers
	{
		Scope Identifiers { get; set; }
		InstanceFlags Flags { get; set; }
	}
}