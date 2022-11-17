using UnityEngine;

public class JoystickInput : IUserInput
{
	[Header ("=======Joystick Settings=======")]
	public string axisX = "axisX";
	public string axisY = "axisY";
	public string axisJright = "axis3";
	public string axisJup = "axis6";
	public string btnA = "btn0";
	public string btnB = "btn1";
	public string btnC = "btn2";
	public string btnD = "btn3";
	public string btnLB = "btn4";
	public string btnRB = "btn5";
	public string btnLT = "btn6";
	public string btnRT = "btn7";
	public string btnRightJoystick = "btn11";

	#region Xbox
	//string axisX = "axisX";
	//string axisY = "axisY";
	//string axisJright = "axis4";
	//string axisJup = "axis5";
	//string axisDpadX = "axis6";
	//string axisDpadY = "axis7";
	//string axisLT = "axis3";
	//string axisRT = "axis3";
	//string btnA = "btn0";
	//string btnB = "btn1";
	//string btnC = "btn2";
	//string btnD = "btn3";
	//string btnLB = "btn4";
	//string btnRB = "btn5";
	//string btnReset = "btn6";
	//string btnMenu = "btn7";
	//string btnLeftJoystick = "btn8";
	//string btnRightJoystick = "btn9";
	#endregion

	public MyButton buttonA = new MyButton ();
	public MyButton buttonB = new MyButton ();
	public MyButton buttonC = new MyButton ();
	public MyButton buttonD = new MyButton ();
	public MyButton buttonLB = new MyButton ();
	public MyButton buttonRB = new MyButton ();
	public MyButton buttonLT = new MyButton ();
	public MyButton buttonRT = new MyButton ();
	public MyButton buttonJstick = new MyButton ();

	//[Header("=======Output signal=======")]
	//public float Dup;
	//public float Dright;
	//public float Dmag;//Drmagnitude
	//public Vector3 DVec;//DrVector
	//public float Jup;
	//public float Jright;

	////1.pressing signal
	//public bool run;
	////2.trigger signal
	//public bool jump;
	//private bool lastJump;
	//public bool attack;
	//private bool lastAttack;

	//[Header("=======Others=======")]
	//public bool InputEnabled = true;

	//private float targetDup;
	//private float targetDright;
	//private float velocityDup;
	//private float velocityDright;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		buttonA.Tick (Input.GetButton (btnA));
		buttonB.Tick (Input.GetButton (btnB));
		buttonC.Tick (Input.GetButton (btnC));
		buttonD.Tick (Input.GetButton (btnD));
		buttonLB.Tick (Input.GetButton (btnLB));
		buttonRB.Tick (Input.GetButton (btnRB));
		buttonLT.Tick (Input.GetButton (btnLT));
		buttonRT.Tick (Input.GetButton (btnRT));
		buttonJstick.Tick (Input.GetButton (btnRightJoystick));

		//print(buttonJstick.OnReleased);
		//print(buttonA.IsExtending && buttonA.OnPressed);

		Jup = Input.GetAxis (axisJup);
		Jright = Input.GetAxis (axisJright);

		targetDup = Input.GetAxis (axisY);
		targetDright = Input.GetAxis (axisX);

		if (InputEnabled == false)
		{
			targetDup = targetDright = 0;
		}

		Dup = Mathf.SmoothDamp (Dup, targetDup, ref velocityDup, .1f);
		Dright = Mathf.SmoothDamp (Dright, targetDright, ref velocityDright, .1f);

		//Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
		//float Dright2 = tempDAxis.x;
		//float Dup2 = tempDAxis.y;

		//Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
		//DVec = Dright2 * transform.right + Dup2 * transform.forward;
		Dmag = Mathf.Sqrt (Dup * Dup + Dright * Dright);
		DVec = Dright * transform.right + Dup * transform.forward;
		Dmag = Mathf.Clamp (Dmag, 0, 1);

		run = buttonA.IsPressing && !buttonA.IsDelaying || buttonA.IsExtending;
		jump = buttonA.OnPressed && buttonA.IsExtending;
		roll = buttonA.OnReleased && buttonA.IsDelaying;
		action = buttonC.OnPressed;

		defense = buttonLB.IsPressing;
		//attack = buttonC.OnPressed;
		rb = buttonRB.OnPressed;
		rt = buttonRT.OnPressed;
		lb = buttonLB.OnPressed;
		lt = buttonLT.OnPressed;
		lockOn = buttonJstick.OnPressed;
	}

}
