using System;
using System.Collections.Generic;
using Loxodon.Framework.Observables;
using MapleLib.WzLib;

namespace ms
{
    public class UISkillBook : UIDragElement<PosSKILL>
    {
        public const Type TYPE = UIElement.Type.SKILLBOOK;
        public const bool FOCUSED = false;
        public const bool TOGGLED = true;

        public UISkillBook(params object[] args) : this((CharStats)args[0], (SkillBook)args[1])
        {
        }

        public UISkillBook(CharStats in_stats, SkillBook in_skillbook)
        {
            //this.UIDragElement<PosSKILL> = new <type missing>();
            this.stats = in_stats;
            this.skillbook = in_skillbook;
            this.grabbing = false;
            this.tab = 0;
            this.macro_enabled = false;
            this.sp_enabled = false;
            WzObject Skill = ms.wz.wzFile_ui["UIWindow2.img"]["Skill"];
            WzObject main = Skill["main"];
            WzObject ui_backgrnd = main["backgrnd"];

            bg_dimensions = new Texture(ui_backgrnd).get_dimensions();

            skilld = main["skill0"];
            skille = main["skill1"];
            skillb = main["skillBlank"];
            line = main["line"];

            buttons[(int)Buttons.BT_HYPER] = new MapleButton(main["BtHyper"]);
            buttons[(int)Buttons.BT_GUILDSKILL] = new MapleButton(main["BtGuildSkill"]);
            buttons[(int)Buttons.BT_RIDE] = new MapleButton(main["BtRide"]);
            buttons[(int)Buttons.BT_MACRO] = new MapleButton(main["BtMacro"]);

            buttons[(int)Buttons.BT_HYPER].set_state(Button.State.DISABLED);
            buttons[(int)Buttons.BT_GUILDSKILL].set_state(Button.State.DISABLED);
            buttons[(int)Buttons.BT_RIDE].set_state(Button.State.DISABLED);

            WzObject skillPoint = ms.wz.wzFile_ui["UIWindow4.img"]["Skill"]["skillPoint"];

            sp_backgrnd = skillPoint["backgrnd"];
            sp_backgrnd2 = skillPoint["backgrnd2"];
            sp_backgrnd3 = skillPoint["backgrnd3"];

            buttons[(int)Buttons.BT_CANCLE] = new MapleButton(skillPoint["BtCancle"], new Point_short(bg_dimensions.x(), 0));
            buttons[(int)Buttons.BT_OKAY] = new MapleButton(skillPoint["BtOkay"], new Point_short(bg_dimensions.x(), 0));
            buttons[(int)Buttons.BT_SPDOWN] = new MapleButton(skillPoint["BtSpDown"], new Point_short(bg_dimensions.x(), 0));
            buttons[(int)Buttons.BT_SPMAX] = new MapleButton(skillPoint["BtSpMax"], new Point_short(bg_dimensions.x(), 0));
            buttons[(int)Buttons.BT_SPUP] = new MapleButton(skillPoint["BtSpUp"], new Point_short(bg_dimensions.x(), 0));

            buttons[(int)Buttons.BT_SPDOWN].set_state(Button.State.DISABLED);

            sp_before = new Charset(skillPoint["num"], Charset.Alignment.RIGHT);
            sp_after = new Charset(skillPoint["num"], Charset.Alignment.RIGHT);
            sp_used = new Text(Text.Font.A12B, Text.Alignment.RIGHT, Color.Name.WHITE);
            sp_remaining = new Text(Text.Font.A12B, Text.Alignment.LEFT, Color.Name.SUPERNOVA);
            sp_name = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);

            sprites.Add(new Sprite(ui_backgrnd, new Point_short(1, 0)));
            sprites.Add(main["backgrnd2"]);
            sprites.Add(main["backgrnd3"]);

            WzObject macro = Skill["macro"];

            macro_backgrnd = macro["backgrnd"];
            macro_backgrnd2 = macro["backgrnd2"];
            macro_backgrnd3 = macro["backgrnd3"];

            buttons[(int)Buttons.BT_MACRO_OK] = new MapleButton(macro["BtOK"], new Point_short(bg_dimensions.x(), 0));

            buttons[(int)Buttons.BT_MACRO_OK].set_state(Button.State.DISABLED);

            WzObject close = ms.wz.wzFile_ui["Basic.img"]["BtClose3"];

