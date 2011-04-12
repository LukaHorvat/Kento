namespace Kento
{
	internal delegate Value EvaluateBinding();

	internal class ExternalProperty : ExternalMember
	{
		private readonly EvaluateBinding getter;

		public ExternalProperty(string Name, bool Static, EvaluateBinding Getter)
		{
			this.Static = Static;
			Representation = Name;
			getter = Getter;
		}

		public override Value Evaluate()
		{
			Value toReturn = getter();
			toReturn.Static = Static;
			return toReturn;
		}
	}
}