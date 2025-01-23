using UnityEngine;

public class AI_State : MonoBehaviour
{
    public StateMachine stateMachine;
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new StateMachine.EnemyIdleState(stateMachine));
    }

    void Update()
    {
        stateMachine.Update(gameObject);
    }
}
