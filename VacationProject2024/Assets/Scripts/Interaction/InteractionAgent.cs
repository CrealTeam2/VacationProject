using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

public class InteractionAgent : MonoBehaviour
{
    [ContextMenu("GenerateGuid")]
    void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private Transform feedbackTransform;
    public Vector3 feedbackPosition;
    public string feedbackText;
    public string id;


    public bool AllowInteraction { get; protected set; }

    public KeyCode key;
    public float detectDistance;

    public virtual void OnInteraction()
    {
        AllowInteraction = false;
    }
    public virtual void UpdateUnitFromVariable(ref DataUnit du) { }
    public virtual void UpdateVariableFromUnit(DataUnit du) { }


    private void Awake()
    {
        InteractionManager.Instance.RegisterInteraction(this);
    }
    protected void Start()
    {


        if (feedbackTransform == null) feedbackPosition = transform.position;
        else feedbackPosition = feedbackTransform.position;
        AllowInteraction = true;
    }
}
