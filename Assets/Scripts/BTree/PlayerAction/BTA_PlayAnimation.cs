using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using FairyGUI;

namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_PlayAnimation : PlayerAction_Base
	{
		public Enum_Animation pressType = Enum_Animation.ShowChargeHeavyAttack;
		private FGUI_Joystick _Joystick => ms_Unity.FGUI_Manager.Instance.GetFGUI<ms_Unity.FGUI_Joystick> ();
		protected override string info
		{
			get { return "²¥·Åwz¶¯»­: " + pressType.ToString (); }
		}
		private Animation animation;

		protected override void OnExecute ()
		{
			if (pressType == Enum_Animation.ShowChargeHeavyAttack)
			{
				animation = wz.wzFile_skill?.GetObjectFromPath ("Skill.wz/232.img/skill/2321001/keydown");
			}
		}

		protected override void OnUpdate ()
		{
			animation?.update ();
			animation?.draw (player?.absp ?? Point_short.zero, MapleStory.Instance?.alpha ?? 0);
		}

		///----------------------------------------------------------------------------------------------
		///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

		protected override void OnTaskInspectorGUI ()
		{

			UnityEditor.EditorGUILayout.BeginHorizontal ();
			pressType = (Enum_Animation)UnityEditor.EditorGUILayout.EnumPopup (pressType);
			UnityEditor.EditorGUILayout.EndHorizontal ();
		}

#endif
	}
}