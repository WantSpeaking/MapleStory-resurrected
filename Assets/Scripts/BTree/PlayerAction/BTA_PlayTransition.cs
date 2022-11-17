using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using FairyGUI;

namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_PlayTransition : PlayerAction_Base
	{
        public Enum_Transition pressType = Enum_Transition.t_ShowBtnSkill;
        private FGUI_Joystick _Joystick => ms_Unity.FGUI_Manager.Instance.GetFGUI<ms_Unity.FGUI_Joystick> ();
        protected override string info
        {
            get { return "²¥·Å¶¯Ð§: " + pressType.ToString (); }
        }

        protected override void OnExecute ()
        {
            if (pressType == Enum_Transition.t_ShowBtnSkill || pressType == Enum_Transition.t_HideBtnSkill)
			{
                _Joystick.GetTransition (pressType.ToString ())?.Play ();
                //_Joystick._t_ShowBtnSkill.Play ();
            }

            EndAction (true);
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override void OnTaskInspectorGUI ()
        {

            UnityEditor.EditorGUILayout.BeginHorizontal ();
            pressType = (Enum_Transition)UnityEditor.EditorGUILayout.EnumPopup (pressType);
            UnityEditor.EditorGUILayout.EndHorizontal ();
        }

#endif
    }
}