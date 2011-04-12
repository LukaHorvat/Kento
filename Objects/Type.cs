using System.Collections.Generic;

namespace Kento
{
	internal class Type : CodeBlock, IClass
	{
		public Type(Type BaseClass, CodeBlock Code)
			: this(BaseClass, Code.Value) {}

		public Type(Type BaseClass, List<Token> Code)
			: base(Code)
		{
			Identifiers = BaseClass.Identifiers.Clone();
			Type = CodeBlockType.Type;
		}

		public Type(CodeBlock Code)
			: base(Code.Value)
		{
			Identifiers = new Scope();
			Type = CodeBlockType.Type;
		}

		#region IClass Members

		public Scope Identifiers { get; set; }
		public InstanceFlags Flags { get; set; }

		public Instance MakeInstance()
		{
			return new Instance(this);
		}

		#endregion

		public override Value Run()
		{
			Compiler.SetAsCurrentScope(Identifiers);
			base.Run();
			Compiler.ExitScope(true);
			return this;
		}

		public override Value Clone()
		{
			return new Type(this, Value);
		}
	}
}