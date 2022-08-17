using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using MonoGame.Extended.SceneGraphs;

namespace ms
{
    // Interface for active buffs which are applied to character stats
    public abstract class ActiveBuff : IDisposable
    {
        public virtual void Dispose()
        {
        }

        public virtual void apply_to(CharStats stats, short value)
        {

        }

        public virtual void OnAdd(CharStats stats, short value)
        {

        }
        public virtual void OnRemove(CharStats stats, short value)
        {

        }
        public virtual void OnUpdate(CharStats stats, short value)
        {

        }

        public virtual void OnDraw(CharStats stats, short value)
        {


        }
    }

    // Template for buffs which just add their value to a stat
    public class SimpleStatBuff : ActiveBuff
    {
        private readonly EquipStat.Id STAT;

        public SimpleStatBuff(EquipStat.Id stat)
        {
            STAT = stat;
        }

        public override void apply_to(CharStats stats, short value)
        {
            stats.add_buff(STAT, value);
        }
    }

    // Template for buffs which apply an increase by percentage
    public class PercentageStatBuff : ActiveBuff
    {
        private readonly EquipStat.Id STAT;

        public PercentageStatBuff(EquipStat.Id stat)
        {
            STAT = stat;
        }

        public override void apply_to(CharStats stats, short value)
        {
            stats.add_percent(STAT, (float)value / 100);
        }
    }

    // Buff for MAPLEWARRIOR
    public class MapleWarriorBuff : ActiveBuff
    {
        public override void apply_to(CharStats stats, short value)
        {
            stats.add_percent(EquipStat.Id.STR, (float)value / 100);
            stats.add_percent(EquipStat.Id.DEX, (float)value / 100);
            stats.add_percent(EquipStat.Id.INT, (float)value / 100);
            stats.add_percent(EquipStat.Id.LUK, (float)value / 100);
        }
    }

    // Buff for STANCE
    public class StanceBuff : ActiveBuff
    {
        public override void apply_to(CharStats stats, short value)
        {
            stats.set_stance((float)value / 100);
        }
    }

    // Buff for BOOSTER
    public class BoosterBuff : ActiveBuff
    {
        public override void apply_to(CharStats stats, short value)
        {
            stats.set_attackspeed((sbyte)value);
        }
    }

    public class FightSpiritBuff : ActiveBuff
    {
        public override void apply_to(CharStats stats, short value)
        {
            stats.set_attackspeed((sbyte)value);
        }
    }

    public class ComboBuff : ActiveBuff
    {
        private short state = -1;
        private Texture tex_state_0;
        private Texture tex_state_1;
        private Texture tex_state_2;
        private Texture tex_state_3;
        private Texture tex_state_4;
        private Texture tex_state_5;
        private List<Texture> stateTexture_List;
        //private List<SceneNode> stateSceneNode_List;
        private const string path_skill_111100 = "Skill.wz/111.img/skill/1111002";

        private short r = 61;
        private float _speed = 0.02f;
        private float _rotation = 0f;
        /*private SceneGraph _sceneGraph;
        private SceneNode _node_state_0;
        private SceneNode _node_state_1;
        private SceneNode _node_state_2;
        private SceneNode _node_state_3;
        private SceneNode _node_state_4;
        private SceneNode _node_state_5;*/

        public ComboBuff()
        {
            stateTexture_List = new List<Texture>();

            var wzObj_skill_1111002 = wz.wzFile_skill.GetObjectFromPath(path_skill_111100);

            tex_state_0 = wzObj_skill_1111002["state"]["0"];
            tex_state_1 = wzObj_skill_1111002["state"]["1"];
            tex_state_2 = wzObj_skill_1111002["state"]["2"];
            tex_state_3 = wzObj_skill_1111002["state"]["3"];
            tex_state_4 = wzObj_skill_1111002["state"]["4"];
            tex_state_5 = wzObj_skill_1111002["state"]["5"];

            stateTexture_List.Add(tex_state_0);
            stateTexture_List.Add(tex_state_1);
            stateTexture_List.Add(tex_state_2);
            stateTexture_List.Add(tex_state_3);
            stateTexture_List.Add(tex_state_4);
            stateTexture_List.Add(tex_state_5);

            /*stateSceneNode_List = new List<SceneNode>();
            _sceneGraph = new SceneGraph();
            _node_state_0 = new SceneNode("_node_state_0", new Vector2(0, 0));

            _node_state_1 = new SceneNode("_node_state_1", new Vector2(1, 0));
            _node_state_2 = new SceneNode("_node_state_2", new Vector2(0.309f, 0.951f));
            _node_state_3 = new SceneNode("_node_state_3", new Vector2(-0.809f, 0.588f));
            _node_state_4 = new SceneNode("_node_state_4", new Vector2(-0.809f, -0.588f));
            _node_state_5 = new SceneNode("_node_state_5", new Vector2(0.309f, -0.951f));

            _node_state_0.Children.Add(_node_state_1);
            _node_state_0.Children.Add(_node_state_2);
            _node_state_0.Children.Add(_node_state_3);
            _node_state_0.Children.Add(_node_state_4);
            _node_state_0.Children.Add(_node_state_5);

            _sceneGraph.RootNode.Children.Add(_node_state_0);

            stateSceneNode_List.Add(_node_state_0);
            stateSceneNode_List.Add(_node_state_1);
            stateSceneNode_List.Add(_node_state_2);
            stateSceneNode_List.Add(_node_state_3);
            stateSceneNode_List.Add(_node_state_4);
            stateSceneNode_List.Add(_node_state_5);*/
        }

