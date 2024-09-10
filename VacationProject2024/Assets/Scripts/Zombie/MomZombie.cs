using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MomZombie : Zombie
{
    [SerializeField] LockedInteraction[] unlocks;
    protected override void Start() { }
    protected override TopLayer<Zombie> InitTop() => new Mom_TopLayer(this);
    public override void Die()
    {
        base.Die();
        foreach (var i in unlocks) i.Unlock();
    }
}
class Mom_TopLayer : TopLayer<Zombie>
{
    public Mom_TopLayer(MomZombie origin) : base(origin)
    {
        defaultState = new Mom_Alive(origin, this);
        AddState("Alive", defaultState);
        AddState("Dead", new Mom_Dead(origin, this));
    }
}
class Mom_Alive : State<Zombie>
{
    public Mom_Alive(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SoundManager.Instance.PlaySound(GameObject.Find("Piano").transform.Find("Piano_Cap").gameObject, "Piano1", 0.6f, 9999);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        SoundManager.Instance.StopSound(GameObject.Find("Piano").transform.Find("Piano_Cap").gameObject, "Piano1");
    }
}
class Mom_Dead : State<Zombie>
{
    public Mom_Dead(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SoundManager.Instance.PlaySound(origin.gameObject, "ZombieScream", 1, 1);
    }
}