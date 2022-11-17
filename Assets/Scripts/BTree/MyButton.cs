public class MyButton
{
    public bool IsPressing = false;//hold
    public bool OnPressed = false;//down
    public bool OnReleased = false;//up
    public bool IsExtending = false;//extend after release
    public bool IsDelaying = false;//Delay after down

    public float extendingDurtion = 0.15f;
    public float delayingDurtion = 0.15f;

    private bool curState = false;
    private bool lastState = false;

    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();

    public void Tick(bool input)
    {
        //StartTimer(extTimer, 1.0f);
        extTimer.Tick();
        delayTimer.Tick();

        curState = input;

        IsPressing = curState;

        OnPressed = false;
        OnReleased = false;
        IsExtending = false;
        IsDelaying = false;
        if (curState != lastState)
        {
            if (curState == true)
            {
                OnPressed = true;
                StartTimer(delayTimer, delayingDurtion);
            }
            else
            {
                OnReleased = true;
                StartTimer(extTimer, extendingDurtion);
            }
        }

        lastState = curState;

        IsExtending = extTimer.state == STATE.RUN;
        IsDelaying = delayTimer.state == STATE.RUN;
    }

    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }
}
