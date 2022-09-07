using System;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedGenericVariable : SharedVariable<GenericVariable>
	{
		public SharedGenericVariable()
		{
			mValue = new GenericVariable();
		}

		public static implicit operator SharedGenericVariable(GenericVariable value)
		{
			SharedGenericVariable sharedGenericVariable = new SharedGenericVariable();
			sharedGenericVariable.mValue = value;
			return sharedGenericVariable;
		}
	}
}
