using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class MySelector : OdinSelector<ms.CharAction.Id>
{
    private readonly List<ms.CharAction.Id> source;
    private readonly bool supportsMultiSelect;

    public MySelector (List<ms.CharAction.Id> source, bool supportsMultiSelect)
    {
        this.source = source;
        this.supportsMultiSelect = supportsMultiSelect;
    }

    protected override void BuildSelectionTree (OdinMenuTree tree)
    {
        tree.Config.DrawSearchToolbar = true;
        tree.Selection.SupportsMultiSelect = this.supportsMultiSelect;

        tree.Add ("Defaults/None", null);
        tree.Add ("Defaults/A", new ms.CharAction.Id ());
        tree.Add ("Defaults/B", new ms.CharAction.Id ());
        tree.Add ("Defaults/C", new ms.CharAction.Id ());

        //tree.AddRange (this.source, x => x.Path, x => x.SomeTexture);
    }

    [OnInspectorGUI]
    private void DrawInfoAboutSelectedItem ()
    {
        ms.CharAction.Id selected = this.GetCurrentSelection ().FirstOrDefault ();

        if (selected != null)
        {
            GUILayout.Label ("Name: " + selected.ToString());
            //GUILayout.Label ("Data: " + selected.Data);
        }
    }
}