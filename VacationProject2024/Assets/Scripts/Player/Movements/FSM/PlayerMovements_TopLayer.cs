using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_TopLayer : TopLayer<Player>
{
    public PlayerMovements_TopLayer(Player origin) : base(origin)
    {
        defaultState = new PlayerMovements_Idle(origin, this);
        AddState("Idle", defaultState);
        AddState("Moving", new PlayerMovements_Moving(origin, this));
    }
}
