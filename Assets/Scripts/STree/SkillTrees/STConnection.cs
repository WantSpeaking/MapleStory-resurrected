using NodeCanvas.Framework;
using UnityEngine;


namespace NodeCanvas.SkillTrees
{

    ///<summary>The connection object for FSM nodes. AKA Transitions</summary>
    public class STConnection : Connection, ITaskAssignable<ConditionTask>
    {
        [SerializeField]
        private SkillTree.TransitionCallMode _transitionCallMode;
        [SerializeField]
        private ConditionTask _condition;

        public ConditionTask condition {
            get { return _condition; }
            set { _condition = value; }
        }

        public Task task {
            get { return condition; }
            set { condition = (ConditionTask)value; }
        }

        public SkillTree.TransitionCallMode transitionCallMode {
            get { return _transitionCallMode; }
            private set { _transitionCallMode = value; }
        }

        //...
        public void EnableCondition(Component agent, IBlackboard blackboard) {
            if ( condition != null ) {
                condition.Enable(agent, blackboard);
            }
        }

        //...
        public void DisableCondition() {
            if ( condition != null ) {
                condition.Disable();
            }
        }

        ///<summary>Perform the transition disregarding whether or not the condition (if any) is valid</summary>
        public void PerformTransition() {
            ( graph as SkillTree ).EnterState((STState)targetNode, transitionCallMode);
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        public override TipConnectionStyle tipConnectionStyle {
            get { return TipConnectionStyle.Arrow; }
        }

        public override bool animate {
            get { return status == Status.Failure; }
        }

        protected override string GetConnectionInfo() {
            var result = transitionCallMode == SkillTree.TransitionCallMode.Normal ? string.Empty : string.Format("<b>[{0}]</b>\n", transitionCallMode.ToString());
            result += condition != null ? condition.summaryInfo : "OnFinish";
            return result;
        }

        protected override void OnConnectionInspectorGUI() {
            UnityEditor.EditorGUILayout.HelpBox("Stacked Call Mode will push the current state to the stack and pop return to it later when another finished state without outgoing transitions has been encountered within the FSM. If you decide to use this feature make sure that you are not cycle stacking states.\nA Clean transition will clear the FSM stack.", UnityEditor.MessageType.None);
            transitionCallMode = (SkillTree.TransitionCallMode)UnityEditor.EditorGUILayout.EnumPopup("Call Mode (Experimental)", transitionCallMode);
            ParadoxNotion.Design.EditorUtils.Separator();
            NodeCanvas.Editor.TaskEditor.TaskFieldMulti<ConditionTask>(condition, graph, (c) => { condition = c; });
        }

#endif
    }
}