using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.SkillTrees
{

    [Name("Parallel Sub FSM", -1)]
    [Description("Execute a Sub FSM in parallel and for as long as this FSM is running.")]
    [Category("SubGraphs")]
    [Color("ff64cb")]
    public class ConcurrentSubFSM : STNodeNested<SkillTree>, IUpdatable
    {

        [SerializeField, ExposeField, Name("Parallel FSM")]
        protected BBParameter<SkillTree> _subFSM = null;

        public override string name => base.name.ToUpper();
        public override int maxInConnections => 0;
        public override int maxOutConnections => 0;
        public override bool allowAsPrime => false;

        public override SkillTree subGraph { get { return _subFSM.value; } set { _subFSM.value = value; } }
        public override BBParameter subGraphParameter => _subFSM;

        ///----------------------------------------------------------------------------------------------

        public override void OnGraphStarted() {
            if ( subGraph == null ) { return; }
            status = Status.Running;
            this.TryStartSubGraph(graphAgent, (result) => { status = result ? Status.Success : Status.Failure; });
        }

        void IUpdatable.Update() {
            this.TryUpdateSubGraph();
        }
    }
}