using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    List<List<InteractionAgent>> interactionTypeList = new();
    List<InteractionAgent> allInteractions = new();
    List<InteractionAgent> nearInteractions = new();
    GameObject player;
    Icon fIcon;
    public float interactionDistance;

    InteractionAgent enabledInteraction;

    public void RegisterInteraction(InteractionAgent agent)
    {
        allInteractions.Add(agent);
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        fIcon = IconManager.Instance.GetIcon(KeyCode.F, 80);
        fIcon.Disable();
    }

    public void Update()
    {
        for(int i = 0; i < nearInteractions.Count;)
        {
            if ((nearInteractions[i].FeedbackTransform.position - player.transform.position).magnitude > interactionDistance
                || nearInteractions[i].AllowInteraction == false)
            {
                nearInteractions.RemoveAt(i);
                continue;
            }
            i++;
        }

        foreach(InteractionAgent agent in allInteractions) 
        {
            if((agent.FeedbackTransform.position - player.transform.position).magnitude < interactionDistance
                && !nearInteractions.Find(inter => inter == agent)
                && agent.AllowInteraction)
                nearInteractions.Add(agent);
        }



        var nearestInteraction = nearInteractions.OrderBy
            (interplay => (Camera.main.WorldToScreenPoint(interplay.FeedbackTransform.position) 
            - new Vector3(Screen.width / 2, Screen.height / 2)).magnitude).FirstOrDefault();
        Vector2 screenPosition = Vector2.zero;
        if (nearestInteraction)
            screenPosition = Camera.main.WorldToScreenPoint(nearestInteraction.FeedbackTransform.position);

        if (nearestInteraction == null 
            || ((Vector3)screenPosition - new Vector3(Screen.width / 2, Screen.height / 2)).magnitude > 600)
        {
            fIcon.Disable();
            return;
        }

        if (nearestInteraction != enabledInteraction)
        {
            enabledInteraction = nearestInteraction;
        } 

        fIcon.Enable();
        fIcon.SetPosition(screenPosition.x, screenPosition.y);


        if (Input.GetKeyDown(KeyCode.F))
        {
            enabledInteraction.OnInteraction();
        }
    }

}
