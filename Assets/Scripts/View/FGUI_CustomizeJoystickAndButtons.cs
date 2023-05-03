using FairyGUI;
using FairyGUI.Utils;
using ms;
using System;
using static ms.UIActionButton;

namespace ms_Unity
{
    public partial class FGUI_CustomizeJoystickAndButtons
    {
        

        public void OnCreate()
        {
            _ActionButtons._Btn_HeavyAttack.draggable = true;
            _ActionButtons._Btn_LightAttack.draggable = true;
            _ActionButtons._Btn_Jump        .draggable = true;
            _ActionButtons._Btn_PickUp      .draggable = true;
            _ActionButtons._Btn_Skill1.draggable = true;
            _ActionButtons._Btn_Skill2.draggable = true;
            _ActionButtons._Btn_Skill3.draggable = true;
            _ActionButtons._Btn_Skill4.draggable = true;
            _ActionButtons._Btn_Skill5.draggable = true;
            _ActionButtons._Btn_Skill6.draggable = true;

            _Btn_OK.onClick.Add(OnClick_OK);
            _Btn_Cancel.onClick.Add(OnClick_Cancel);
            _Btn_Reset.onClick.Add(OnClick_Reset);
        }


        private void OnClick_OK(EventContext context)
        {
            ms.Setting<ms.PosBtnHeavyAttack>.get().save(new Point_short((short)_ActionButtons._Btn_HeavyAttack.x, (short)_ActionButtons._Btn_HeavyAttack.y));
            ms.Setting<ms.PosBtnLightAttack>.get().save(new Point_short((short)_ActionButtons._Btn_LightAttack.x, (short)_ActionButtons._Btn_LightAttack.y));
            ms.Setting<ms.PosBtnJump  >.get().save(new Point_short((short)_ActionButtons._Btn_Jump       .x, (short)_ActionButtons._Btn_Jump.y));
            ms.Setting<ms.PosBtnPickUp>.get().save(new Point_short((short)_ActionButtons._Btn_PickUp.x, (short)_ActionButtons._Btn_PickUp.y));
            ms.Setting<ms.PosBtnSkill1>.get().save(new Point_short((short)_ActionButtons._Btn_Skill1.x, (short)_ActionButtons._Btn_Skill1.y));
            ms.Setting<ms.PosBtnSkill2>.get().save(new Point_short((short)_ActionButtons._Btn_Skill2.x, (short)_ActionButtons._Btn_Skill2.y));
            ms.Setting<ms.PosBtnSkill3>.get().save(new Point_short((short)_ActionButtons._Btn_Skill3.x, (short)_ActionButtons._Btn_Skill3.y));
            ms.Setting<ms.PosBtnSkill4>.get().save(new Point_short((short)_ActionButtons._Btn_Skill4.x, (short)_ActionButtons._Btn_Skill4.y));
            ms.Setting<ms.PosBtnSkill5>.get().save(new Point_short((short)_ActionButtons._Btn_Skill5.x, (short)_ActionButtons._Btn_Skill5.y));
            ms.Setting<ms.PosBtnSkill6>.get().save(new Point_short((short)_ActionButtons._Btn_Skill6.x, (short)_ActionButtons._Btn_Skill6.y));

            FGUI_Manager.Instance.GetFGUI<FGUI_ActionButtons>().RefreshPos();
            Close();
        }

        private void OnClick_Cancel(EventContext context)
        {
            Close();

        }

        private void OnClick_Reset(EventContext context)
        {
            _ActionButtons._Btn_HeavyAttack.SetXY(new Point_short(Configuration.DefaultPos_Btn_HeavyAttack).x(), new Point_short(Configuration.DefaultPos_Btn_HeavyAttack).y());
            _ActionButtons._Btn_LightAttack.SetXY(new Point_short(Configuration.DefaultPos_Btn_LightAttack).x(), new Point_short(Configuration.DefaultPos_Btn_LightAttack).y());
            _ActionButtons._Btn_Jump.SetXY(new Point_short(Configuration.DefaultPos_Btn_Jump).x(), new Point_short(Configuration.DefaultPos_Btn_Jump).y());
            _ActionButtons._Btn_PickUp.SetXY(new Point_short(Configuration.DefaultPos_Btn_PickUp).x(), new Point_short(Configuration.DefaultPos_Btn_PickUp).y());
            _ActionButtons._Btn_Skill1.SetXY(new Point_short(Configuration.DefaultPos_Btn_Skill1).x(), new Point_short(Configuration.DefaultPos_Btn_Skill1).y());
            _ActionButtons._Btn_Skill2.SetXY(new Point_short(Configuration.DefaultPos_Btn_Skill2).x(), new Point_short(Configuration.DefaultPos_Btn_Skill2).y());
            _ActionButtons._Btn_Skill3.SetXY(new Point_short(Configuration.DefaultPos_Btn_Skill3).x(), new Point_short(Configuration.DefaultPos_Btn_Skill3).y());
            _ActionButtons._Btn_Skill4.SetXY(new Point_short(Configuration.DefaultPos_Btn_Skill4).x(), new Point_short(Configuration.DefaultPos_Btn_Skill4).y());
            _ActionButtons._Btn_Skill5.SetXY(new Point_short(Configuration.DefaultPos_Btn_Skill5).x(), new Point_short(Configuration.DefaultPos_Btn_Skill5).y());
            _ActionButtons._Btn_Skill6.SetXY(new Point_short(Configuration.DefaultPos_Btn_Skill6).x(), new Point_short(Configuration.DefaultPos_Btn_Skill6).y());

           /* ms.Setting<ms.PosBtnHeavyAttack>.get().save(new Point_short(Configuration.DefaultPos_Btn_HeavyAttack));
            ms.Setting<ms.PosBtnLightAttack>.get().save(new Point_short(Configuration.DefaultPos_Btn_LightAttack));
            ms.Setting<ms.PosBtnJump>.get().save(new Point_short(Configuration.DefaultPos_Btn_Jump));
            ms.Setting<ms.PosBtnPickUp>.get().save(new Point_short(Configuration.DefaultPos_Btn_PickUp));
            ms.Setting<ms.PosBtnSkill1>.get().save(new Point_short(Configuration.DefaultPos_Btn_Skill1));
            ms.Setting<ms.PosBtnSkill2>.get().save(new Point_short(Configuration.DefaultPos_Btn_Skill2));
            ms.Setting<ms.PosBtnSkill3>.get().save(new Point_short(Configuration.DefaultPos_Btn_Skill3));
            ms.Setting<ms.PosBtnSkill4>.get().save(new Point_short(Configuration.DefaultPos_Btn_Skill4));
            ms.Setting<ms.PosBtnSkill5>.get().save(new Point_short(Configuration.DefaultPos_Btn_Skill5));
            ms.Setting<ms.PosBtnSkill6>.get().save(new Point_short(Configuration.DefaultPos_Btn_Skill6));*/

            //FGUI_Manager.Instance.GetFGUI<FGUI_ActionButtons>().RefreshPos();
            //Close();
        }

        private void Close()
        {
            FGUI_Manager.Instance.CloseFGUI<FGUI_CustomizeJoystickAndButtons>();
        }

        //todo 打开的时候 设置成 和 原按键一样的位置
    }
}