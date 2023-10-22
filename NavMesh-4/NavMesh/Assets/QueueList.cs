using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Behavior/QueueList")]
public class QueueList : ScriptableObject
{

    [System.NonSerialized]
    List<AgentQueue> queues = new List<AgentQueue>();

    public AgentQueue Get(int i) {
        return queues[i];
    }

    public int Count() {
        return queues.Count;
    }

    public void Add(AgentQueue queue) 
    {
        queues.Add(queue);
    }
}
