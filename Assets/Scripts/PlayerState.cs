using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState         //모든 플레이어 상태의 기본이 되는 추상 클래스
{
    protected PlayerStateMachine stateMachine;          //상태 머신에 대한 참조 
    protected PlayerController playerController;        //플레이어 컨트롤러에 대한 참조

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.playerController;
    }

    //가상 메서드 들 : 하위 클래스에서 필요에 따라 오버라이드
    public virtual void Enter() { }  //상태 진입 시 호출
    public virtual void Exit() { }   //상태 종료 시 호출
    public virtual void Update() { } //매 플레임 호출
    public virtual void FixedUpdate() { }  //고정 시간 간격으로 호출 (물리 연상용)

    //상태 전환 조건을 체크하는 메서드
    protected void CheckTransitions()
    {
        if (playerController.isGrounded())   //지상에 있을 의 전환 로직
        {
            if (Input.GetKeyDown(KeyCode.Space))    //점프 키를 눌렀을 때 
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)    //이동 키가 눌렸을 때
            {
                stateMachine.TransitionToState(new MoveingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new IdleState(stateMachine));        //아무 키도 누르지 않았을 때
            }
        }

        //공중에 있을떄의 상태 전환 로직
        else
        {
            if(playerController.GetVerticalVelocity() > 0)
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new FallingState(stateMachine));
            }
        }
    }
    //IdleState : 플레이어가 정지해 있는 상태
    public class IdleState : PlayerState
    {
        public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Update()
        {
            CheckTransitions();
        }
    }

    //MoveingState :플레이어가 이동 중인 상태
    public class MoveingState : PlayerState
    {
        public MoveingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Update()
        {
            CheckTransitions();
        }
        public override void FixedUpdate()
        {
            playerController.HandleMovement();   //물리 기반 이동 처리
        }
    }
    //JumpingState : 플레이어가 점프 중일 때
    public class JumpingState : PlayerState
    {
        public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Update()
        {
            CheckTransitions();                 //매 프레임 마다 상태 전환 조건 체크 
        }

        public override void FixedUpdate()
        {
            playerController.HandleMovement();      //물리 기반 이동 처리 
        }
    }

    //FellingState : vmffpdldjrk EJfdjwlf EO
    public class FallingState : PlayerState
    {
        public FallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
        public override void Update()
        {
            CheckTransitions();                 //매 프레임 마다 상태 전환 조건 체크 
        }
        public override void FixedUpdate()
        {
            playerController.HandleMovement();      //물리 기반 이동 처리 
        }
    }
}
