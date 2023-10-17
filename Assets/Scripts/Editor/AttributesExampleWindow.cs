using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEditor;

public class MyCustomEditorWindow : OdinMenuEditorWindow
{
    [MenuItem("My Game/My Editor")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow<MyCustomEditorWindow>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;

        tree.Add("Menu Style", tree.DefaultMenuStyle);

        var allAssets = AssetDatabase.GetAllAssetPaths()
            .Where(x => x.StartsWith("Assets/"))
            .OrderBy(x => x);

        foreach (var path in allAssets)
        {
            tree.AddAssetAtPath(path.Substring("Assets/".Length), path);
        }

        tree.EnumerateTree().AddThumbnailIcons();
        return tree;
    }
}