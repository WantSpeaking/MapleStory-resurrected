using System.Collections;
using System.Collections.Generic;
using ms_Unity;
using server.quest;
using UnityEngine;

namespace ms
{
    public class MapNpcs
    {
        public MapNpcs() 
        {
            TestURPBatcher.Instance.StartCoroutine(SpawnNPC());
        }

        // Draw all NPCs on a layer
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
        public void draw(Layer.Id layer, double viewx, double viewy, float alpha)
        {
            npcs.draw(layer, viewx, viewy, alpha);
        }

        // Update all NPCs
        public void update(Physics physics)
        {
            npcs.update(physics);
        }

        public IEnumerator SpawnNPC()
        {
            while (true)
            {
                yield return null;
                Physics physics = Stage.get().get_Physics();

                for (; spawns.Count > 0; spawns.Dequeue())
                {
                    NpcSpawn spawn = spawns.Peek();

                    int oid = spawn.get_oid();
                    Optional<MapObject> npc = npcs.get(oid);

                    if (npc)
                    {
                        npc.get().makeactive();
                    }
                    else
                    {
                        npcs.add(spawn.instantiate(physics));
                    }
                    yield return new WaitForSecondsRealtime(GameUtil.Instance.spawnNPCInterval);
                }

                

            }
        }
        // Add an NPC to the spawn queue
        public void spawn(NpcSpawn spawn)
        {
            //onFirstSpawn (spawn);

            spawns.Enqueue((spawn));
        }

        // Remove the NPC with the specified oid
        public void remove(int oid)
        {
            var npc = npcs.get(oid);
            if (npc)
            {
                npc.get ().deactivate();
            }
        }

        // Remove all NPCs
        public void clear()
        {
            npcs.clear();
        }

        // Returns a reference to the MapObject's object
        public MapObjects get_npcs()
        {
            return npcs;
        }

        // Send mouse input to clickable NPCs
        public Cursor.State send_cursor(bool pressed, Point_short position, Point_short viewpos)
        {
            foreach (var map_object in npcs)
            {
                Npc npc = (Npc)map_object.Value;

                if (npc != null && npc.is_active() && npc.inrange(new Point_short(position), new Point_short(viewpos)))
                {
                    if (pressed)
                    {
                        // TODO: Try finding dialog first
                        if (npc.hasQuest())
						{
                            UI.get ().emplace<UINpcTalk> ();
                            UI.get ().get_element<UINpcTalk> ().get().InitChooseQuestSayPage(npc,npc.getInitPage ());
						}
                        else
						{
                            new TalkToNPCPacket (npc.get_oid ()).dispatch ();
                            FGUI_Manager.Instance.GetFGUI<FGUI_NpcTalk>().beginScriptTalk = true;
                        }

                        return Cursor.State.IDLE;
                    }
                    else
                    {
                        return Cursor.State.CANCLICK;
                    }
                }
            }

            return Cursor.State.IDLE;
        }

        public void UpdateQuest()
		{
			foreach (var pair in npcs)
			{
				if (pair.Value is Npc npc)
				{
                    npc.UpdateQuest (true);
                }
            }
		}

        bool isFirstSpawn = false;
        private void onFirstSpawn(NpcSpawn spawn)
		{
            if (isFirstSpawn == false)
			{
                isFirstSpawn = true;
                AppDebug.Log ($"npc {spawn.get_id()} onFirstSpawn");
            }

        }

        private MapObjects npcs = new MapObjects();

        private Queue<NpcSpawn> spawns = new Queue<NpcSpawn>();

        public void clearSpawns()
        {
            spawns.Clear();
        }
    }
}