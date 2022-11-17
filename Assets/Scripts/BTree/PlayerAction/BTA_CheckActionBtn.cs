using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_CheckActionBtn : ConditionTask
	{
        public PressTypes pressType = PressTypes.Down;
        public Enum_ActionButton key = Enum_ActionButton.HeavyAttack;

        protected override string info
        {
            get { return pressType.ToString () + " " + key.ToString (); }
        }

        protected override bool OnCheck ()
        {

            if (pressType == PressTypes.Down)
                return MyJoystickInput.GetButtonDown(key);

            if (pressType == PressTypes.Up)
                return MyJoystickInput.GetButtonUp (key);

            if (pressType == PressTypes.Pressed)
                return MyJoystickInput.GetButtonHold (key);

            return false;
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override void OnTaskInspectorGUI ()
        {

            UnityEditor.EditorGUILayout.BeginHorizontal ();
            pressType = (PressTypes)UnityEditor.EditorGUILayout.EnumPopup (pressType);
            key = (Enum_ActionButton)UnityEditor.EditorGUILayout.EnumPopup (key);
            UnityEditor.EditorGUILayout.EndHorizontal ();
        }

#endif
    }
}