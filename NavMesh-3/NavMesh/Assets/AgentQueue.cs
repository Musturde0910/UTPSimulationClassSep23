using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentQueue : MonoBehaviour
{
    public int q_id = 0;

    List<Agent> agentqueue = new List<Agent>();

    GameObject counter;
    string counterId;
    Vector3 qdir;  // queue direction
    Vector3 qPos;

    // Start is called before the first frame update
    void Start()
    {
        counterId = "Counter-"+q_id;
        counter = GameObject.Find(counterId);
        qdir = -counter.transform.right; // the red vector
        qPos = counter.transform.position + qdir*(agentqueue.Count+1)*2;

        Debug.Log("Queue "+q_id+" created. With pos: "+qPos);

    }

    public void Add(Agent agent) {
        agentqueue.Add(agent);
        agent.SetDestination(qPos);
        qPos = counter.transform.position + qdir*(agentqueue.Count+1)*2;

        Debug.Log("Queue "+q_id+" now has size "+agentqueue.Count+". Next pos: "+qPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
