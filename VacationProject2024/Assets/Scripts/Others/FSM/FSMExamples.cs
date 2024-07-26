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
    }
}
class DefaultState : State<FSMExamples>
{
    public DefaultState(FSMExamples origin, Layer<FSMExamples> parent) : base(origin, parent) 
    { 

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.E)) parentLayer.ChangeState("SubStateMachine");
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
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); //하위 스테이트 업데이트
        if (Input.GetKeyDown(KeyCode.E)) parentLayer.ChangeState("Default");
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
