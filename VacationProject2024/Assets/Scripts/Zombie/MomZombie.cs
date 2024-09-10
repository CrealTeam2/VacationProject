using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MomZombie : Zombie
{
    [SerializeField] float maxHealth;
    protected override void Start() { }
    protected override TopLayer<Zombie> InitTop() => new Mom_TopLayer(this);
}
class Mom_TopLayer : TopLayer<Zombie>
{
    public Mom_TopLayer(MomZombie origin) : base(origin)
    {
        defaultState = new Mom_Alive(origin, this);
        AddState("Dead", new Mom_Dead(origin, this));
    }
}
class Mom_Alive : State<Zombie>
{
    public Mom_Alive(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }
}
class Mom_Dead : State<Zombie>
{
    public Mom_Dead(Zombie origin, Layer<Zombie> parent) : base(origin, parent)
    {

    }
}