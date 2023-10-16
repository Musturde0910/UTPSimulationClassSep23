using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{

    public NavMeshAgent agent;

    List<AgentQueue> queues = new List<AgentQueue>();
    bool inQueue;

    Collider agentCollider;
    public Collider AgentCollider {get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
         agentCollider = GetComponent<CapsuleCollider>();
         var foundQ = GameObject.FindGameObjectsWithTag("Queue");
         foreach (GameObject qObj in foundQ) {
            queues.Add(qObj.GetComponent<AgentQueue>());
            Debug.Log("Added a queue");
         }      
    }

    public bool CollideSomething() {
        float rad = GetComponent<CapsuleCollider>().radius;
        Collider[] contextCollider = Physics.OverlapSphere(transform.position, rad);
        if (contextCollider.Length > 1)
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(1)) {
        if (inQueue == true)
            return;

        Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(movePosition, out var hitInfo)) {
            agent.SetDestination(hitInfo.point);            
        }
       } 
    }

    public bool IsInQueue() {
        return inQueue;
    }
    public void MoveToQueue()
    {
            AgentQueue chosenQ = queues[Random.Range(0, queues.Count)];
            chosenQ.Add(this);
            inQueue = true;
    }

    public void SetDestination(Vector3 pos) {
        agent.SetDestination(pos);
    }
}
