using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_Moving_Running : State<Player>
{
    public PlayerMovements_Moving_Running(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        
    }
    const float runActivationRate = 10.0f;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SoundManager.Instance.PlaySound(origin.transform.Find("RunSoundRange").gameObject, "RunStep", 0.2f, 999);
        SoundManager.Instance.PlaySound(origin.transform.Find("Rotator").Find("Main Camera").gameObject, "Breading", 1, 999);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        SoundManager.Instance.StopSound(origin.transform.Find("RunSoundRange").gameObject, "RunStep");
        SoundManager.Instance.StopSound(origin.transform.Find("Rotator").Find("Main Camera").gameObject, "Breading");
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        origin.Stamina = Mathf.Max(0, origin.Stamina - 10.0f * Time.fixedDeltaTime);
        SoundManager.Instance.ChangeVolume(origin.transform.Find("Rotator").Find("Main Camera").gameObject, "Breading", 0.01f * (100 - origin.Stamina));
        origin.MovePos((origin.transform.forward * Input.GetAxisRaw("Vertical") + origin.transform.right * Input.GetAxisRaw("Horizontal")).normalized * Time.fixedDeltaTime * origin.runSpeed);
        foreach (var i in origin.runSoundRange.detected)
        {
            i.Activation += runActivationRate * Time.fixedDeltaTime;
        }
        if (!Input.GetKey(KeyCode.LeftShift) || origin.Stamina <= 0.0f || !origin.canSprint)
        {
            parentLayer.ChangeState("Walking");
            SoundManager.Instance.PlaySound(origin.transform.Find("Rotator").Find("Main Camera").gameObject, "HeavyBreading", 1, 999);
        }
    }
}
