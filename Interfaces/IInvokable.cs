namespace Kento
{
	public interface IInvokable : INamable
	{
		Value Invoke ( List Arguments );
	}
}