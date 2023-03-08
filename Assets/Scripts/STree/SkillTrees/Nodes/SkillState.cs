using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace NodeCanvas.SkillTrees
{

    [Name("Skill State", 100)]
    [Description("Execute a number of Action Tasks OnEnter. All actions will be stoped OnExit. This state is Finished when all Actions are finished as well")]
    public class SkillState : STState, ITaskAssignable
    {
        [SerializeField]
        private int _curSkillLevel;
        [SerializeField]
        private int _maxSkillLevel;
        [SerializeField]
        private ActionList _actionList;
        [SerializeField]
        private bool _repeatStateActions;

        [ValueDropdown("testString", ExpandAllMenuItems =  true)]
        public List<string> StringTreview = new List<string>() { "10000" };
        private IEnumerable testString = new ValueDropdownList<string>()
        { { "Skill1","10000"} };

        [SerializeField]
        string someValue = "skill1";

        static List<string> source = new List<string>();
        static void FillSourceList ()
        {
            if (source.Count == 0)
            {
                ms.wz.load_string("G:\\Program Files (x86)\\MapleStory\\");
                source.Clear();
                foreach (var skill in ms.wz.wzFile_string["skill.img"])
                {
                    //Debug.Log(skill["name"]);
                    if (skill?["name"] != null)
                    {
                        source.Add(skill.Name);
                    }
                }
               // source = ms.wz.wzFile_string["skill.img"].Select(n => n["name"].ToString()).ToList();
            }
        }

        string SkillIdToName(string id) => ms.wz.wzFile_string["skill.img"]?[id]?["name"]?.ToString() ?? id;
        

        [OnInspectorGUI]
        void OnInspectorGUI()
        {
            FillSourceList();
            if (GUILayout.Button($"{SkillIdToName(someValue)}"))
            {
                //List<string> source = new List<string>() { "skill1","skill2"};
                GenericSelector<string> selector = new GenericSelector<string>("Skill Select", false, x=> SkillIdToName(x), source);
                selector.SetSelection(this.someValue);
                selector.SelectionTree.Config.DrawSearchToolbar = false;
                selector.SelectionTree.DefaultMenuStyle.Height = 22;
                selector.SelectionConfirmed += selection => this.someValue = selection.FirstOrDefault();
                var window = selector.ShowInPopup();
                window.OnEndGUI += () => { EditorGUILayout.HelpBox("A quick way of injecting custom GUI to the editor window popup instance.", MessageType.Info); };
                window.OnClose += selector.SelectionTree.Selection.ConfirmSelection; // Confirm selection when window clses.
            }
        }
        public Task task {
            get { return actionList; }
            set { actionList = (ActionList)value; }
        }

        public ActionList actionList {
            get { return _actionList; }
            set { _actionList = value; }
        }

        public bool repeatStateActions {
            get { return _repeatStateActions; }
            set { _repeatStateActions = value; }
        }

        public int curSkillLevel
        {
            get { return _curSkillLevel; }
            set { _curSkillLevel = value; }
        }
        public int maxSkillLevel
        {
            get { return _maxSkillLevel; }
            set { _maxSkillLevel = value; }
        }
        public override void OnValidate(Graph assignedGraph) {
            if ( actionList == null ) {
                actionList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
                actionList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
            }
        }

        protected override void OnEnter() { OnUpdate(); }

        protected override void OnUpdate() {
            var actionListStatus = actionList.Execute(graphAgent, graphBlackboard);
            if ( !repeatStateActions && actionListStatus != Status.Running ) {
                Finish(actionListStatus);
            }
        }

        protected override void OnExit() {
            actionList.EndAction(null);
        }

        protected override void OnPause() {
            actionList.Pause();
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override void OnNodeGUI() {
            if ( repeatStateActions ) {
                GUILayout.Label("<b>[REPEAT]</b>");
            }
            _curSkillLevel = EditorGUILayout.IntField("CurSkillLevel:", _curSkillLevel);
            _maxSkillLevel = EditorGUILayout.IntField("MaxSkillLevel:", _maxSkillLevel);
            OnInspectorGUI();
            base.OnNodeGUI();
        }

        protected override void OnNodeInspectorGUI() {
            OnInspectorGUI();
            _curSkillLevel = EditorGUILayout.IntField("SkillLevel:", _curSkillLevel);
            _maxSkillLevel = EditorGUILayout.IntField("MaxSkillLevel:", _maxSkillLevel);

            ShowTransitionsInspector();

            if ( actionList == null ) {
                return;
            }

            EditorUtils.CoolLabel("Actions");
            GUI.color = repeatStateActions ? GUI.color : new Color(1, 1, 1, 0.5f);
            repeatStateActions = UnityEditor.EditorGUILayout.ToggleLeft("Repeat State Actions", repeatStateActions);
            GUI.color = Color.white;
            actionList.ShowListGUI();
            actionList.ShowNestedActionsGUI();
        }

#endif
    }
}