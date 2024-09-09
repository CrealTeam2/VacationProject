using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixGenerator : InteractionAgent
{
    [SerializeField] GameObject activator;
    bool isUsed = false;
    Animation anim;

    private void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        base.Start();
        SoundManager.Instance.PlaySound(gameObject, "BrokenGeneratorSound", 1, 99999);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnInteraction()
    {
        base.OnInteraction();
        SoundManager.Instance.StopSound(gameObject, "BrokenGeneratorSound");
        SoundManager.Instance.PlaySound(gameObject, "GeneratorSound", 0.6f, 1);
        GameManager.Instance.ElectorcitySupply = true;
        GameManager.Instance.onGeneratorOn.Invoke();
        activator.SetActive(true);
    }
    public override void UpdateUnitFromVariable(ref DataUnit unit)
    {
        unit.Bool["IsUsed"] = AllowInteraction;
    }

    public override void UpdateVariableFromUnit(DataUnit unit)
    {
        AllowInteraction = unit.Bool["IsUsed"];
    }
}
