using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionAgent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnInteraction()
    {
        base.OnInteraction();

        Debug.Log("DoorInteracted");
        Invoke("ReEnableInteraction", 5);
    }

    void ReEnableInteraction()
    {
        AllowInteraction = true;
        Debug.Log("Enabled");
    }
}
