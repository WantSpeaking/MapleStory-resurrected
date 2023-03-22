using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace ms.SkillTrees
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
        [SerializeField]
        string _skillId = "2001004";

        static List<string> source = new List<string>();

        static void FillSourceList ()
        {
            if (source.Count == 0)
            {
                ms.wz.load_string(ms.Constants.get().path_MapleStoryFolder);
                source.Clear();
                foreach (var skill in ms.wz.wzFile_string["skill.img"])
                {
                    //Debug.Log(skill["name"]);
                    if (skill?["name"] != null)
                    {
                        source.Add(skill.Name);
                    }
                }
            }
        }

        string SkillIdToName(string id) => ms.wz.wzFile_string["skill.img"]?[id]?["name"]?.ToString() ?? id;
        

     
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

        public string SkillId { get => _skillId; set => _skillId = value; }

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
            _skillId = EditorGUILayout.TextField("MaxSkillLevel:", _skillId);

            ShowTransitionsInspector();

            if ( actionList == null ) {
                return;
            }

            EditorUtils.CoolLabel("Actions");
            GUI.color = repeatStateActions ? GUI.color : new UnityEngine. Color(1, 1, 1, 0.5f);
            repeatStateActions = UnityEditor.EditorGUILayout.ToggleLeft("Repeat State Actions", repeatStateActions);
            GUI.color = UnityEngine.Color.white;
            actionList.ShowListGUI();
            actionList.ShowNestedActionsGUI();
        }

        void OnInspectorGUI()
        {
            FillSourceList();
            if (GUILayout.Button($"{SkillIdToName(SkillId)}"))
            {
                //List<string> source = new List<string>() { "skill1","skill2"};
                Sirenix.OdinInspector.Editor.GenericSelector<string> selector = new Sirenix.OdinInspector.Editor.GenericSelector<string>("Skill Select", false, x => SkillIdToName(x), source);
                selector.SetSelection(this.SkillId);
                selector.SelectionTree.Config.DrawSearchToolbar = true;
                selector.SelectionTree.DefaultMenuStyle.Height = 22;
                selector.SelectionConfirmed += selection => this.SkillId = selection.FirstOrDefault();
                var window = selector.ShowInPopup();
                window.OnEndGUI += () => { EditorGUILayout.HelpBox("A quick way of injecting custom GUI to the editor window popup instance.", MessageType.Info); };
                window.OnClose += selector.SelectionTree.Selection.ConfirmSelection; // Confirm selection when window clses.
            }
        }

#endif
    }
}