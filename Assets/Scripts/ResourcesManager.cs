using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
	private string skillBTreePath = "SkillBTree/Skill_";
	private Dictionary<string, Graph> skillBTreePool = new Dictionary<string, Graph> ();
	public Graph GetSkillGraph (string skillId)
	{
		if (!skillBTreePool.TryGetValue (skillId, out var graph))
		{
			graph = Resources.Load<Graph> ($"{skillBTreePath}{skillId}");
			skillBTreePool.Add (skillId, graph);
		}
		return graph;
	}
}
