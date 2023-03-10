namespace ms.SkillTrees
{

    //An interface for states. Works together with IStateCallbackReceiver
    public interface IState
    {
        ///<summary>The name of the state</summary>
        string name { get; }
        ///<summary>The tag of the state</summary>
        string tag { get; }
        ///<summary>The elapsed time of the state</summary>
        float elapsedTime { get; }
        ///<summary>The FSM this state belongs to</summary>
        SkillTree FSM { get; }
        ///<summary>An array of the state's transition connections</summary>
        STConnection[] GetTransitions();
        ///<summary>Evaluates the state's transitions and returns true if a transition has been performed</summary>
        bool CheckTransitions();
        ///<summary>Marks the state as Finished</summary>
        void Finish(bool success);
    }
}