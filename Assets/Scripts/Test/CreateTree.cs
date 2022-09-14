using UnityEngine;
using System.Collections;
using ms;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using System.Collections.Generic;

public class CreateTree : MonoBehaviour
{
	[EnumToggleButtons]
	public CharAction.Id ActionId = CharAction.Id.savage;
	/*public ExternalBehaviorTree behaviorTree;
	private void Start ()
	{
		var bt = gameObject.AddComponent<BehaviorTree> ();
		bt.StartWhenEnabled = false;
		//bt.ExternalBehavior = behaviorTree;
		bt.ExternalBehavior = Resources.Load<ExternalBehavior> ("Skill_Test");
		bt.EnableBehavior ();
		StartCoroutine ("Test");
	}

	IEnumerator Test ()
	{
		yield return new WaitForSeconds (0.1f);

		

	}*/

	/*	KeyCode someEnumValue;

		[OnInspectorGUI]
		void OnInspectorGUI ()
		{
			// Use the selector manually. See the documentation for OdinSelector for more information.
			if (GUILayout.Button ("Open Enum Selector"))
			{
				EnumSelector<KeyCode> selector = new EnumSelector<KeyCode> ();
				selector.SetSelection (this.someEnumValue);
				selector.SelectionConfirmed += selection => this.someEnumValue = selection.FirstOrDefault ();
				selector.ShowInPopup (); // Returns the Odin Editor Window instance, in case you want to mess around with that as well.
			}

			// Draw an enum dropdown field which uses the EnumSelector popup:
			this.someEnumValue = EnumSelector<KeyCode>.DrawEnumField (new GUIContent ("My Label"), this.someEnumValue);
		}

		// All Odin Selectors can be rendered anywhere with Odin. This includes the EnumSelector.
		EnumSelector<KeyCode> inlineSelector;

		[ShowInInspector]
		EnumSelector<KeyCode> InlineSelector
		{
			get { return this.inlineSelector ?? (this.inlineSelector = new EnumSelector<KeyCode> ()); }
			set { }
		}*/
	ms.CharAction.Id someValue;
	List<ms.CharAction.Id> source = new List<CharAction.Id> () { ms.CharAction.Id.airstrike, ms.CharAction.Id.alert2 };
	void OnGUI ()
	{
		if (GUILayout.Button ("Open My Selector"))
		{
			MySelector selector = new MySelector (source, false);

			selector.SetSelection (this.someValue);

			selector.SelectionCancelled += () => { };  // Occurs when the popup window is closed, and no slection was confirmed.
			selector.SelectionChanged += col => { };
			selector.SelectionConfirmed += col => this.someValue = col.FirstOrDefault ();

			selector.ShowInPopup (); // Returns the Odin Editor Window instance, in case you want to mess around with that as well.
		}
	}
	// All Odin Selectors can be rendered anywhere with Odin.
	[ShowInInspector]
	MySelector inlineSelector;
}