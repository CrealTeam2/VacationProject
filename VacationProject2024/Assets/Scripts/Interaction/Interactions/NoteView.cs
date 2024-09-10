using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteView : InteractionAgent
{
    Player player;
    
    protected void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateUnitFromVariable(ref DataUnit du)
    {
        du.Bool["Enabled"] = gameObject.activeSelf;
    }

    public override void UpdateVariableFromUnit(DataUnit du)
    {
        bool enable;
        if (!du.Bool.TryGetValue("Enabled", out enable)) enable = true; 
        gameObject.SetActive(enable);
    }
    public override void OnInteraction()
    {
        base.OnInteraction();

        player.bandages++;
        gameObject.SetActive(false);
    }
}
