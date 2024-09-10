using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : LockedInteraction
{
    Player player;
    protected void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    public override void UpdateUnitFromVariable(ref DataUnit du)
    {
        du.Bool["Enabled"] = gameObject.activeSelf;
    }

    public override void UpdateVariableFromUnit(DataUnit du)
    {
        bool enable;
        if (!du.Bool.TryGetValue("Enabled", out enable))
        {
            enable = true;
        }
        else
        {
            player.hasKey = true;
        }
        gameObject.SetActive(enable);
    }
    protected override void OnUnlockedInteraction()
    {
        base.OnUnlockedInteraction();
        player.hasKey = true;
        gameObject.SetActive(false);    
    }
}
