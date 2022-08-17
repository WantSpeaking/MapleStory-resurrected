using System;
using ms;

namespace Attributes
{
	public class UIElementAttribute : Attribute
	{
		public UIElement.Type TYPE = UIElement.Type.ITEMINVENTORY;
		public bool FOCUSED = false;
		public bool TOGGLED = true;

		public UIElementAttribute (UIElement.Type type, bool focused, bool toggled)
		{
			TYPE = type;
			FOCUSED = focused;
			TOGGLED = toggled;
		}
	}
}