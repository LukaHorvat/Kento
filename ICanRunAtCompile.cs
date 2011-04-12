namespace Kento
{
	internal interface ICanRunAtCompile
	{
		Value CompileTimeOperate(Value First, Value Second);
	}
}