using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentQueue : MonoBehaviour
{
    public QueueList queueList;

    List<Agent> agentqueue = new List<Agent>();

    //string counterId;
    Vector3 qdir;  // queue direction
    Vector3 qPos;

    // Start is called before the first frame update
    void Start()
    {
        //counterId = "Counter-"+q_id;
        //counter = GameObject.Find(counterId);
        /*
        qdir = -counter.transform.right; // the red vector
        qPos = counter.transform.position + qdir*(agentqueue.Count+1)*2;
        */

        qdir = -transform.right; // the red vector
        qPos = transform.position + qdir*(agentqueue.Count+1)*2;
        queueList.Add(this);

        Debug.Log("Queue created for "+gameObject.name+". With pos: "+qPos);

    }

    public void Add(Agent agent) {
        agentqueue.Add(agent);
        agent.SetDestination(qPos);
        //qPos = counter.transform.position + qdir*(agentqueue.Count+1)*2;
        qPos = transform.position + qdir*(agentqueue.Count+1)*2;        

        Debug.Log("Queue for "+gameObject.name+" now has size "+agentqueue.Count+". Next pos: "+qPos);
    }


    public void Shift() {
        foreach (Agent agent in agentqueue) {
            agent.transform.position -= qdir*2;
        }
    }

    public Agent Pop() {
        GameObject ground = GameObject.Find("Ground");
        Vector3 grounddim = ground.transform.localScale;
        Vector3 groundpos = ground.transform.position;
        float y = groundpos.y + grounddim.y/2;

        var x = Random.Range(groundpos.x-grounddim.x/2, groundpos.x+grounddim.x/2);
        var z = Random.Range(groundpos.z-grounddim.z/2, groundpos.z+grounddim.z/2);
        Vector3 newPos = new Vector3(x, y, z);

        Agent agent = agentqueue[0];
        agentqueue.RemoveAt(0); 

        agent.SetDestination(newPos);
        Shift();
        return agent;  
    }

    public int Size() {
        return agentqueue.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
