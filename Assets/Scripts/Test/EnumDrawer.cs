
using System.Linq;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ms_Unity
{
	public class MyRangeAttribute : DrawerAttribute
	{
		public float min = 0;
		public float max = 1;
		public KeyCode keyCode;
		public MyRangeAttribute (float min, float max, KeyCode keyCode)
		{
			this.min = min;
			this.max = max;
			this.keyCode = keyCode;
		}
	}
	public class MyRangeDrawer : AttributeDrawer<MyRangeAttribute>
	{
		public override object OnGUI (GUIContent content, object instance)
		{
			// Use the selector manually. See the documentation for OdinSelector for more information.
			/*if (GUILayout.Button ("Open Enum Selector"))
			{
				EnumSelector<KeyCode> selector = new EnumSelector<KeyCode> ();
				selector.SetSelection ((KeyCode)instance);
				selector.SelectionConfirmed += selection => instance = selection.FirstOrDefault ();
				selector.ShowInPopup (); // Returns the Odin Editor Window instance, in case you want to mess around with that as well.
			}*/

			// Draw an enum dropdown field which uses the EnumSelector popup:
			if (fieldInfo.FieldType == typeof (KeyCode))
			{
				return EnumSelector<KeyCode>.DrawEnumField (new GUIContent ("KeyCode"), (KeyCode)instance);
			}

			if (fieldInfo.FieldType == typeof (float))
			{
				return EditorGUILayout.Slider (content, (float)instance, (float)attribute.min, (float)attribute.max);
			}
			if (fieldInfo.FieldType == typeof (int))
			{
				return EditorGUILayout.IntSlider (content, (int)instance, (int)attribute.min, (int)attribute.max);
			}
			return MoveNextDrawer ();
		}

		//KeyCode someEnumValue;
		// All Odin Selectors can be rendered anywhere with Odin. This includes the EnumSelector.
		EnumSelector<KeyCode> inlineSelector;

		[ShowInInspector]
		EnumSelector<KeyCode> InlineSelector
		{
			get { return this.inlineSelector ?? (this.inlineSelector = new EnumSelector<KeyCode> ()); }
			set { }
		}
	}

	public class CharActionIdSelectorAttribute : DrawerAttribute
	{

	}
	public class CharActionIdSelectorDrawer : AttributeDrawer<CharActionIdSelectorAttribute>
	{
		public override object OnGUI (GUIContent content, object instance)
		{
			return EnumSelector<ms.CharAction.Id>.DrawEnumField (new GUIContent ("CharAction.Id"), (ms.CharAction.Id)instance);
		}
	}

	public class StanceIdSelectorAttribute : DrawerAttribute
	{

	}
	public class StanceIdSelectorDrawer : AttributeDrawer<StanceIdSelectorAttribute>
	{
		public override object OnGUI (GUIContent content, object instance)
		{
			return EnumSelector<ms.Stance.Id>.DrawEnumField (content, (ms.Stance.Id)instance);
		}
	}

	public class AfterimgIdSelectorAttribute : DrawerAttribute
	{

	}
	public class AfterimgIdSelectorDrawer : AttributeDrawer<AfterimgIdSelectorAttribute>
	{
		public override object OnGUI (GUIContent content, object instance)
		{
			return EnumSelector<ms.Afterimage.Id>.DrawEnumField (content, (ms.Afterimage.Id)instance);
		}
	}
}