            buttons[(int)Buttons.BT_CLOSE] = new MapleButton(close, new Point_short((short)(bg_dimensions.x() - 23), 6));

            WzObject Tab = main["Tab"];
            WzObject enabled = Tab["enabled"];
            WzObject disabled = Tab["disabled"];

            for (ushort i = (int)Buttons.BT_TAB0; i <= (int)Buttons.BT_TAB4; ++i)
            {
                ushort tabid = (ushort)(i - Buttons.BT_TAB0);
                buttons[i] = new TwoSpriteButton(disabled[tabid.ToString()], enabled[tabid.ToString()]);
            }

            ushort y_adj = 0;

            for (ushort i = (int)Buttons.BT_SPUP0; i <= (int)Buttons.BT_SPUP11; ++i)
            {
                ushort x_adj = 0;
                Buttons spupid = i - Buttons.BT_SPUP0;

                if ((ushort)spupid % 2 != 0)
                {
                    x_adj = (ushort)ROW_WIDTH;
                }

                Point_short spup_position = SKILL_OFFSET + new Point_short((short)(124 + x_adj), (short)(20 + y_adj));
                buttons[i] = new MapleButton(main["BtSpUp"], spup_position);

                if ((ushort)spupid % 2 != 0)
                {
                    y_adj += (ushort)ROW_HEIGHT;
                }
            }

            booktext = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE, "", 150);
            splabel = new Text(Text.Font.A12M, Text.Alignment.RIGHT, Color.Name.BLACK);

            slider = new Slider((int)Slider.Type.DEFAULT_SILVER, new Range_short(93, 317), 295, ROWS, 1, (bool upwards) =>
          {
              short shift = (short)(upwards ? -1 : 1);
              bool above = offset + shift >= 0;
              bool below = offset + 4 + shift <= skillcount;

              if (above && below)
              {
                  change_offset((ushort)(offset + shift));
              }
          });

            change_job(stats.get_stat(MapleStat.Id.JOB));

            set_macro(false);
            set_skillpoint(false);

