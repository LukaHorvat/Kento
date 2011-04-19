using System;

namespace Kento
{
	internal class ExternalProperty : ExternalMember
	{
		public Value Value { get; set; }
		public ChangeHandler OnChange { get; set; }
		public ExternalGetter External { get; set; }

		public ExternalProperty ( string Name, bool Static, ChangeHandler OnChange, Value Value )
		{
			this.OnChange = OnChange;
			this.Static = Static;
			Representation = Name;
			this.Value = Value;
		}
		public ExternalProperty ( string Name, bool Static, ChangeHandler OnChange, ExternalGetter External )
		{
			this.OnChange = OnChange;
			this.Static = Static;
			Representation = Name;
			this.External = External;
		}
		public ExternalProperty ( string Name, bool Static, Value Value )
		{
			this.Static = Static;
			Representation = Name;
			this.Value = Value;
		}
		public ExternalProperty ( string Name, bool Static, ExternalGetter External )
		{
			this.Static = Static;
			Representation = Name;
			this.External = External;
		}
	}
}