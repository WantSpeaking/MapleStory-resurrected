using System;
using System.Collections.Generic;





namespace ms
{
    public class CharLook
    {
        public CharLook(LookEntry entry)
        {
            reset();

            set_body(entry.skin);
            set_hair(entry.hairid);
            set_face(entry.faceid);
            //AppDebug.Log($"CharLook entry.equips:{entry.equips.ToDebugLog()}");

            foreach (var equip in entry.equips)
            {
                add_equip(equip.Value);
            }
        }

        public CharLook()
        {
            reset();

            body = null;
            hair = null;
            face = null;
        }

        public void reset()
        {
            flip = true;

            action = null;
            actionstr = "";
            actframe = 0;

            set_stance(Stance.Id.STAND1);
            stframe.set(0);
            stelapsed = 0;

            set_expression(Expression.Id.DEFAULT);
            expframe.set(0);
            expelapsed = 0;
        }

        private void draw(DrawArgument args, Stance.Id interstance, Expression.Id interexpression, byte interframe, byte interexpframe)
        {
           
      /*      if (lastDraw_interframe != -1)
            {
                erase(lastDraw_args, lastDraw_interstance, lastDraw_interexpression, (byte)lastDraw_interframe, lastDraw_interexpframe);
            }*/

            Point_short faceshift = drawinfo.getfacepos(interstance, interframe, args.FlipX);
            DrawArgument faceargs = args + faceshift;

            if (Stance.is_climbing(interstance))
            {
                body.draw(interstance, Body.Layer.BODY, interframe, args.SetOrderInLayer(256));
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer(1));

                switch (equips.getcaptype())
                {
                    case CharEquips.CapType.NONE:
                        hair.draw(interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer(1));
                        break;
                    case CharEquips.CapType.HEADBAND:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1));
                        hair.draw(interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer(1));
                        break;
                    case CharEquips.CapType.HALFCOVER:
                        hair.draw(interstance, Hair.Layer.BELOWCAP, interframe, args.IncreaseOrderInLayer(1));
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1));
                        break;
                    case CharEquips.CapType.FULLCOVER:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1));
                        break;
                }

                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.BACKSHIELD, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.BACKWEAPON, interframe, args.IncreaseOrderInLayer(1));
            }
            else
            {
                hair.draw(interstance, Hair.Layer.BELOWBODY, interframe, args.SetOrderInLayer(256));
                equips.draw(EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_BELOW_BODY, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_BODY, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_BELOW_BODY, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.BODY, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_BODY, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_BODY, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.ARM_BELOW_HEAD, interframe, args.IncreaseOrderInLayer(1));

                if (equips.has_overall())
                {
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer(1));
                }
                else
                {
                    equips.draw(EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer(1));
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer(1));
                }

                body.draw(interstance, Body.Layer.ARM_BELOW_HEAD_OVER_MAIL, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer(1));
                hair.draw(interstance, Hair.Layer.SHADE, interframe, args.IncreaseOrderInLayer(1));
                hair.draw(interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer(1));
                face.draw(interexpression, interexpframe, faceargs.SetOrderInLayer(args.orderInLayer).IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.FACE, interstance, Clothing.Layer.FACEACC, 0, faceargs.SetOrderInLayer(args.orderInLayer).IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.EYEACC, interstance, Clothing.Layer.EYEACC, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD, interframe, args.IncreaseOrderInLayer(1));

                switch (equips.getcaptype())
                {
                    case CharEquips.CapType.NONE:
                        hair.draw(interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer(1));
                        break;
                    case CharEquips.CapType.HEADBAND:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1));
                        hair.draw(interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer(1));
                        hair.draw(interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer(1));
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1));
                        break;
                    case CharEquips.CapType.HALFCOVER:
                        hair.draw(interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer(1));
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1));
                        break;
                    case CharEquips.CapType.FULLCOVER:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1));
                        break;
                }

                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_ARM, interframe, args.IncreaseOrderInLayer(1));
                bool twohanded = is_twohanded(interstance);

                if (twohanded)
                {
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer(1));
                    body.draw(interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer(1));
                    equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer(1));
                }
                else
                {
                    equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer(1));
                    body.draw(interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer(1));
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer(1));
                }

                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_GLOVE, interframe, args.IncreaseOrderInLayer(1));

                body.draw(interstance, Body.Layer.HAND_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer(1));

                body.draw(interstance, Body.Layer.ARM_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.ARM_OVER_HAIR_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_HAND, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_BODY, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.HAND_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1));
                body.draw(interstance, Body.Layer.HAND_OVER_WEAPON, interframe, args.IncreaseOrderInLayer(1));

                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1));
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1));
            }

            lastDraw_args = args;
            lastDraw_interstance = interstance;
            lastDraw_interexpression = interexpression;
            lastDraw_interframe = (sbyte)interframe;
            lastDraw_interexpframe = interexpframe;
        }

        public void draw(DrawArgument args, float alpha)
        {
            //AppDebug.Log($"args.get_xscale:{args.get_xscale ()}\t flip:{flip}");
            if (body == null || hair == null || face == null)
            {
                return;
            }

            Point_short acmove = new Point_short();

            if (action != null)
            {
                acmove = new Point_short (action.get_move());
            }

            DrawArgument relargs = new DrawArgument(new Point_short(acmove), flip);
            //AppDebug.Log ($"charLook args.FlipX: {args.FlipX}\t relargs:{relargs.FlipX}\t +;{(relargs + args).FlipX}\t this:{flip}");
            //AppDebug.Log ($"charLook args.FlipX: {args.get_xscale ()}\t relargs:{relargs.get_xscale ()}\t +;{(relargs + args).get_xscale ()}\t this:{flip}");
            Stance.Id interstance = (Stance.Id)stance.get(alpha);
            Expression.Id interexpression = (Expression.Id)expression.get(alpha);
            byte interframe = stframe.get(alpha);
            byte interexpframe = expframe.get(alpha);
            //AppDebug.Log ($"alpha:{alpha} \t interframe:{interframe}");
            switch (interstance)
            {
                case Stance.Id.STAND1:
                case Stance.Id.STAND2:
                    {
                        if ((bool)alerted)
                        {
                            interstance = Stance.Id.ALERT;
                        }

                        break;
                    }
            }

            draw(relargs + args, interstance, interexpression, interframe, interexpframe);
        }

        public void draw(Point_short position, bool flipped, Stance.Id interstance, Expression.Id interexpression)
        {
            interstance = equips.adjust_stance(interstance);
            draw(new DrawArgument(position, flipped), interstance, interexpression, 0, 0);
        }

        public Action OnActionChanged;
        public bool update(ushort timestep)
        {
            //AppDebug.Log ($"now:{stance.get()}\t last:{stance.last ()}");

            if (timestep == 0)
            {
                stance.normalize();
                stframe.normalize();
                expression.normalize();
                expframe.normalize();
                return false;
            }

            alerted.update();
            expcooldown.update();

            bool aniend = false;

            if (action == null)
            {
                ushort delay = get_delay((Stance.Id)stance.get(), stframe.get());//500
                ushort delta = (ushort)(delay - stelapsed);//5

                if (timestep >= delta)
                {
                    stelapsed = (ushort)(timestep - delta);//3

                    byte nextframe = getnextframe((Stance.Id)stance.get(), stframe.get());
                    float threshold = (float)delta / timestep;//5/8
                    stframe.next(nextframe, threshold);

                    if (stframe == 0)
                    {
                        aniend = true;
                    }
                }
                else
                {
                    stance.normalize();
                    stframe.normalize();

                    stelapsed += timestep;
                }
            }
            else
            {
                ushort delay = action.get_delay();//240
                ushort delta = (ushort)(delay - stelapsed);//5

                if (timestep >= delta)
                {
                    stelapsed = (ushort)(timestep - delta);//8-5=3
                    actframe = drawinfo.next_actionframe(actionstr, actframe);

                    if (actframe > 0)
                    {
                        action = drawinfo.get_action(actionstr, actframe);

                        float threshold = (float)delta / timestep;//5/8
                        stance.next((short)action.get_stance(), threshold);
                        stframe.next(action.get_frame(), threshold);//after image 在相应的frame才会 播放 

                        OnActionChanged?.Invoke ();
                    }
                    else
                    {
                        aniend = true;
                        action = null;
                        actionstr = "";
                        set_stance(Stance.Id.STAND1);
                    }
                }
                else
                {
                    stance.normalize();
                    stframe.normalize();

                    stelapsed += timestep;
                }

            }

            ushort expdelay = (ushort)face.get_delay((Expression.Id)expression.get(), expframe.get());
            ushort expdelta = (ushort)(expdelay - expelapsed);

            if (timestep >= expdelta)
            {
                expelapsed = (ushort)(timestep - expdelta);

                byte nextexpframe = face.nextframe((Expression.Id)expression.get(), expframe.get());
                float fcthreshold = (float)expdelta / timestep;
                expframe.next(nextexpframe, fcthreshold);

                if (expframe == 0)
                {
                    if (expression == (int)Expression.Id.DEFAULT)
                    {
                        expression.next((int)Expression.Id.BLINK, fcthreshold);
                    }
                    else
                    {
                        expression.next((int)Expression.Id.DEFAULT, fcthreshold);
                    }
                }
            }
            else
            {
                expression.normalize();
                expframe.normalize();

                expelapsed += timestep;
            }

		
            return aniend;
        }

        public void set_body(int skin_id)
        {

            /*		if (bodytypes.TryGetValue (skin_id, out body)) return;
					body = new Body (skin_id, drawinfo);
					bodytypes.Add (skin_id, body);*/

            if (!bodytypes.TryGetValue(skin_id, out body))
            {
                body = new Body(skin_id, drawinfo);
                bodytypes.Add(skin_id, body);
            }

        }

        public void set_hair(int hair_id)
        {
            /*if (!hairstyles.ContainsKey (hair_id))
			{
				var hair = new Hair (hair_id,drawinfo);p
				hairstyles.Add (hair_id,hair);
			}*/
            if (hairstyles.TryGetValue(hair_id, out hair)) return;
            hair = new Hair(hair_id, drawinfo);
            hairstyles.Add(hair_id, hair);

        }

        public void set_face(int face_id)
        {
            /*if (!facetypes.ContainsKey (face_id))
			{
				var face = new Face (face_id);
				facetypes.Add (face_id,face);
			}*/
            if (facetypes.TryGetValue(face_id, out face)) return;
            face = new Face(face_id);
            facetypes.Add(face_id, face);

  
        }

        private void updatetwohanded()
        {
            Stance.Id basestance = Stance.baseof((Stance.Id)stance.get());
            set_stance(basestance);
        }

        public void add_equip(int itemid)
        {
            equips.add_equip(itemid, drawinfo);
            updatetwohanded();
        }

        public void remove_equip(EquipSlot.Id slot)
        {
            equips.remove_equip(slot);

            if (slot == EquipSlot.Id.WEAPON)
            {
                updatetwohanded();
            }
        }

        public void attack(bool degenerate)
        {
            int weapon_id = equips.get_weapon();

            if (weapon_id <= 0)
            {
                return;
            }

            WeaponData weapon = WeaponData.get(weapon_id);

            byte attacktype = weapon.get_attack();

            if (attacktype == 9 && !degenerate)
            {
                stance.set((short)Stance.Id.SHOT);
                set_action("handgun");
            }
            else
            {
                stance.set((short)getattackstance(attacktype, degenerate));
                stframe.set(0);
                stelapsed = 0;
            }

            weapon.get_usesound(degenerate).play();
        }
        public void attack(Stance.Id newstance)
        {
            if (action != null || newstance == Stance.Id.NONE)
            {
                return;
            }

            switch (newstance)
            {
                case Stance.Id.SHOT:
                    set_action("handgun");
                    break;
                default:
                    set_stance(newstance);
                    break;
            }
        }

        public void set_stance(Stance.Id newstance)
        {
            if (action != null || newstance == Stance.Id.NONE)
            {
                return;
            }

            Stance.Id adjstance = equips.adjust_stance(newstance);

            if (stance != (short)adjstance)
            {
                stance.set((short)adjstance);
                stframe.set(0);
                stelapsed = 0;
            }
        }

        private Stance.Id getattackstance(byte attack, bool degenerate)
        {
            if (stance == (short)Stance.Id.PRONE)
            {
                return Stance.Id.PRONESTAB;
            }

            if (attack <= (int)Attack.NONE || attack >= (int)Attack.NUM_ATTACKS)
            {
                return Stance.Id.STAND1;
            }

            var stances = degenerate ? degen_stances[attack] : attack_stances[attack];

            if (stances.Count == 0)
            {
                return Stance.Id.STAND1;
            }

            int index = randomizer.next_int(stances.Count);

            return stances[(int)index];
        }

        private ushort get_delay(Stance.Id st, byte fr)
        {
            return drawinfo.get_delay(st, fr);
        }

        private byte getnextframe(Stance.Id st, byte fr)
        {
            return drawinfo.nextframe(st, fr);
        }

        public void set_expression(Expression.Id newexpression)
        {
            if (expression != (int)newexpression && !expcooldown)
            {
                expression.set((int)newexpression);
                expframe.set(0);

                expelapsed = 0;
                expcooldown.set_for(5000);
            }
        }

        public void set_action(string acstr)
        {
            if (acstr == actionstr || acstr == "")
            {
                return;
            }

            var ac_stance = Stance.by_string(acstr);
            if (ac_stance != 0)
            {
                set_stance(ac_stance);
            }
            else
            {
                action = drawinfo.get_action(acstr, 0);

                if (action != null)
                {
                    actframe = 0;
                    stelapsed = 0;
                    actionstr = acstr;

                    stance.set((short)action.get_stance());
                    stframe.set(action.get_frame());
                }
            }
        }

        public void set_direction(bool f)
        {
            flip = f;
        }

        public void set_alerted(long millis)
        {
            alerted.set_for(millis);
        }

        public bool get_alerted()
        {
            return (bool)alerted;
        }

        public bool is_twohanded(Stance.Id st)
        {
            switch (st)
            {
                case Stance.Id.STAND1:
                case Stance.Id.WALK1:
                    return false;
                case Stance.Id.STAND2:
                case Stance.Id.WALK2:
                    return true;
                default:
                    return equips.is_twohanded();
            }
        }

        public ushort get_attackdelay(uint no, byte first_frame)
        {
            if (action != null)
            {
                return drawinfo.get_attackdelay(actionstr, no);
            }

            ushort delay = 0;

            for (byte frame = 0; frame < first_frame; frame++)
            {
                delay += get_delay((Stance.Id)stance.get(), frame);
            }

            return delay;
        }
        public ushort get_total_attackdelay ()
        {
            if (action != null)
            {
                return drawinfo.get_total_attackdelay (actionstr);
            }

            return drawinfo.get_total_delay ((Stance.Id)stance.get ());
            
        }

        public ushort get_total_attackdelay (string actionstr)
        {
            return drawinfo.get_total_attackdelay (actionstr);
        }

        public byte get_frame()
        {
            return stframe.get();
        }

        public Stance.Id get_stance()
        {
            return (Stance.Id)stance.get();
        }
        public Stance.Id get_stance (float alpha)
        {
            return (Stance.Id)stance.get (alpha);
        }
        public Body get_body()
        {
            return body;
        }

        public Hair get_hair()
        {
            return hair;
        }

        public Face get_face()
        {
            return face;
        }

        public CharEquips get_equips()
        {
            return equips;
        }

        // Initialize drawinfo
        public static void init()
        {
            drawinfo.init();
        }



        private DrawArgument lastDraw_args;
        private Stance.Id lastDraw_interstance;
        private Expression.Id lastDraw_interexpression;
        private sbyte lastDraw_interframe = -1;
        private byte lastDraw_interexpframe;

        private void erase(DrawArgument args, Stance.Id interstance, Expression.Id interexpression, byte interframe, byte interexpframe)
        {
            if (args == null) return;
            //Point_short faceshift = drawinfo.getfacepos (interstance, interframe);
            Point_short faceshift = drawinfo.getfacepos(interstance, interframe) ?? Point_short.zero;
            DrawArgument faceargs = args + new DrawArgument(faceshift, false, new Point_short());

            if (Stance.is_climbing(interstance))
            {
                body.draw(interstance, Body.Layer.BODY, interframe, args.SetOrderInLayer(256), false);
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer(1), false);

                switch (equips.getcaptype())
                {
                    case CharEquips.CapType.NONE:
                        hair.draw(interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                    case CharEquips.CapType.HEADBAND:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1), false);
                        hair.draw(interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                    case CharEquips.CapType.HALFCOVER:
                        hair.draw(interstance, Hair.Layer.BELOWCAP, interframe, args.IncreaseOrderInLayer(1), false);
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                    case CharEquips.CapType.FULLCOVER:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                }

                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.BACKSHIELD, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.BACKWEAPON, interframe, args.IncreaseOrderInLayer(1), false);
            }
            else
            {
                hair.draw(interstance, Hair.Layer.BELOWBODY, interframe, args, false);
                equips.draw(EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_BELOW_BODY, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_BODY, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_BELOW_BODY, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.BODY, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_BODY, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_BODY, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.ARM_BELOW_HEAD, interframe, args.IncreaseOrderInLayer(1), false);

                if (equips.has_overall())
                {
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer(1), false);
                }
                else
                {
                    equips.draw(EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer(1), false);
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer(1), false);
                }

                body.draw(interstance, Body.Layer.ARM_BELOW_HEAD_OVER_MAIL, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer(1), false);
                hair.draw(interstance, Hair.Layer.SHADE, interframe, args.IncreaseOrderInLayer(1), false);
                hair.draw(interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer(1), false);
                face.draw(interexpression, interexpframe, faceargs.SetOrderInLayer(args.orderInLayer).IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.FACE, interstance, Clothing.Layer.FACEACC, 0, faceargs.SetOrderInLayer(args.orderInLayer).IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.EYEACC, interstance, Clothing.Layer.EYEACC, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD, interframe, args.IncreaseOrderInLayer(1), false);

                switch (equips.getcaptype())
                {
                    case CharEquips.CapType.NONE:
                        hair.draw(interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                    case CharEquips.CapType.HEADBAND:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1), false);
                        hair.draw(interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer(1), false);
                        hair.draw(interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer(1), false);
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                    case CharEquips.CapType.HALFCOVER:
                        hair.draw(interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer(1), false);
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                    case CharEquips.CapType.FULLCOVER:
                        equips.draw(EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer(1), false);
                        break;
                }

                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_ARM, interframe, args.IncreaseOrderInLayer(1), false);
                bool twohanded = is_twohanded(interstance);

                if (twohanded)
                {
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer(1), false);
                    body.draw(interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer(1), false);
                    equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer(1), false);
                }
                else
                {
                    equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer(1), false);
                    body.draw(interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer(1), false);
                    equips.draw(EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer(1), false);
                }

                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_GLOVE, interframe, args.IncreaseOrderInLayer(1), false);

                body.draw(interstance, Body.Layer.HAND_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer(1), false);

                body.draw(interstance, Body.Layer.ARM_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.ARM_OVER_HAIR_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_HAND, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_BODY, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.HAND_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1), false);
                body.draw(interstance, Body.Layer.HAND_OVER_WEAPON, interframe, args.IncreaseOrderInLayer(1), false);

                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1), false);
                equips.draw(EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_HAIR, interframe, args.IncreaseOrderInLayer(1), false);
            }
        }





        private List<Stance.Id>[] degen_stances =
        {
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.SWINGT1, Stance.Id.SWINGT3},
            new List<Stance.Id> {Stance.Id.SWINGT1, Stance.Id.STABT1},
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.SWINGT1, Stance.Id.STABT1},
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.SWINGP1, Stance.Id.STABT2}
        };

        private List<Stance.Id>[] attack_stances =
        {
            new List<Stance.Id> {Stance.Id.NONE},
            new List<Stance.Id> {Stance.Id.STABO1, Stance.Id.STABO2, Stance.Id.SWINGO1, Stance.Id.SWINGO2, Stance.Id.SWINGO3},//1
            new List<Stance.Id> {Stance.Id.STABT1, Stance.Id.SWINGP1},//2
            new List<Stance.Id> {Stance.Id.SHOOT1},//3
            new List<Stance.Id> {Stance.Id.SHOOT2},//4
            new List<Stance.Id> {Stance.Id.STABO1, Stance.Id.STABO2, Stance.Id.SWINGT1, Stance.Id.SWINGT2, Stance.Id.SWINGT3},//5
            new List<Stance.Id> {Stance.Id.SWINGO1, Stance.Id.SWINGO2},//6
            new List<Stance.Id> {Stance.Id.SWINGO1, Stance.Id.SWINGO2},//7
            new List<Stance.Id> {Stance.Id.NONE},//8
            new List<Stance.Id> {Stance.Id.SHOT}//9
        };



        private enum Attack
        {
            NONE = 0,
            S1A1M1D = 1,
            SPEAR = 2,
            BOW = 3,
            CROSSBOW = 4,
            S2A2M2 = 5,
            WAND = 6,
            CLAW = 7,
            GUN = 9,
            NUM_ATTACKS
        }

        private Nominal_short stance = new Nominal_short();
        private Nominal_byte stframe = new Nominal_byte();
        private ushort stelapsed;

        private Nominal_int expression = new Nominal_int();
        private Nominal_byte expframe = new Nominal_byte();
        private ushort expelapsed;
        private TimedBool expcooldown = new TimedBool();

        private bool flip;

        private BodyAction action;
        private string actionstr;
        private byte actframe;

        private Body body;
        private Hair hair;
        private Face face;
        private CharEquips equips = new CharEquips();

        private Randomizer randomizer = new Randomizer();
        private TimedBool alerted = new TimedBool();

        public static BodyDrawInfo get_BodyDrawInfo () => drawinfo;
        private static BodyDrawInfo drawinfo = new BodyDrawInfo();
        private static Dictionary<int, Hair> hairstyles = new Dictionary<int, Hair>();
        private static Dictionary<int, Face> facetypes = new Dictionary<int, Face>();
        private static Dictionary<int, Body> bodytypes = new Dictionary<int, Body>();
    }
}