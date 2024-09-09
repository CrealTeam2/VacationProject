using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Door : InteractionAgent
{
    bool isOpened = false;
    Animation anim;
    [Header("Opening")]
    [SerializeField] string openAnim;
    [SerializeField] string openText;
    [SerializeField] string openSound;

    [Header("Closing")]
    [SerializeField] string closeAnim;
    [SerializeField] string closeText;
    [SerializeField] string closeSound;

    [Header("Pathfinding")]
    [SerializeField] NavMeshLink link;

    private void Awake()
    {
        base.Awake();
        anim = GetComponent<Animation>();
        feedbackText = openText;
        if (link != null) link.enabled = false;
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
            if(link != null) link.enabled = false;
            anim.Play(closeAnim);
            isOpened = false;
            feedbackText = openText;
            SoundManager.Instance.PlaySound(gameObject, closeSound, 1, 1);
        }

        else
        {
            if(link != null) link.enabled = true;
            anim.Play(openAnim);
            isOpened = true;
            feedbackText = closeText;
            SoundManager.Instance.PlaySound(gameObject, openSound, 1, 1);
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
        if (isOpened)
        {
            if(link != null) link.enabled = true;
            anim.Play(openAnim);
            feedbackText = closeText;
        }
        else
        {
            if(link != null) link.enabled = false;
            feedbackText = openText;
        }
    }


    void ReEnableInteraction()
    {
        AllowInteraction = true;
    }
}
