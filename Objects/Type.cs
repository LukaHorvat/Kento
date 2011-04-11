using System.Collections.Generic;

namespace Kento
{
	class Type : CodeBlock, IClass
	{
		public Scope Identifiers { get; set; }
		public InstanceFlags Flags { get; set; }

		public Type ( Type BaseClass, CodeBlock Code )
			: this( BaseClass, Code.Value ) { }
		public Type ( Type BaseClass, List<Token> Code )
			: base( Code )
		{
			Identifiers = BaseClass.Identifiers.Clone();
			Type = CodeBlockType.Type;
		}
		public Type ( CodeBlock Code )
			: base( Code.Value )
		{
			Identifiers = new Scope();
			Type = CodeBlockType.Type;
		}
		public override Value Run ()
		{
			Compiler.SetAsCurrentScope( Identifiers );
			base.Run();
			Compiler.ExitScope( true );
			return this;
		}
		public override Value Clone ()
		{
			return new Type( this, Value );
		}
		public Instance MakeInstance ()
		{
			return new Instance( this );
		}
	}
}
