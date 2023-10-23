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

    System.Random rnd = new System.Random();

    DateTime motionT;
    Vector3 lastPosition;

    DateTime mouseT;
    const float mouseTime = 5;
    AgentState currState;

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

        navagent.SetDestination(transform.position + RandomNearPosition());
        currState = AgentState.Wandering;
    }

    // Update is called once per frame
    void Update()
    {
        if (currState == AgentState.InQueue)
            return;

        if (currState == AgentState.ToQueue) {
            if (ReachedDestination()) {
                currState = AgentState.InQueue;
            }

            return;
        }

        if (currState == AgentState.FollowingMouse) {
            if (ReachedDestination()) {
                currState = AgentState.Wandering;
            }

            DateTime mouseTNow = DateTime.Now;
            var diffTime = mouseTNow - mouseT;
            if (diffTime.Seconds > mouseTime)
                currState = AgentState.Wandering;
            return;
        }

        // currState == Wandering

        if (Input.GetMouseButtonDown(1)) {
                if (rnd.Next(2) == 0)
                    return;
                
                Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(movePosition, out var hitInfo)) {
                    navagent.SetDestination(hitInfo.point);
                    currState = AgentState.FollowingMouse; 
                    mouseT = DateTime.Now;       
                }
            } 
        else {
            if (ReachedDestination()) {
                Debug.Log("Agent "+this.name +" is moving elsewhere");
                Vector3 wander = RandomNearPosition();
                Vector3 gangPos = CalcAttractionToGang();
                Vector3 dir = wander + gangPos;  
                SetDestination(transform.position + dir);
                }
            }

    }

    Vector3 CalcAttractionToGang() {
        List<Agent> agents = GetNearbyAgents(true);
        Vector3 center = Vector3.zero;
        if (agents.Count == 0) {
            return transform.position;
        }        
        foreach (Agent agent in agents) {
            center += agent.transform.position;
        }
        center /= agents.Count;
        Vector3 dir = center - transform.position;
        return dir;
    }

    List<Agent> GetNearbyAgents(bool aliketype) {
        Collider[] hitColliders = new Collider[20];
        float radius = 5;
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders);

        List<Agent> context = new List<Agent>();
        for (int i=0; i<numColliders; i++)  {
            Collider c = hitColliders[i];
            Agent agent = c.GetComponent<Agent>();
            if (agent == null)
                continue;

            if (agent.CompareTag(this.tag) != aliketype)
                    continue;
                    
            if (agent.name == this.name) 
                continue;
            
            context.Add(agent);
        }

        return context;
    }

    public bool MovingToQueue() {
        return currState == AgentState.ToQueue;
    }

    public bool IsInQueue() {
        return currState == AgentState.InQueue;
    }

    public string MoveToQueue(int qindex)
    {
        AgentQueue chosenQ = queueList.Get(qindex);
        chosenQ.Add(this);
        currState = AgentState.ToQueue;
        return chosenQ.name;
    }

    public string MoveToQueue()
    {
        AgentQueue chosenQ = queueList.Get(rnd.Next(queueList.Count()));
        chosenQ.Add(this);
        currState = AgentState.ToQueue;
        return chosenQ.name;
    }

    public void MoveFromQueue()
    {
        currState = AgentState.Wandering;
    }

    public void SetDirection(Vector3 dir) {
        navagent.Move(dir);
    }

    Vector3 GetDir() {
        Vector3 pos = navagent.path.corners[0];
        Vector3 dir = pos - transform.position;
        return dir;
    }


    public void SetDestination(Vector3 pos) {
        navagent.SetDestination(pos);
        motionT = DateTime.Now;
    }

    public float GetTimeInMotion() {
        return (DateTime.Now - motionT).Seconds;
    }

    public bool IsNotMoving() {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        Debug.Log("Agent "+this.name+" has speed "+speed);
        lastPosition = transform.position;
        if (speed < 1) {
            Debug.Log(" should be stopping");
        }

        return speed < 1;
    }

    public bool ReachedDestination() {
      return (navagent.remainingDistance <= navagent.stoppingDistance);
    }

    public bool ReachedDestination(float maxsec) {
      if (navagent.remainingDistance <= navagent.stoppingDistance) {
        return true;
      }

      if (GetTimeInMotion() > maxsec) {
        return true;
      }

      return false;
    }


    public Vector3 RandomNearPosition() {

        GameObject ground = GameObject.Find("Ground");
        Vector3 grounddim = ground.transform.localScale;
        Vector3 groundpos = ground.transform.position;
        float y = transform.position.y;

        var x = UnityEngine.Random.Range(groundpos.x-grounddim.x/2, groundpos.x+grounddim.x/2);
        var z = UnityEngine.Random.Range(groundpos.z-grounddim.z/2, groundpos.z+grounddim.z/2);
        Vector3 randPos = new Vector3(x, y, z);
        Vector3 rdir = randPos - transform.position;
        //rdir = Vector3.Normalize(rdir);

        return rdir; //*5;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
                }
            }
        result = Vector3.zero;
        return false;
        }

}
