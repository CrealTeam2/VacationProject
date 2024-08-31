using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>, ISingletonStart
{
    List<List<InteractionAgent>> interactionTypeList;
    List<InteractionAgent> allInteractions;
    List<InteractionAgent> nearInteractions;
    GameObject player;
    Dictionary<KeyCode, Icon> iconDict;

    InteractionAgent enabledInteraction;

    public void RegisterInteraction(InteractionAgent agent)
    {
        allInteractions.Add(agent);
        if (!iconDict.ContainsKey(agent.key))
        {
            iconDict.Add(agent.key, IconManager.Instance.GetIcon(agent.key, 40));
        }
    }

    void Init()
    {
        interactionTypeList = new();
        allInteractions = new();
        nearInteractions = new();
        iconDict = new();
    }

    private void Awake()
    {
        Init();
        
    }

    public void IStart()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
        for(int i = 0; i < nearInteractions.Count;)
        {
            if ((nearInteractions[i].FeedbackTransform.position - player.transform.position).magnitude > nearInteractions[i].detectDistance
                || nearInteractions[i].AllowInteraction == false)
            {
                nearInteractions.RemoveAt(i);
                continue;
            }
            i++;
        }

        List<InteractionAgent> removed = new();
        foreach (InteractionAgent agent in allInteractions) 
        {
            if (agent == null)
            {
                removed.Add(agent);
                continue;
            }
            if((agent.FeedbackTransform.position - player.transform.position).magnitude < agent.detectDistance
                && !nearInteractions.Find(inter => inter == agent)
                && agent.AllowInteraction)
                nearInteractions.Add(agent);
        }

        while (removed.Count > 0)
        {
            allInteractions.Remove(removed[0]);
            removed.RemoveAt(0);
        }

        for (int i = 0; i < nearInteractions.Count;)
        {
            if (Camera.main.WorldToScreenPoint(nearInteractions[i].FeedbackTransform.position).z > 0)
            {
                i++;
                continue;
            }
            nearInteractions.Remove(nearInteractions[i]);
        }

        var nearestInteraction = nearInteractions.OrderBy
            (interplay => (Camera.main.WorldToScreenPoint(interplay.FeedbackTransform.position) 
            - new Vector3(Screen.width / 2, Screen.height / 2)).magnitude).FirstOrDefault();



        Vector3 screenPosition = Vector3.zero;
        if (nearestInteraction)
            screenPosition = Camera.main.WorldToScreenPoint(nearestInteraction.FeedbackTransform.position);
        
        if (nearestInteraction == null 
            || (screenPosition - new Vector3(Screen.width / 2, Screen.height / 2)).magnitude > 600)
        {
            foreach (var ui in iconDict.Values)
            {
                ui.Disable();
            }
            return;
        }

        if (nearestInteraction != enabledInteraction)
        {
            enabledInteraction = nearestInteraction;
        }

        iconDict[enabledInteraction.key].Enable();
        iconDict[enabledInteraction.key].SetPosition(screenPosition.x, screenPosition.y);
        iconDict[enabledInteraction.key].SetText("[" + enabledInteraction.key.ToString() + enabledInteraction.feedbackText + "]");


        if (Input.GetKeyDown(enabledInteraction.key))
        {
            enabledInteraction.OnInteraction();
        }
    }

}
