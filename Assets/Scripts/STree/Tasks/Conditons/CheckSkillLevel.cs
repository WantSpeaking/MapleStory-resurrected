using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using NodeCanvas.StateMachines;
using ms.SkillTrees;

namespace NodeCanvas.Tasks.Conditions
{

    [Category("✫ Utility")]
    [Description("Check the parent state status. This condition is only meant to be used along with an SkillTree system.")]
    public class CheckSkillLevel : ConditionTask
    {
        public CompareMethod checkType = CompareMethod.GreaterOrEqualTo;
        public BBParameter<int> valueB;
        protected override string info {
            get { return "ParentSkillLevel " + OperationTools.GetCompareString(checkType) + valueB ; }
        }

        protected override bool OnCheck() {
            var st = ownerSystem as SkillTree;
            if ( st != null ) {
                var state = st.previousState as SkillState;
                return OperationTools.Compare(state.curSkillLevel, valueB.value, checkType);
            }
            return false;
        }
    }
}