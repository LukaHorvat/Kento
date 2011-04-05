using System.Collections.Generic;

namespace Kento
{
	class Type : CodeBlock
	{
		Dictionary<string, Reference> identifiers;
		public Dictionary<string, Reference> Identifiers
		{
			get { return identifiers; }
			set { identifiers = value; }
		}

		public Type ( Type BaseClass, CodeBlock Code )
			: base( Code.Value )
		{
			identifiers = BaseClass.Identifiers.Clone();
		}
		public Type ( Type BaseClass, List<Token> Code )
			: base( Code )
		{
			identifiers = BaseClass.Identifiers.Clone();
		}
		public Type ( CodeBlock Code )
			: base( Code.Value )
		{
			identifiers = new Dictionary<string, Reference>();
		}
		public override Value Run ()
		{
			Compiler.SetAsCurrentScope( identifiers );
			base.Run();
			return this;
		}
		public override Value Clone ()
		{
			return new Type( this, Value );
		}
	}
}
