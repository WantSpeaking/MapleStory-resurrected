using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AOTLinker : MonoBehaviour
{
	public void Linker()
	{
		BehaviorManager.BehaviorTree behaviorTree = new BehaviorManager.BehaviorTree();
		BehaviorManager.BehaviorTree.ConditionalReevaluate conditionalReevaluate = new BehaviorManager.BehaviorTree.ConditionalReevaluate();
		BehaviorManager.TaskAddData taskAddData = new BehaviorManager.TaskAddData();
		BehaviorManager.TaskAddData.OverrideFieldValue overrideFieldValue = new BehaviorManager.TaskAddData.OverrideFieldValue();
		UnknownTask unknownTask = new UnknownTask();
	}
}
