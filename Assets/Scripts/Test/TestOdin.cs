using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TestOdin : MonoBehaviour
{
    [ValueDropdown("testString", ExpandAllMenuItems = true)]
    public List<string> StringTreview = new List<string>() { "10000" };

    private IEnumerable testString = new ValueDropdownList<string>()
    { 
        { "Skill1","10000"},
        { "Skill2","10001"} 
    };
    string someValue = "skill1";

    [OnInspectorGUI]
    void OnInspectorGUI()
    {
        if (GUILayout.Button($"Open Generic Selector Popup:{someValue}"))
        {
            List<string> source = new List<string>() { "skill1", "skill2", "skill3" };
            GenericSelector<string> selector = new GenericSelector<string>("Title", false, x => x, source);
            selector.SetSelection(this.someValue);
            selector.SelectionTree.Config.DrawSearchToolbar = false;
            selector.SelectionTree.DefaultMenuStyle.Height = 22;
            selector.SelectionConfirmed += selection => this.someValue = selection.FirstOrDefault();
            var window = selector.ShowInPopup();
            window.OnEndGUI += () => { EditorGUILayout.HelpBox("A quick way of injecting custom GUI to the editor window popup instance.", MessageType.Info); };
            window.OnClose += selector.SelectionTree.Selection.ConfirmSelection; // Confirm selection when window clses.
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
