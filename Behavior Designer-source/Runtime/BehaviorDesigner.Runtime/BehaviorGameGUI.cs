using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[AddComponentMenu("Behavior Designer/Behavior Game GUI")]
	public class BehaviorGameGUI : MonoBehaviour
	{
		private BehaviorManager behaviorManager;

		private Camera mainCamera;

		public void Start()
		{
			mainCamera = Camera.main;
		}

		public void OnGUI()
		{
			if (behaviorManager == null)
			{
				behaviorManager = BehaviorManager.instance;
			}
			if (behaviorManager == null || mainCamera == null)
			{
				return;
			}
			List<BehaviorManager.BehaviorTree> behaviorTrees = behaviorManager.BehaviorTrees;
			for (int i = 0; i < behaviorTrees.Count; i++)
			{
				BehaviorManager.BehaviorTree behaviorTree = behaviorTrees[i];
				string text = string.Empty;
				for (int j = 0; j < behaviorTree.activeStack.Count; j++)
				{
					Stack<int> stack = behaviorTree.activeStack[j];
					if (stack.Count != 0)
					{
						Task task = behaviorTree.taskList[stack.Peek()];
						if (task is Action)
						{
							text = text + behaviorTree.taskList[behaviorTree.activeStack[j].Peek()].FriendlyName + ((j >= behaviorTree.activeStack.Count - 1) ? string.Empty : "\n");
						}
					}
				}
				Transform transform = behaviorTree.behavior.transform;
				Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
				Vector2 vector2 = GUIUtility.ScreenToGUIPoint(vector);
				GUIContent content = new GUIContent(text);
				Vector2 vector3 = GUI.skin.label.CalcSize(content);
				vector3.x += 14f;
				vector3.y += 5f;
				GUI.Box(new Rect(vector2.x - vector3.x / 2f, (float)Screen.height - vector2.y + vector3.y / 2f, vector3.x, vector3.y), content);
			}
		}
	}
}