        public override void OnAdd(CharStats stats, short value)
        {
            state = value;
            AppDebug.Log($"OnAdd combo value:{value}");
        }

        public override void OnUpdate(CharStats stats, short value)
        {
            if (state == -1)
            {
                return;
            }

            _rotation += _speed;

            var playerPos = Stage.Instance.get_player().absp;
            /*_node_state_0.Position = new Vector2(playerPos.x(), playerPos.y() - 45);
            _node_state_0.Rotation = _rotation;*/

        }

        public override void OnDraw(CharStats stats, short value)
        {
            if (state < 1)
            {
                return;
            }
            var playerPos = Stage.Instance.get_player().absp;

            /*var pos0 = stateSceneNode_List.TryGet(0).WorldPosition;
            DrawArgument d1 = new DrawArgument((short)(pos0.X), (short)(pos0.Y), _rotation,0.3f);
            stateTexture_List.TryGet(0)?.draw(d1);

            for (int i = 1; i <= state; i++)
            {

                var tempState = Math.Clamp(i, 1, 5);

                var tex = stateTexture_List.TryGet(tempState);
                var node = stateSceneNode_List.TryGet(tempState);

                if (tex != null && node != null)
                {
                    node.WorldMatrix.Transform(node.Position.X * r, node.Position.Y * r, out var nodeWorldPos);
                    DrawArgument d2 = new DrawArgument((short)(nodeWorldPos.X), (short)(nodeWorldPos.Y), -_rotation,1f);

                    tex.draw(d2);
                }

            }*/

            //AppDebug.Log($"OnAdd combo value:{value}");

        }
        public override void OnRemove(CharStats stats, short value)
        {
            AppDebug.Log($"OnAdd combo value:{value}");
            state = -1;
        }
    }
    public class ActiveBuffs
    {
        // Register all buffs effects
        public ActiveBuffs()
        {
            buffs[Buffstat.Id.MAPLE_WARRIOR] = new MapleWarriorBuff();
            buffs[Buffstat.Id.STANCE] = new StanceBuff();
            buffs[Buffstat.Id.BOOSTER] = new BoosterBuff();
            buffs[Buffstat.Id.WATK] = new SimpleStatBuff(EquipStat.Id.WATK);
            buffs[Buffstat.Id.WDEF] = new SimpleStatBuff(EquipStat.Id.WDEF);
            buffs[Buffstat.Id.MATK] = new SimpleStatBuff(EquipStat.Id.MAGIC);
            buffs[Buffstat.Id.MDEF] = new SimpleStatBuff(EquipStat.Id.MDEF);
            buffs[Buffstat.Id.SPEED] = new SimpleStatBuff(EquipStat.Id.SPEED);
            buffs[Buffstat.Id.JUMP] = new SimpleStatBuff(EquipStat.Id.JUMP);
            buffs[Buffstat.Id.HYPERBODYHP] = new PercentageStatBuff(EquipStat.Id.HP);
            buffs[Buffstat.Id.HYPERBODYMP] = new PercentageStatBuff(EquipStat.Id.MP);
            buffs[Buffstat.Id.COMBO] = new ComboBuff();
        }

        public void Add(CharStats stats, Buffstat.Id stat, short value)
        {
            if (buffs.TryGetValue(stat, out var activeBuff))
            {
                activeBuff?.OnAdd(stats, value);
            }
        }

        public void Update(CharStats stats, Buffstat.Id stat, short value)
        {
            if (buffs.TryGetValue(stat, out var activeBuff))
            {
                activeBuff?.OnUpdate(stats, value);
            }
        }

        public void Draw(CharStats stats, Buffstat.Id stat, short value)
        {
            if (buffs.TryGetValue(stat, out var activeBuff))
            {
                activeBuff?.OnDraw(stats, value);
            }
        }

        public void Remove(CharStats stats, Buffstat.Id stat, short value)
        {
            if (buffs.TryGetValue(stat, out var activeBuff))
            {
                activeBuff?.OnRemove(stats, value);
            }
        }

        // Return the buff effect associated with the buff stat
        public void apply_buff(CharStats stats, Buffstat.Id stat, short value)
        {
            var buff = buffs[stat];
            buff?.apply_to(stats, value);
        }

        private readonly EnumMap<Buffstat.Id, ActiveBuff> buffs = new EnumMap<Buffstat.Id, ActiveBuff>();
    }
}