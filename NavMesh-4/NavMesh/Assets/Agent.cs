using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Agent : MonoBehaviour
{
    NavMeshAgent navagent;
    public QueueList queueList;

    bool inQueue;
    bool followMouse;
    System.Random rnd = new System.Random();

    Collider agentCollider;
    public Collider AgentCollider {get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
         agentCollider = GetComponent<CapsuleCollider>();
         navagent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inQueue == true)
            return;

        if (followMouse == true) {
            if (navagent.remainingDistance <= navagent.stoppingDistance) {
                followMouse = false;
            }
        }

       if (followMouse == false && Input.GetMouseButtonDown(1)) {
            if (rnd.Next(2) == 0)
                return;

            followMouse = true;        
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePosition, out var hitInfo)) {
                navagent.SetDestination(hitInfo.point);            
            }
        } 
    }

    public Vector3 getGroupCentre() {
        return Vector3.zero;
    }

    public bool IsInQueue() {
        return inQueue;
    }
    public void MoveToQueue()
    {
        AgentQueue chosenQ = queueList.Get(rnd.Next(queueList.Count()));
        chosenQ.Add(this);
        inQueue = true;
    }

    public void SetDestination(Vector3 pos) {
        navagent.SetDestination(pos);
    }
}
