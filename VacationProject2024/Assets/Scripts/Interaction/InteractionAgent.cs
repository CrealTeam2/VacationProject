using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAgent : MonoBehaviour
{
    [SerializeField] private Transform feeabackTransform;
    public Transform FeedbackTransform => feeabackTransform;

    public int id;

    public bool AllowInteraction { get; protected set; }

    public virtual void OnInteraction()
    {
        AllowInteraction = false;
    }

    public void Awake()
    {
        InteractionManager.Instance.RegisterInteraction(this);
        AllowInteraction = true;
    }
}
