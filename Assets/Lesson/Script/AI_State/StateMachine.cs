using UnityEngine;

public interface IState
{
    void Enter();

    void Update(GameObject obj);

    void Exit();
}

public partial class StateMachine 
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public void Update(GameObject obj)
    {
        if(currentState != null)
        {
            currentState.Update(obj);
        }
    }
}
