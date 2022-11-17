using UnityEngine;

public enum STATE
{
    IDIE,
    RUN,
    FINISHED
}

public class MyTimer
{

    public STATE state;

    public float duration = 1.0f;

    private float elapsedTime = 0;//流逝的时间

    public void Tick()
    {
        switch (state)
        {
            case STATE.IDIE:

                break;
            case STATE.RUN:
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= duration)
                {
                    state = STATE.FINISHED;
                }
                break;
            case STATE.FINISHED:

                break;
            default:
                Debug.Log("Mytimer error");
                break;
        }
    }

    public void Go()
    {
        elapsedTime = 0;
        state = STATE.RUN;
    }
}
