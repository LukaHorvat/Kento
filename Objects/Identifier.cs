namespace Kento
{
	internal class Identifier : Variable
	{
		public Identifier(string Name)
		{
			this.Name = Name;
		}

		public string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public override Value Evaluate()
		{
			if (Compiler.Runtime)
			{
				Reference reference = Compiler.Identify(Name);
				if (reference == NullReference.Value)
				{
					reference = Compiler.Reserve();
					Compiler.SetAlias(Name, reference);
				}
				if (Compiler.PendingDot)
				{
					Compiler.PendingDot = false;
					Compiler.ExitScope(true);
				}
				return reference;
			}
			return NoValue.Value;
		}
	}
}