            dimension = new Point_short(bg_dimensions);
            dragarea = new Point_short(dimension.x(), 20);
        }

        public override void draw(float alpha)
        {
            /*base.draw_sprites(alpha);

            bookicon.draw(position + new Point_short(11, 85));
            booktext.draw(position + new Point_short(173, 59));
            splabel.draw(position + new Point_short(304, 23));

            Point_short skill_position_l = position + SKILL_OFFSET + new Point_short(-1, 0);
            Point_short skill_position_r = position + SKILL_OFFSET + new Point_short(-1 + ROW_WIDTH, 0);

            //AppDebug.Log($"before position:{position}\t skill_position_l:{skill_position_l}\t skill_position_r:{skill_position_r}");
            for (int i = 0; i < ROWS; i++)
			{
				Point_short pos = new Point_short (skill_position_l);

				if (i % 2 != 0)
				{
					pos = new Point_short (skill_position_r);
				}

				if (i < skills.Count)
				{
					if (check_required (skills[(int)i].get_id ()))
					{
						skille.draw (pos);
					}
					else
					{
						skilld.draw (pos);
						skills[i].get_icon ().set_state (StatefulIcon.State.DISABLED);
					}

					skills[i].draw (pos + SKILL_META_OFFSET);
				}
				else
				{
					skillb.draw (pos);
				}

				if (i < ROWS - 2)
				{
					line.draw (pos + LINE_OFFSET);
				}

				if (i % 2 != 0)
				{
					skill_position_l.shift_y (ROW_HEIGHT);
					skill_position_r.shift_y (ROW_HEIGHT);
				}
			}
            //AppDebug.Log($"after position:{position}\t skill_position_l:{skill_position_l}\t skill_position_r:{skill_position_r}");

            *//*for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Point_short pos = new Point_short(skill_position_l);

                    pos += new Point_short((short)(32 * i), (short)(32 * j));

                    if (i % 2 == 0 || j % 2 == 0)
                    {
                        if (i*j < skills.Count)
                        {
                            if (check_required(skills[(int)(i*j)].get_id()))
                            {
                                //skille.draw (pos);
                            }
                            else
                            {
                                //skilld.draw (pos);
                                skills[i].get_icon().set_state(StatefulIcon.State.DISABLED);
                            }

                            skills[i*j].draw(pos + SKILL_META_OFFSET);
                        }
                    }


                }
            }*//*

            slider.draw(new Point_short(position));

            if (macro_enabled)
            {
                Point_short macro_pos = position + new Point_short(bg_dimensions.x(), 0);

                macro_backgrnd.draw(macro_pos + new Point_short(1, 0));
                macro_backgrnd2.draw(macro_pos);
                macro_backgrnd3.draw(macro_pos);
            }

            if (sp_enabled)
            {
                Point_short sp_pos = position + new Point_short(bg_dimensions.x(), 0);

                sp_backgrnd.draw(sp_pos);
                sp_backgrnd2.draw(sp_pos);
                sp_backgrnd3.draw(sp_pos);

                Point_short sp_level_pos = sp_pos + new Point_short(78, 149);

                sp_before.draw(sp_before_text, 12, sp_level_pos);
                sp_after.draw(sp_after_text, 11, sp_level_pos + new Point_short(78, 0));
                sp_used.draw(sp_pos + new Point_short(82, 87));
                sp_remaining.draw(sp_pos + new Point_short(76, 65));
                sp_name.draw(sp_pos + new Point_short(97, 35));
                sp_skill.draw(sp_pos + new Point_short(13, 31));
            }

            base.draw_buttons(alpha);*/
        }

        public override void toggle_active()
        {
            if (!is_skillpoint_enabled())
            {
                base.toggle_active();

                clear_tooltip();
            }
        }

        public override void doubleclick(Point_short cursorpos)
        {
            SkillDisplayMeta skill = skill_by_position(cursorpos - position);

            if (skill != null)
            {
                int skill_id = skill.get_id();
                int skill_level = skillbook.get_level(skill_id);

                if (skill_level > 0)
                {
                    Stage.get().get_combat().use_move(skill_id);
                }
            }
        }

        public override void remove_cursor()
        {
            base.remove_cursor();

            slider.remove_cursor();
        }

        static readonly Rectangle_short bounds = new Rectangle_short(0, 32, 0, 32);

        public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
        {
            Cursor.State dstate = base.send_cursor(clicked, new Point_short(cursorpos));

            if (dragged)
            {
                return dstate;
            }

            Point_short cursor_relative = cursorpos - position;

            if (slider.isenabled())
            {
                Cursor.State new_state = slider.send_cursor(new Point_short(cursor_relative), clicked);
                if (new_state != Cursor.State.IDLE)
                {
                    clear_tooltip();

                    return new_state;
                }
            }

            Point_short skill_position_l = position + SKILL_OFFSET + new Point_short(-1, 0);
            Point_short skill_position_r = position + SKILL_OFFSET + new Point_short(-1 + ROW_WIDTH, 0);

            if (!grabbing)
            {
                for (int i = 0; i < skills.Count; i++)
                {
                    Point_short skill_position = new Point_short(skill_position_l);

                    if (i % 2 != 0)
                    {
                        skill_position = new Point_short(skill_position_r);
                    }

                    bool inrange = bounds.contains(cursorpos - skill_position);

                    if (inrange)
                    {
                        if (clicked)
                        {
                            clear_tooltip();
                            grabbing = true;

                            int skill_id = skills[i].get_id();
                            int skill_level = skillbook.get_level(skill_id);

                            if (skill_level > 0 && !SkillData.get(skill_id).is_passive())
                            {
                                skills[i].get_icon().start_drag(cursorpos - skill_position);
                                UI.get().drag_icon(skills[i].get_icon());

                                return Cursor.State.GRABBING;
                            }
                            else
                            {
                                return Cursor.State.IDLE;
                            }
                        }
                        else
                        {
                            skills[i].get_icon().set_state(StatefulIcon.State.MOUSEOVER);
                            show_skill(skills[i].get_id());

                            return Cursor.State.IDLE;
                        }
                    }

                    if (i % 2 != 0)
                    {
                        skill_position_l.shift_y(ROW_HEIGHT);
                        skill_position_r.shift_y(ROW_HEIGHT);
                    }
                }

                for (int i = 0; i < skills.Count; i++)
                {
                    skills[i].get_icon().set_state(StatefulIcon.State.NORMAL);
                }

                clear_tooltip();
            }
            else
            {
                grabbing = false;
            }

            return base.send_cursor(clicked, new Point_short(cursorpos));
        }

        public override void send_key(int keycode, bool pressed, bool escape)
        {
            if (pressed)
            {
                if (escape)
                {
                    if (sp_enabled)
                    {
                        set_skillpoint(false);
                    }
                    else
                    {
                        close();
                    }
                }
                else if (keycode == (int)KeyAction.Id.TAB)
                {
                    clear_tooltip();

                    Job.Level level = job.get_level();
                    var id = tab + 1;
                    var new_tab = tab + Buttons.BT_TAB0;

                    if (new_tab < Buttons.BT_TAB4 && id <= (int)level)
                    {
                        new_tab++;
                    }
                    else
                    {
                        new_tab = Buttons.BT_TAB0;
                    }

                    change_tab(new_tab - Buttons.BT_TAB0);
                }
            }
        }

        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public void update_stat(MapleStat.Id stat, short value)
        {
            switch (stat)
            {
                case MapleStat.Id.JOB:
                    change_job((ushort)value);
                    break;
                case MapleStat.Id.SP:
                    change_sp();
                    break;
            }
        }

        public void update_skills(int skill_id)
        {
            change_tab(tab);
        }

        public bool is_skillpoint_enabled()
        {
            return sp_enabled;
        }

        public override Button.State button_pressed(ushort id)
        {
            var cur_sp = Convert.ToInt32(splabel.get_text());

            switch ((Buttons)id)
            {
                case Buttons.BT_CLOSE:
                    close();
                    break;
                case Buttons.BT_MACRO:
                    set_macro(!macro_enabled);
                    break;
                case Buttons.BT_CANCLE:
                    set_skillpoint(false);
                    break;
                case Buttons.BT_OKAY:
                    {
                        int used = Convert.ToInt32(sp_used.get_text());

                        while (used > 0)
                        {
                            spend_sp(sp_id);
                            used--;
                        }

                        change_sp();
                        set_skillpoint(false);
                    }
                    break;
                case Buttons.BT_SPDOWN:
                    {
                        int used = Convert.ToInt32(sp_used.get_text());
                        int sp_after = Convert.ToInt32(sp_after_text);
                        int sp_before = Convert.ToInt32(sp_before_text);
                        used--;
                        sp_after--;

                        sp_after_text = Convert.ToString(sp_after);
                        sp_used.change_text(Convert.ToString(used));
                        sp_remaining.change_text(Convert.ToString(cur_sp - used));

                        buttons[(int)Buttons.BT_SPUP].set_state(Button.State.NORMAL);
                        buttons[(int)Buttons.BT_SPMAX].set_state(Button.State.NORMAL);

                        if (sp_after - 1 == sp_before)
                        {
                            return Button.State.DISABLED;
                        }

                        return Button.State.NORMAL;
                    }
                    break;
                case Buttons.BT_SPMAX:
                    {
                        int used = Convert.ToInt32(sp_used.get_text());
                        int sp_before = Convert.ToInt32(sp_before_text);
                        int sp_touse = sp_masterlevel - sp_before - used;

                        used += sp_touse;

                        sp_after_text = Convert.ToString(sp_masterlevel);
                        sp_used.change_text(Convert.ToString(used));
                        sp_remaining.change_text(Convert.ToString(cur_sp - used));

                        buttons[(int)Buttons.BT_SPUP].set_state(Button.State.DISABLED);
                        buttons[(int)Buttons.BT_SPDOWN].set_state(Button.State.NORMAL);

                        return Button.State.DISABLED;
                    }
                    break;
                case Buttons.BT_SPUP:
                    {
                        int used = Convert.ToInt32(sp_used.get_text());
                        int sp_after = Convert.ToInt32(sp_after_text);
                        used++;
                        sp_after++;

                        sp_after_text = Convert.ToString(sp_after);
                        sp_used.change_text(Convert.ToString(used));
                        sp_remaining.change_text(Convert.ToString(cur_sp - used));

                        buttons[(int)Buttons.BT_SPDOWN].set_state(Button.State.NORMAL);

                        if (sp_after == sp_masterlevel)
                        {
                            buttons[(int)Buttons.BT_SPMAX].set_state(Button.State.DISABLED);

                            return Button.State.DISABLED;
                        }

                        return Button.State.NORMAL;
                    }
                    break;
                case Buttons.BT_TAB0:
                case Buttons.BT_TAB1:
                case Buttons.BT_TAB2:
                case Buttons.BT_TAB3:
                case Buttons.BT_TAB4:
                    change_tab((ushort)(id - Buttons.BT_TAB0));

                    return Button.State.PRESSED;
                case Buttons.BT_SPUP0:
                case Buttons.BT_SPUP1:
                case Buttons.BT_SPUP2:
                case Buttons.BT_SPUP3:
                case Buttons.BT_SPUP4:
                case Buttons.BT_SPUP5:
                case Buttons.BT_SPUP6:
                case Buttons.BT_SPUP7:
                case Buttons.BT_SPUP8:
                case Buttons.BT_SPUP9:
                case Buttons.BT_SPUP10:
                case Buttons.BT_SPUP11:
                    send_spup((ushort)(id - Buttons.BT_SPUP0 + offset));
                    break;
                case Buttons.BT_HYPER:
                case Buttons.BT_GUILDSKILL:
                case Buttons.BT_RIDE:
                case Buttons.BT_MACRO_OK:
                default:
                    break;
            }

            return Button.State.NORMAL;
        }

        public override void OnActivityChange (bool isActiveAfterChange)
        {
            if (isActiveAfterChange)
            {
                ms_Unity.FGUI_Manager.Instance.OpenFGUI<ms_Unity.FGUI_SkillBook> ().OnVisiblityChanged(true);
            }
            else
            {
                ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_SkillBook> ().OnVisiblityChanged (false);
            }
        }

        private class SkillIcon : StatefulIcon.Type
        {
            public SkillIcon(int id)
            {
                this.skill_id = id;
            }

            public override void drop_on_stage()
            {
            }

            public override void drop_on_equips(EquipSlot.Id UnnamedParameter1)
            {
            }

            public override bool drop_on_items(InventoryType.Id UnnamedParameter1, EquipSlot.Id UnnamedParameter2, short UnnamedParameter3, bool UnnamedParameter4)
            {
                return true;
            }

            public override void drop_on_bindings(Point_short cursorposition, bool remove)
            {
                var keyconfig = UI.get().get_element<UIKeyConfig>();
                Keyboard.Mapping mapping = new Keyboard.Mapping(KeyType.Id.SKILL, skill_id);

                if (remove)
                {
                    keyconfig.get().unstage_mapping(mapping);
                }
                else
                {
                    keyconfig.get().stage_mapping(cursorposition, mapping);
                }
            }

            public override void set_count(short UnnamedParameter1)
            {
            }

            public override void set_state(StatefulIcon.State UnnamedParameter1)
            {
            }

            public override Icon.IconType get_type()
            {
                return Icon.IconType.SKILL;
            }

            private int skill_id;
        }

        public class SkillDisplayMeta
        {
            public SkillDisplayMeta(int i, int l)
            {
                this.id = i;
                this.level = l;
                SkillData data = SkillData.get(id);

                Texture ntx = new Texture(data.get_icon(SkillData.Icon.NORMAL));//同一张 Texture 还画在ToolTips上 位置就不对了，必须要new
                Texture dtx = new Texture(data.get_icon(SkillData.Icon.DISABLED));
                Texture motx = new Texture(data.get_icon(SkillData.Icon.MOUSEOVER));
                icon = new StatefulIcon(new SkillIcon(id), ntx, dtx, motx);
                nTexture_Icon_Normal = new FairyGUI.NTexture (ntx.texture2D);

                namestr = data.get_name();
                levelstr = Convert.ToString(level);

                name_text = new Text(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR, namestr);
                level_text = new Text(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR, levelstr);
                descstr = data.get_desc ();
/*                const ushort MAX_NAME_WIDTH = 97;
                int overhang = 3;

                while (name_text.Width() > MAX_NAME_WIDTH)
                {
                    namestr = namestr.Remove (namestr.end () - overhang, overhang);
                        namestr = namestr.Insert(namestr.end() - overhang, "..");
                    overhang += 1;

                    name_text.change_text(namestr);
                }*/
            }

            public void draw(DrawArgument args)
            {
                icon.draw(args.getpos());
                name_text.draw(args + new Point_short(38, -5));
                level_text.draw(args + new Point_short(38, 13));
            }

            public int get_id()
            {
                return id;
            }

            public int get_level()
            {
                return level;
            }

            public StatefulIcon get_icon()
            {
                return icon;
            }

            private int id;
            private int level;
            private StatefulIcon icon;
            private Text name_text;
            private Text level_text;
            string namestr;
            string levelstr;

            public FairyGUI.NTexture nTexture_Icon_Normal;
            public string get_namestr ()
            {
                return namestr;
            }

            public string get_levelstr ()
            {
                return levelstr;
            }
            public string descstr { get; set; }
			public string get_level_desc ()
			{
                return SkillData.get(id)?.get_level_desc(level);
			}

            public string get_full_desc ()
			{
                return @$"{get_level_desc ()} /n {descstr}";
			}
      

		}

        private void change_job(ushort id)
        {
            job.change_job(id);

            Job.Level level = job.get_level();

            for (ushort i = 0; i <= (int)Job.Level.FOURTH; i++)
            {
                buttons[(uint)((int)Buttons.BT_TAB0 + i)].set_active(i <= (int)level);
            }

            change_tab(level - Job.Level.BEGINNER);

        }

        public void change_sp()
        {
            Job.Level joblevel = joblevel_by_tab(tab);
            ushort level = stats.get_stat(MapleStat.Id.LEVEL);

            if (joblevel == Job.Level.BEGINNER)
            {
                var remaining_beginner_sp = 0;

                if (level >= 7)
                {
                    remaining_beginner_sp = 6;
                }
                else
                {
                    remaining_beginner_sp = (short)(level - 1);
                }

                for (int i = 0; i < skills.Count; i++)
                {
                    int skillid = skills[i].get_id();

                    if (skillid == (int)SkillId.Id.THREE_SNAILS || skillid == (int)SkillId.Id.HEAL || skillid == (int)SkillId.Id.FEATHER)
                    {
                        remaining_beginner_sp -= skills[i].get_level();
                    }
                }

                beginner_sp = (short)remaining_beginner_sp;
                splabel.change_text(Convert.ToString(beginner_sp));
            }
            else
            {
                sp = (short)stats.get_stat(MapleStat.Id.SP);
                splabel.change_text(Convert.ToString(sp));
            }

            change_offset(offset);
            set_skillpoint(false);
        }

        public void change_tab(ushort new_tab)
        {
            buttons[(uint)((int)Buttons.BT_TAB0 + tab)].set_state(Button.State.NORMAL);
            buttons[(uint)((int)Buttons.BT_TAB0 + new_tab)].set_state(Button.State.PRESSED);
            tab = new_tab;

            skills.Clear();
            skillcount = 0;

            Job.Level joblevel = joblevel_by_tab(tab);
            ushort subid = job.get_subjob(joblevel);

            JobData data = JobData.get(subid);

            bookicon = new Texture(data.get_icon());
            booktext.change_text(data.get_name());

            foreach (int skill_id in data.get_skills())
            {
                int level = skillbook.get_level(skill_id);
                int masterlevel = skillbook.get_masterlevel(skill_id);

                bool invisible = SkillData.get(skill_id).is_invisible();

                if (invisible /*&& masterlevel == 0*/)
                {
                    continue;
                }

                skills.Add(new SkillDisplayMeta(skill_id, level));
                skillcount++;
            }

            slider.setrows(ROWS, (short)skillcount);
            change_offset(0);
            change_sp();
        }

        private void change_offset(ushort new_offset)
        {
            offset = new_offset;

            for (short i = 0; i < ROWS; i++)
            {
                ushort index = (ushort)((int)Buttons.BT_SPUP0 + i);
                ushort row = (ushort)(offset + i);
                buttons[index].set_active(row < skillcount);

                if (row < skills.Count)
                {
                    int skill_id = skills[row].get_id();
                    bool canraise = can_raise(skill_id);
                    buttons[index].set_state(canraise ? Button.State.NORMAL : Button.State.DISABLED);
                }
            }
        }

        private void show_skill(int id)
        {
/*            int skill_id = id;
            int level = skillbook.get_level(id);
            int masterlevel = skillbook.get_masterlevel(id);
            long expiration = skillbook.get_expiration(id);

            UI.get().show_skill(Tooltip.Parent.SKILLBOOK, skill_id, level, masterlevel, expiration);*/
        }

        private void clear_tooltip()
        {
            UI.get().clear_tooltip(Tooltip.Parent.SKILLBOOK);
        }

        public bool can_raise(int skill_id)
        {
            Job.Level joblevel = joblevel_by_tab(tab);

            if (joblevel == Job.Level.BEGINNER && beginner_sp <= 0)
            {
                return false;
            }

            if (tab + Buttons.BT_TAB0 != Buttons.BT_TAB0 && sp <= 0)
            {
                return false;
            }

            int level = skillbook.get_level(skill_id);
            int masterlevel = skillbook.get_masterlevel(skill_id);

            if (masterlevel == 0)
            {
                masterlevel = SkillData.get(skill_id).get_masterlevel();
            }

            if (level >= masterlevel)
            {
                return false;
            }

            switch ((SkillId.Id)skill_id)
            {
                case SkillId.Id.ANGEL_BLESSING:
                    return false;
                default:
                    return check_required(skill_id);
            }
        }

        private void send_spup(ushort row)
        {
            if (row >= skills.Count)
            {
                return;
            }

            int id = skills[row].get_id();

            if (sp_enabled && id == sp_id)
            {
                set_skillpoint(false);
                return;
            }

            int level = skills[row].get_level();
            int used = 1;

            SkillData skillData = SkillData.get(id);
            string name = skillData.get_name();
            var cur_sp = Convert.ToInt32(splabel.get_text());

            sp_before_text = Convert.ToString(level);
            sp_after_text = Convert.ToString(level + used);
            sp_used.change_text(Convert.ToString(used));
            sp_remaining.change_text(Convert.ToString(cur_sp - used));
            sp_name.change_text(name);
            sp_skill = new Texture(skills[row].get_icon().get_texture());
            sp_id = id;
            sp_masterlevel = skillData.get_masterlevel();

            if (sp_masterlevel == 1)
            {
                buttons[(int)Buttons.BT_SPDOWN].set_state(Button.State.DISABLED);
                buttons[(int)Buttons.BT_SPMAX].set_state(Button.State.DISABLED);
                buttons[(int)Buttons.BT_SPUP].set_state(Button.State.DISABLED);
            }
            else
            {
                buttons[(int)Buttons.BT_SPDOWN].set_state(Button.State.DISABLED);
                buttons[(int)Buttons.BT_SPMAX].set_state(Button.State.NORMAL);
                buttons[(int)Buttons.BT_SPUP].set_state(Button.State.NORMAL);
            }

            if (!sp_enabled)
            {
                set_skillpoint(true);
            }
        }

        public void spend_sp(int skill_id)
        {
            new SpendSpPacket(skill_id).dispatch();

            UI.get().disable();
        }

        private Job.Level joblevel_by_tab(ushort t)
        {
            switch (t)
            {
                case 1:
                    return Job.Level.FIRST;
                case 2:
                    return Job.Level.SECOND;
                case 3:
                    return Job.Level.THIRD;
                case 4:
                    return Job.Level.FOURTH;
                default:
                    return Job.Level.BEGINNER;
            }
        }

        private UISkillBook.SkillDisplayMeta skill_by_position(Point_short cursorpos)
        {
            short x = cursorpos.x();

            if (x < SKILL_OFFSET.x() || x > SKILL_OFFSET.x() + 2 * ROW_WIDTH)
            {
                return null;
            }

            short y = cursorpos.y();

            if (y < SKILL_OFFSET.y())
            {
                return null;
            }

            var row = (y - SKILL_OFFSET.y()) / ROW_HEIGHT;

            if (row < 0 || row >= ROWS)
            {
                return null;
            }

            var offset_row = offset + row;

            if (offset_row >= ROWS)
            {
                return null;
            }

            var col = (x - SKILL_OFFSET.x()) / ROW_WIDTH;

            var skill_idx = 2 * offset_row + col;

            if (skill_idx >= skills.Count)
            {
                return null;
            }

            var iter = skills[skill_idx];

            return iter;
        }

        private void close()
        {
            clear_tooltip();
            deactivate();
        }

        private bool check_required(int id)
        {
            Dictionary<int, int> required = skillbook.collect_required(id);

            if (required.Count <= 0)
            {
                required = new Dictionary<int, int>(SkillData.get(id).get_reqskills());
            }

            foreach (var reqskill in required)
            {
                int reqskill_level = skillbook.get_level(reqskill.Key);
                int req_level = reqskill.Value;

                if (reqskill_level < req_level)
                {
                    return false;
                }
            }

            return true;
        }

        private void set_macro(bool enabled)
        {
            macro_enabled = enabled;

            if (macro_enabled)
            {
                dimension = bg_dimensions + new Point_short(macro_backgrnd.get_dimensions().x(), 0);
            }
            else if (!sp_enabled)
            {
                dimension = new Point_short(bg_dimensions);
            }

            buttons[(int)Buttons.BT_MACRO_OK].set_active(macro_enabled);

            if (macro_enabled && sp_enabled)
            {
                set_skillpoint(false);
            }
        }

        private void set_skillpoint(bool enabled)
        {
            sp_enabled = enabled;

            if (sp_enabled)
            {
                dimension = bg_dimensions + new Point_short(sp_backgrnd.get_dimensions().x(), 0);
            }
            else if (!macro_enabled)
            {
                dimension = new Point_short(bg_dimensions);
            }

            buttons[(int)Buttons.BT_CANCLE].set_active(sp_enabled);
            buttons[(int)Buttons.BT_OKAY].set_active(sp_enabled);
            buttons[(int)Buttons.BT_SPDOWN].set_active(sp_enabled);
            buttons[(int)Buttons.BT_SPMAX].set_active(sp_enabled);
            buttons[(int)Buttons.BT_SPUP].set_active(sp_enabled);

            if (sp_enabled && macro_enabled)
            {
                set_macro(false);
            }
        }

        ms_Unity.FGUI_SkillBook fGUI_SkillBook;
        public void Set_FGUI_SkillBook(ms_Unity.FGUI_SkillBook fGUI_SkillBook)
		{
            this.fGUI_SkillBook = fGUI_SkillBook;
        }
        public enum Buttons : ushort
        {
            BT_CLOSE,
            BT_HYPER,
            BT_GUILDSKILL,
            BT_RIDE,
            BT_MACRO,
            BT_MACRO_OK,
            BT_CANCLE,
            BT_OKAY,
            BT_SPDOWN,
            BT_SPMAX,
            BT_SPUP,
            BT_TAB0,
            BT_TAB1,
            BT_TAB2,
            BT_TAB3,
            BT_TAB4,
            BT_SPUP0,
            BT_SPUP1,
            BT_SPUP2,
            BT_SPUP3,
            BT_SPUP4,
            BT_SPUP5,
            BT_SPUP6,
            BT_SPUP7,
            BT_SPUP8,
            BT_SPUP9,
            BT_SPUP10,
            BT_SPUP11
        }

        private const short ROWS = 12;
        private const short ROW_HEIGHT = 40;
        private const short ROW_WIDTH = 143;
        private static readonly Point_short SKILL_OFFSET = new Point_short(11, 93);
        private static readonly Point_short SKILL_META_OFFSET = new Point_short(2, 2);
        private static readonly Point_short LINE_OFFSET = new Point_short(0, 37);

        private readonly CharStats stats;
        private readonly SkillBook skillbook;

        private Slider slider = new Slider();
        private Texture skille = new Texture();
        private Texture skilld = new Texture();
        private Texture skillb = new Texture();
        private Texture line = new Texture();
        private Texture bookicon = new Texture();
        private Text booktext = new Text();
        public Text splabel = new Text();

        public Job job = new Job();
        private short sp;
        private short beginner_sp;

        private ushort tab;
        private ushort skillcount;
        private ushort offset;

        public ObservableList<SkillDisplayMeta> skills = new ObservableList<SkillDisplayMeta>();
        
        private bool grabbing;

        private Point_short bg_dimensions = new Point_short();

        private bool macro_enabled;
        private Texture macro_backgrnd = new Texture();
        private Texture macro_backgrnd2 = new Texture();
        private Texture macro_backgrnd3 = new Texture();

        private bool sp_enabled;
        private Texture sp_backgrnd = new Texture();
        private Texture sp_backgrnd2 = new Texture();
        private Texture sp_backgrnd3 = new Texture();
        private Charset sp_before = new Charset();
        private Charset sp_after = new Charset();
        private string sp_before_text;
        private string sp_after_text;
        private Text sp_used = new Text();
        private Text sp_remaining = new Text();
        private Text sp_name = new Text();
        private Texture sp_skill = new Texture();
        private int sp_id;
        private int sp_masterlevel;
    }
}