using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAgent : MonoBehaviour
{
    [SerializeField] private Transform feeabackTransform;
    public Transform FeedbackTransform => feeabackTransform;
    public string feedbackText;
    public int id;


    public bool AllowInteraction { get; protected set; }

    public KeyCode key;
    public float detectDistance;

    public virtual void OnInteraction()
    {
        AllowInteraction = false;
    }

    protected void Start()
    {
        InteractionManager.Instance.RegisterInteraction(this);
        AllowInteraction = true;
    }
}
