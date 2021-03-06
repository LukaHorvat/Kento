﻿using System;

namespace Kento
{
	public class ExternalClass : ExternalBinding, IClass
	{
		System.Type toMake;

		public ExternalClass ( string Name, InstanceFlags Flags, params ExternalMember[] Members )
		{
			this.Flags = Flags;
			Identifiers = new Scope();
			this.Name = Name;
			foreach ( ExternalMember externalMember in Members )
			{
				var reference = new Reference( externalMember );
				Identifiers[ externalMember.Name ] = reference;
			}
		}

		public ExternalClass ( string Name, InstanceFlags Flags, System.Type TypeToMake, params ExternalMember[] Members )
		{
			this.Flags = Flags;
			Identifiers = new Scope();
			this.Name = Name;
			toMake = TypeToMake;
			foreach ( ExternalMember externalMember in Members )
			{
				Identifiers[ externalMember.Name ] = new Reference( externalMember );
			}
		}

		#region IClass Members

		public InstanceFlags Flags { get; set; }
		public Scope Identifiers { get; set; }

		public virtual Instance MakeInstance ()
		{
			if ( toMake != null )
			{
				if ( toMake.IsSubclassOf( typeof( Instance ) ) )
				{
					var toReturn = Activator.CreateInstance( toMake ) as Instance;
					toReturn.ClassName = Name;
					return toReturn;
				}
			}
			return new Instance( this );
		}

		#endregion
	}
}