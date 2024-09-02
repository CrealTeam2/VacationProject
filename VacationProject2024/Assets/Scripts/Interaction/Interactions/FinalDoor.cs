using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : InteractionAgent
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
    public override void OnInteraction()
    {
        base.OnInteraction();

        if (true)
        {
            anim.Play("Outer_Door_Open");
        }

/*        if (isOpened)
        {
            anim.Play("Door2_Close");
            isOpened = false;
            feedbackText = "문 열기";
        }
        else
        {
            anim.Play("Door2_Open");
            isOpened = true;
            feedbackText = "문 닫기";
        }
        Invoke("ReEnableInteraction", 0.2f);*/
    }
}
