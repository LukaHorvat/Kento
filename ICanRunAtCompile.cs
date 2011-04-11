namespace Kento
{
	interface ICanRunAtCompile
	{
		Value CompileTimeOperate ( Value First, Value Second );
	}
}
