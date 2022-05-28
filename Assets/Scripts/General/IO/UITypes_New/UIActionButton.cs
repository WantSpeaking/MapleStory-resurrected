using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using MapleLib.WzLib;
using Microsoft.Xna.Framework.Input;

namespace ms
{
    public class UIActionButton : UIElement
    {
        public const Type TYPE = UIElement.Type.ActionButton;
        public const bool FOCUSED = false;
        public const bool TOGGLED = false;

        enum Buttons
        {
            Jump,
            Attack,
            Pickup,
            Action1,
            Action2,
            Action3,
            Action4,
            Action5,
        }
        public UIActionButton()
        {

            var img_game = ms.wz.wzFile_UI_Endless["Game.img"];
            var wzobj_btnJump = img_game["Btn_Jump"];

            Texture tex_bgAction = img_game["BG_Action"];

            var KeyConfig = ms.wz.wzFile_ui["StatusBar3.img"]["KeyConfig"];


            var icon = (KeyConfig["icon"]);

            Texture pickup = icon[50.ToString()];
            Texture attack = icon[52.ToString()];
            Texture jump = icon[53.ToString()];

            WzObject Login = ms.wz.wzFile_ui["Login.img"];
            WzObject Common = Login["Common"];
            WzObject CharSelect = Login["CharSelect"];

            /*var offset_baseActionButton = new Point_short((short)(Constants.get().get_viewwidth() * 3 / 4), (short)(Constants.get().get_viewheight() * 0.5f));

          var baseBtnDimension = 50;
           buttons[(int)Buttons.Attack] = new MapleButton(attack, attack, attack, attack, new Point_short((short)(baseBtnDimension), (short)(baseBtnDimension * 2)));
           buttons[(int)Buttons.Jump] = new MapleButton(jump, jump, jump, jump, new Point_short((short)(baseBtnDimension), (short)(baseBtnDimension)));
           //buttons[(int)Buttons.Jump] = new MapleButton(wzobj_btnJump);
           buttons[(int)Buttons.Pickup] = new MapleButton(pickup, pickup, pickup, pickup, new Point_short((short)(0), (short)(baseBtnDimension * 2)));*/

            var offset_ActionButton = new Point_short((short)(Constants.get().get_viewwidth() - 55), (short)(Constants.get().get_viewheight() - 55));
            //actionButtons = new List<ActionButton>();

            var btnDimension = 90;

            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    var totalIndex = i * 3 + j;
                    ms.KeyConfig.Key configKey = (ms.KeyConfig.Key)(2 + totalIndex);
                    var btn_Action = new ActionButton(configKey, new Point_short((short)(offset_ActionButton.x() - i * btnDimension), (short)(offset_ActionButton.y() - j * btnDimension)), 1, 0.5f, tex_BG: tex_bgAction, offset_FG: new Point_short(-16, -16));

                    if (0 <= totalIndex && totalIndex <= 2)
                    {
                        if (totalIndex == 0)
                        {
                            configKey = ms.KeyConfig.Key.Z;
                        }
                        else if (totalIndex == 1)
                        {
                            configKey = ms.KeyConfig.Key.LEFT_ALT;
                        }
                        else if (totalIndex == 2)
                        {
                            configKey = ms.KeyConfig.Key.LEFT_CONTROL;
                        }
                        btn_Action.configKey = configKey;
                        btn_Action.is_SendIcon = false;
                    }

                    btn_Action.is_PlaySound = false;
                    btn_Action.OnDown += Action_OnDown;
                    btn_Action.OnUp += Action_OnUp;
                    btn_Action.OnClick += Action_OnClick;
                    btn_Action.OnPressing += Action_OnPressing;
                    btn_Action.OnRollIn += Action_OnRollIn;
                    btn_Action.OnRollOver += Action_OnRollOver;
                    btn_Action.OnRollOut += Action_OnRollOut;
                    buttons.Add((uint)totalIndex, btn_Action);

                }
            }
        }

        private void Action_OnDown(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                UI.get().send_key(actionButton.mapleKey, true, true);
                //AppDebug.Log($"OnDown:{actionButton.configKey}");
            }
        }

        private void Action_OnUp(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                UI.get().send_key(actionButton.mapleKey, false, true);
                //AppDebug.Log($"OnUp:{actionButton.configKey}");
            }
        }
        private void Action_OnClick(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                //AppDebug.Log($"OnClick:{actionButton.configKey}");
            }
        }
        private void Action_OnPressing(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                //AppDebug.Log($"OnPressing:{actionButton.configKey}");
            }
        }
        private void Action_OnRollIn(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                //AppDebug.Log($"OnRollIn:{actionButton.configKey}");
            }
        }
        private void Action_OnRollOver(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                //AppDebug.Log($"OnRollOver:{actionButton.configKey}");
            }
        }
        private void Action_OnRollOut(object sender, object e)
        {
            if (sender is ActionButton actionButton)
            {
                //AppDebug.Log($"OnRollOut:{actionButton.configKey}");
            }
        }
        public override Type get_type()
        {
            return TYPE;
        }

       
        public override bool is_in_range(Point_short cursorpos)
        {
            foreach (var btit in buttons)
            {
                if (btit.Value.is_active() && btit.Value.bounds(position).expand(5).contains(cursorpos))
                {
                    return true;
                }
            }
            return false;
        }

        public override void update()
        {
            var ref_UIKeyConfig = UI.get().get_element<UIKeyConfig>();
            if (ref_UIKeyConfig)
            {
                foreach (var pair in buttons)
                {
                    if (pair.Value is ActionButton actionButton)
                    {
                        actionButton.Icon_FG = ref_UIKeyConfig.get().GetStagedIcon((int)actionButton.configKey);
                    }
                }
            }

            base.update();
        }

        public override bool send_icon(Icon icon, Point_short cursorpos)
        {
            foreach (var btn in buttons)
            {
                if (btn.Value is ActionButton actionBtn && actionBtn.is_active() && actionBtn.bounds(position).contains(cursorpos) && actionBtn.is_SendIcon)
                {
                    var keyconfig = UI.get().get_element<UIKeyConfig>();
                    if (keyconfig)
                    {
                        var tempResult = keyconfig.get().send_icon(icon, keyconfig.get().GetAbsKeyPos(actionBtn.configKey));
                        keyconfig.get().SaveAndClose();
                        return tempResult;
                    }
                }
            }

            return false;
        }

        public class ActionButton : MapleButton
        {
            public int mapleKey => (int)configKey;
            public KeyConfig.Key configKey;
            public bool is_SendIcon = true;

            public ActionButton(KeyConfig.Key configKey, Point_short pos, int downEffect, float downEffectValue, Texture tex_BG = null, Texture tex_FG = null, Icon icon_FG = null, Point_short offset_FG = null) :
                base(pos, downEffect, downEffectValue, tex_BG, tex_FG, icon_FG, offset_FG)
            {
                this.configKey = configKey;
            }
        }
    }
}
