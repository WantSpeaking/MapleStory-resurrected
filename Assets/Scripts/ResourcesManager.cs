using System.Collections;
using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
	private string skillBTreePath = "SkillBTree/Skill_";
	private Dictionary<string, BehaviourTree> skillBTreePool = new Dictionary<string, BehaviourTree> ();
	public BehaviourTree GetSkillBTree (string skillId)
	{
		if (!skillBTreePool.TryGetValue (skillId, out var graph))
		{
			graph = Resources.Load<BehaviourTree> ($"{skillBTreePath}{skillId}");
			skillBTreePool.Add (skillId, graph);
		}
		return graph;
	}

	private string mobBTreePath = "MobBTree/Mob_";
	private Dictionary<string, BehaviourTree> mobBTreePool = new Dictionary<string, BehaviourTree> ();
	public BehaviourTree GetMobBTree (string mobId)
	{
		if (!mobBTreePool.TryGetValue (mobId, out var btree))
		{
			btree = Resources.Load<BehaviourTree> ($"{mobBTreePath}{mobId}");
			if (btree != null)
			{
				mobBTreePool.Add (mobId, btree);
			}
		}
		return btree;
	}
}
