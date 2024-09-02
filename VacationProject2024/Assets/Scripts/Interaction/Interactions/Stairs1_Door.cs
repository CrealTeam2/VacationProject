using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs1_Door : InteractionAgent
{
    bool isOpened = false;
    Animation anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animation>();
    }

    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnInteraction()
    {
        base.OnInteraction();

        if (isOpened)
        {
            anim.Play("Stairs1_Door_Close");
            isOpened = false;
            feedbackText = "�� ����";
        }

        else
        {
            anim.Play("Stairs1_Door_Open");
            isOpened = true;
            feedbackText = "�� �ݱ�";
        }
        Invoke("ReEnableInteraction", 0.2f);
    }

    public override void UpdateUnitFromVariable(ref DataUnit unit)
    {
        unit.Bool["IsOpened"] = isOpened;
    }

    public override void UpdateVariableFromUnit(DataUnit unit)
    {
        isOpened = unit.Bool["IsOpened"];
        if (isOpened) anim.Play("Stairs1_Door_Open");
    }

    void ReEnableInteraction()
    {
        AllowInteraction = true;
        Debug.Log("Enabled");
    }
}
