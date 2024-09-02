using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

public class InteractionManager : Singleton<InteractionManager>, ISingletonStart, ISavable
{
    Dictionary<string, InteractionAgent> allInteractions;
    Dictionary<string, InteractionAgent> nearInteractions;
    GameObject player;
    Dictionary<KeyCode, Icon> iconDict;

    InteractionAgent enabledInteraction;

    public void RegisterInteraction(InteractionAgent agent)
    {
        allInteractions[agent.id] = agent;
        if (!iconDict.ContainsKey(agent.key))
        {
            iconDict.Add(agent.key, IconManager.Instance.GetIcon(agent.key, 40));
        }
    }

    void Init()
    {
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
        //근접 상호작용중 멀어진 것이 있는지 확인
        List<string> removed = new();
        foreach (var agentId in nearInteractions.Keys) { 
            if ((nearInteractions[agentId].feedbackTransform.position - player.transform.position).magnitude > nearInteractions[agentId].detectDistance
                || nearInteractions[agentId].AllowInteraction == false)
            {
                removed.Add(agentId);
                continue;
            }
        }
        while (removed.Count > 0)
        {
            nearInteractions.Remove(removed[0]);
            removed.RemoveAt(0);
        }

        
        //모든 상호작용들 중 근접한 상호작용이 될 수 있는것이 있는지
        foreach (var agentId in allInteractions.Keys) 
        {
            if (allInteractions.ContainsKey(agentId) == false)
            {
                removed.Add(agentId);
                continue;
            }
            if ((allInteractions[agentId].feedbackTransform.position - player.transform.position).magnitude < allInteractions[agentId].detectDistance
                && !nearInteractions.ContainsKey(agentId)
                && allInteractions[agentId].AllowInteraction)
                nearInteractions[agentId] = allInteractions[agentId];
        }

        while (removed.Count > 0)
        {
            allInteractions.Remove(removed[0]);
            removed.RemoveAt(0);
        }


        //상호작용이 화면상에 있는지
/*        for (int i = 0; i < nearInteractions.Count;)
        {
            if (Camera.main.WorldToScreenPoint(nearInteractions[i].feedbackPosition).z > 0)
            {
                i++;
                continue;
            }
            nearInteractions.Remove(nearInteractions[i]);
        }*/

        removed.Clear();
        foreach(var agentId in nearInteractions.Keys)
        {
            if (Camera.main.WorldToScreenPoint(nearInteractions[agentId].feedbackTransform.position).z <= 0)
            {
                removed.Add((agentId));
            }
        }

        while (removed.Count > 0)
        {
            nearInteractions.Remove(removed[0]);
            removed.RemoveAt(0);
        }

        var nearestInteraction = nearInteractions.OrderBy
            (interplay => (Camera.main.WorldToScreenPoint(interplay.Value.feedbackTransform.position) 
            - new Vector3(Screen.width / 2, Screen.height / 2)).magnitude).FirstOrDefault();



        Vector3 screenPosition = Vector3.zero;
        if (nearestInteraction.Key != null)
            screenPosition = Camera.main.WorldToScreenPoint(nearestInteraction.Value.feedbackTransform.position);
        
        if (nearestInteraction.Key == null 
            || (screenPosition - new Vector3(Screen.width / 2, Screen.height / 2)).magnitude > 600)
        {
            foreach (var ui in iconDict.Values)
            {
                ui.Disable();
            }
            return;
        }

        if (nearestInteraction.Value != enabledInteraction)
        {
            enabledInteraction = nearestInteraction.Value;
        }

        iconDict[enabledInteraction.key].Enable();
        iconDict[enabledInteraction.key].SetPosition(screenPosition.x, screenPosition.y);
        iconDict[enabledInteraction.key].SetText("[" + enabledInteraction.key.ToString() + enabledInteraction.feedbackText + "]");


        if (Input.GetKeyDown(enabledInteraction.key))
        {
            enabledInteraction.OnInteraction();
        }
    }

    public void LoadData(Database data)
    {
        List<string> removeAgentId = new();
        foreach (var agentId in data.interactionDatas.Keys)
        {
            if (!allInteractions.ContainsKey(agentId))
            {
                removeAgentId.Add(agentId);
                continue;
            }
            allInteractions[agentId].UpdateVariableFromUnit(data.interactionDatas[agentId]);
        }

        while (removeAgentId.Count > 0)
        {
            data.zombieData.Remove(removeAgentId[0]);
            removeAgentId.RemoveAt(0);
        }

    }

    public void SaveData(ref Database data)
    {
        data.interactionDatas.Clear();
        foreach (var agentId in allInteractions.Keys)
        {
            DataUnit dataUnit;
            data.interactionDatas.TryGetValue(agentId, out dataUnit);
            if(dataUnit == null) dataUnit = new DataUnit();

            allInteractions[agentId].UpdateUnitFromVariable(ref dataUnit);
            data.interactionDatas[agentId] = dataUnit;
        }
    }
}
