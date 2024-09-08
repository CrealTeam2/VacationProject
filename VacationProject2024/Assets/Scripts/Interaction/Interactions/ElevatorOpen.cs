using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOpen : InteractionAgent
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

    private void Awake()
    {
        base.Awake();
        anim = transform.parent.parent.GetComponent<Animation>();
        feedbackText = openText;
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
            anim.Play(closeAnim);
            isOpened = false;
            feedbackText = openText;
            SoundManager.Instance.PlaySound(gameObject, closeSound, 1, 1);
        }

        else
        {
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
            anim.Play(openAnim);
            feedbackText = closeText;
        }
        else feedbackText = openText;
    }


    void ReEnableInteraction()
    {
        AllowInteraction = true;
    }
}
