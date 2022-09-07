using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class TaskIconAttribute : Attribute
	{
		public readonly string mIconPath;

		public string IconPath => mIconPath;

		public TaskIconAttribute(string iconPath)
		{
			mIconPath = iconPath;
		}
	}
}
