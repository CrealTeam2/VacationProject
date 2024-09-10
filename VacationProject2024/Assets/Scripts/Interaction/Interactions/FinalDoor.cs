using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : LockedInteraction
{
    Animation anim;
    
    protected void Start()
    {
        base.Start();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void OnUnlockedInteraction()
    {
        base.OnUnlockedInteraction();
        anim.Play("Outer_Door_Open");
        StartCoroutine(GameManager.Instance.GameWin());
    }
}
