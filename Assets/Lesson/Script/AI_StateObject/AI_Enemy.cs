using UnityEngine;

public class AI_Enemy : MonoBehaviour
{
    public AI_Player player;
    public AI_State ai_state;
    private int state;
    public float walkCondition;
    public float AttackCondition;

    enum StateEnum
    {
        Idle,
        Walking,
        Attack
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        if( distance < walkCondition && state == (int)StateEnum.Idle)
        {
            ai_state.stateMachine.ChangeState(new StateMachine.EnemyWalkingState(ai_state.stateMachine));
            state = (int)StateEnum.Walking;
        }

        else if (distance >= walkCondition && state == (int)StateEnum.Walking)
        {
            ai_state.stateMachine.ChangeState(new StateMachine.EnemyIdleState(ai_state.stateMachine));
            state = (int)StateEnum.Idle;
        }

        else if(distance < AttackCondition && state == (int)StateEnum.Walking)
        {
            ai_state.stateMachine.ChangeState(new StateMachine.EnemyAttackState(ai_state.stateMachine));
            state = (int)StateEnum.Attack;
        }

        else if (distance >= AttackCondition && state == (int)StateEnum.Attack)
        {
            ai_state.stateMachine.ChangeState(new StateMachine.EnemyWalkingState(ai_state.stateMachine));
            state = (int)StateEnum.Walking;
        }
    }
}
