using UnityEngine;
using ms_Unity;

public class MyJoystickInput : SingletonMono<MyJoystickInput>
{

	public MyButton btnLightAttack = new MyButton ();
	public MyButton btnHeavyAttack = new MyButton ();

	[Header ("--------LightAttack--------")]
	public bool is_BtnLightAttack_Down;
	public bool is_BtnLightAttack_Up;
	public bool is_BtnLightAttack_Hold;

	[Header ("--------HeavyAttack--------")]
	public bool is_BtnHeavyAttack_Down;
	public bool is_BtnHeavyAttack_Up;
	public bool is_BtnHeavyAttack_Hold;

	private static EnumMap<Enum_ActionButton, bool> actionBtn_IsDown = new EnumMap<Enum_ActionButton, bool> ();
	private static EnumMapNew<Enum_ActionButton, MyButton> actionBtn_MyButton = new EnumMapNew<Enum_ActionButton, MyButton> ();
	public static void SetButton (Enum_ActionButton _ActionButton, bool state)
	{
		actionBtn_IsDown.TryAdd (_ActionButton, state, true);
	}

	private static bool GetButton (Enum_ActionButton _ActionButton)
	{
		return actionBtn_IsDown.TryGetValue (_ActionButton);
	}
	public static bool GetButtonHold (Enum_ActionButton _ActionButton)
	{
		return actionBtn_MyButton[_ActionButton].IsPressing;
	}
	public static bool GetButtonDown (Enum_ActionButton _ActionButton)
	{
		return actionBtn_MyButton[_ActionButton].OnPressed;
	}
	public static bool GetButtonUp (Enum_ActionButton _ActionButton)
	{
		return actionBtn_MyButton[_ActionButton].OnReleased;
	}

	protected override void OnUpdate ()
	{
		foreach (var pair in actionBtn_MyButton)
		{
			pair.Value.Tick (GetButton(pair.Key));
		}
/*
		btnLightAttack.Tick (GetButton (Enum_ActionButton.LightAttack));
		btnHeavyAttack.Tick (GetButton (Enum_ActionButton.HeavyAttack));

		is_BtnLightAttack_Down = btnLightAttack.OnPressed;
		is_BtnLightAttack_Up = btnLightAttack.OnReleased;
		is_BtnLightAttack_Hold = btnLightAttack.IsPressing;

		is_BtnHeavyAttack_Down = btnHeavyAttack.OnPressed;
		is_BtnHeavyAttack_Up = btnHeavyAttack.OnReleased;
		is_BtnHeavyAttack_Hold = btnHeavyAttack.IsPressing;*/
	}


}
