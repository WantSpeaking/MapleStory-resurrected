using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Utility.Inspector
{
	[Serializable]
	public class ButtonAttribute : PropertyAttribute
	{
		public string method;
		public string label;
		public ButtonColor color;


		public ButtonAttribute (string method, String label, ButtonColor color = ButtonColor.Default)
		{
			this.method = method;
			this.label = label;
			this.color = color;
		}

		public ButtonAttribute (string method, ButtonColor color = ButtonColor.Default)
		{
			this.method = method;
			this.label = method;
			this.color = color;
		}
	}

	public enum ButtonColor
	{
		Default,
		Dark,
		Medium,
		Light,
		Red,
		Orange,
		Yellow,
		Green,
		White
	}

#if UNITY_EDITOR
	[UnityEditor.CustomPropertyDrawer (typeof (ButtonAttribute))]
	public class QuickButtonDrawer : UnityEditor.PropertyDrawer
	{
		private GUIStyle buttonStyle = new GUIStyle (GUI.skin.button);

		[ExecuteInEditMode]
		public override void OnGUI (Rect position, UnityEditor.SerializedProperty property, GUIContent label)
		{
			var att = attribute as ButtonAttribute;
			var obj = property.serializedObject.targetObject;

			SetColor (att);
			if (GUI.Button (position, att.label, buttonStyle))
			{
				obj.GetType ().GetMethod (att.method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Invoke (obj, null);
			}
		}

		void SetColor (ButtonAttribute att)
		{
			Color guiCol;
			Color txtCol;


			switch (att.color)
			{
				default:
				case ButtonColor.Default:
					guiCol = Color.white;
					txtCol = Color.black;
					break;

				case ButtonColor.Dark:
					guiCol = new Color (0.275f, 0.342f, 0.369f);
					txtCol = new Color (0.695f, 0.866f, 0.935f);
					break;

				case ButtonColor.Medium:
					guiCol = new Color (0.410f, 0.511f, 0.552f);
					txtCol = new Color (0.157f, 0.196f, 0.211f);
					break;

				case ButtonColor.Light:
					guiCol = new Color (0.584f, 0.727f, 0.786f);
					txtCol = new Color (0.157f, 0.196f, 0.211f);
					break;

				case ButtonColor.Red:
					guiCol = new Color (0.802f, 0.145f, 0.132f);
					txtCol = new Color (1.000f, 0.773f, 0.766f);
					break;

				case ButtonColor.Orange:
					guiCol = new Color (1.000f, 0.426f, 0.186f);
					txtCol = new Color (1.000f, 0.854f, 0.792f);
					break;

				case ButtonColor.Yellow:
					guiCol = new Color (1.000f, 0.738f, 0.235f);
					txtCol = new Color (1.000f, 0.936f, 0.810f);
					break;

				case ButtonColor.Green:
					guiCol = new Color (0.417f, 0.828f, 0.115f);
					txtCol = new Color (0.879f, 1.000f, 0.786f);
					break;
			}

			buttonStyle.normal.textColor = txtCol;
			GUI.backgroundColor = guiCol;
		}
	}


#endif
}