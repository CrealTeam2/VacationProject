using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMExamples : MonoBehaviour
{
    TopLayer<FSMExamples> topLayer;
    [SerializeField] string FSMPath;
    private void Awake()
    {
        topLayer = new TopLayer(this);
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); }; //FSM 경로 표시 업데이트
        topLayer.OnStateEnter(); //기본 State 세팅을 해주기 때문에 생성과 동시에 발동 필요
    }
    private void Update()
    {
        topLayer.OnStateUpdate();
    }
}
class TopLayer : TopLayer<FSMExamples>
{
    public TopLayer(FSMExamples origin) : base(origin)
    {
        defaultState = new DefaultState(origin, this);
        AddState("Default", defaultState);
        AddState("SubStateMachine", new SubStateMachine(origin, this));
        AddState("NoDefaultState", new SubStateMachineThatDoesNotUseDefaultState(origin, this));
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentState != states["Default"])
        {
            ChangeState("Default");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && currentState != states["SubStateMachine"])
        {
            ChangeState("SubStateMachine");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentState != states["NoDefaultState"])
        {
            ChangeState("NoDefaultState");
        }
    }
}
class DefaultState : State<FSMExamples>
{
    public DefaultState(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent) 
    { 

    }
}
class SubStateMachine : Layer<FSMExamples>
{
    public SubStateMachine(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent)
    {
        defaultState = new SubStateMachine_DefaultState(origin, this);
        AddState("Default", defaultState);
        AddState("Second", new SubStateMachine_SecondState(origin, this));
    }
}
class SubStateMachine_DefaultState : State<FSMExamples>
{
    public SubStateMachine_DefaultState(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent)
    {

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.F)) parentLayer.ChangeState("Second");
    }
}
class SubStateMachine_SecondState : State<FSMExamples>
{
    public SubStateMachine_SecondState(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent)
    {

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.F)) parentLayer.ChangeState("Default");
    }
}
class SubStateMachineThatDoesNotUseDefaultState : Layer<FSMExamples>
{
    public SubStateMachineThatDoesNotUseDefaultState(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent)
    {
        AddState("State1", new State1(origin, this));
        AddState("State2", new State2(origin, this));
    }
    public override void OnStateEnter()
    {
        //base.OnStateEnter(); defaultState를 사용할 거면 주석 해제
        if (Input.GetKey(KeyCode.R)) //R을 누른 채로 스테이트 진입 시 State1, 아니면 State2
        {
            currentState = states["State1"];
        }
        else
        {
            currentState = states["State2"];
        }
        currentState.OnStateEnter();
    }
}
class State1 : State<FSMExamples>
{
    public State1(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent)
    {

    }
}
class State2 : State<FSMExamples>
{
    public State2(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent)
    {

    }
}
