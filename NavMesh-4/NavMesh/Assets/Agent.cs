using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Agent : MonoBehaviour
{
    static string[] possibleTags = {"Group-1", "Group-2"};


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

        int tagIndex = UnityEngine.Random.Range(0, possibleTags.Length);
        this.tag = possibleTags[tagIndex];
        Debug.Log("Created a "+this.tag+" agent");
        if (tagIndex == 0) {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            GetComponent<Renderer>().material.color = Color.blue;
        }

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
        else {
            if (Input.GetMouseButtonDown(1)) {
                if (rnd.Next(2) == 0)
                    return;

                followMouse = true;        
                Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(movePosition, out var hitInfo)) {
                    navagent.SetDestination(hitInfo.point);            
                }
            } 
            else {
                Vector3 gangCenter = getGroupCentre();
                navagent.SetDestination(gangCenter);
            }
       }

    }

    public Vector3 getGroupCentre() {
        Vector3 center = Vector3.zero;
        GameObject[] gang = GameObject.FindGameObjectsWithTag(this.tag);
        foreach (GameObject go in gang) 
        {
            Agent comrade = go.GetComponent<Agent>();
            if (comrade == this) {
                continue;
            }
            center += comrade.transform.position;
        }
        center /= gang.Length;

        return center;
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
