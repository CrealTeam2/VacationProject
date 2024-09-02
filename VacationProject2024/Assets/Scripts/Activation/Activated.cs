using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activated : MonoBehaviour, ISavable
{
    [SerializeField] string id;
    protected virtual bool oneTime => true;
    protected bool activated = false;
    public void SaveData(ref Database db)
    {
        db.activatedDatas[id] = activated;
    }
    public void LoadData(Database db)
    {
        db.activatedDatas.TryGetValue(id, out activated);
    }
    public void Activate()
    {
        if (activated && oneTime) return;
        OnActivate();
    }
    protected virtual void OnActivate()
    {
        activated = true;
    }
}
