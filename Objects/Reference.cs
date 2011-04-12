namespace Kento
{
	public class Reference : Value
	{
		protected int Index;

		public Reference(Value ValueToReference)
			: this(ValueToReference is Reference ? (ValueToReference as Reference).Index : Compiler.StoreValue(ValueToReference)) {}

		public Reference(Reference Reference)
			: this(Reference.Index) {}

		public Reference(int Index)
		{
			Static = Compiler.GetValue(Index).Static;
			this.Index = Index;
			if (this.Index != -1) //NullReference case
				Compiler.RegisterReference(this, this.Index);
		}

		public Value ReferencingValue
		{
			get
			{
				Value toReturn = Compiler.GetValue(Index);
				if (toReturn is ExternalProperty) toReturn = toReturn.Evaluate();
				return toReturn;
			}
			set { Compiler.SetValue(Index, value); }
		}

		public void ChangeReference(Reference Reference)
		{
			Index = Reference.Index;
		}

		public void ChangeReference(int Index)
		{
			Dereference();
			this.Index = Index;
			Compiler.RegisterReference(this, this.Index);
		}

		public override Value Clone()
		{
			return new Reference(ReferencingValue.Clone());
		}

		public HardReference GetHardReference()
		{
			return new HardReference(Index);
		}

		public virtual void Dereference()
		{
			Compiler.Deference(this, Index);
			Index = -1;
		}

		public override bool Equals(object Obj)
		{
			var reference = Obj as Reference;
			if (reference != null)
			{
				int myIndex = Index;
				int referenceIndex = reference.Index;
				return myIndex == referenceIndex;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Index;
		}

		public override string ToString()
		{
			return ReferencingValue.ToString();
		}
	}

	internal class NullReference : Reference
	{
		private static NullReference value = new NullReference();

		public NullReference()
			: base(-1) {}

		public static NullReference Value
		{
			get { return value; }
			set { NullReference.value = value; }
		}
	}
}