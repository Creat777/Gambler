using UnityEngine;

public partial class StateMachine
{
    // EnemyIdleState는 StateMachine 클래스 내에서만 접근 가능한 클래스
    public class EnemyIdleState : IState
    {
        private StateMachine stateMachine;

        public EnemyIdleState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            Debug.Log("Entered Idle State");
        }

        public void Update(GameObject obj)
        {
            // 일정 거리 안에 Player가 오면 상태 전이
            if(Input.GetKeyDown(KeyCode.W))
            {
                stateMachine.ChangeState(new EnemyWalkingState(stateMachine));
            }
        }

        public void Exit()
        {
            Debug.Log("Exit Idle State");
        }
        
    }
}



