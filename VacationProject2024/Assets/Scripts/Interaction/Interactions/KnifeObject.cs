using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeObject : InteractionAgent
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
    public override void OnInteraction()
    {
        base.OnInteraction();

        player.UnlockKnife();
        gameObject.SetActive(false);
    }
}
