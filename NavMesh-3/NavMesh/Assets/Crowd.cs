using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public Agent agentPrefab;
    List<Agent> crowd = new List<Agent>();
 
    public int startingCount = 10;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(1f, 10f)]
    public float neighRadius = 1.5f;

    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; }}

    // Start is called before the first frame update
    void Start()
    {
        GameObject ground = GameObject.Find("Ground");
        Vector3 grounddim = ground.transform.localScale;
        Vector3 groundpos = ground.transform.position;
        float y = groundpos.y + grounddim.y/2;

        while (crowd.Count < startingCount) {
            var x = Random.Range(groundpos.x-grounddim.x/2, groundpos.x+grounddim.x/2);
            var z = Random.Range(groundpos.z-grounddim.z/2, groundpos.z+grounddim.z/2);
            Vector3 spawnPos = new Vector3(x, y, z);

            Agent agent = Instantiate(agentPrefab, 
                                    spawnPos,
                                     Quaternion.identity);
            agent.name = "Agent-"+crowd.Count;

            //if (agent.CollideSomething()) {
            //    continue;
            //}
            crowd.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            for (int i=0; i<3; i++) { // try 3 times
                Agent agent = crowd[Random.Range(0, crowd.Count)];
                if (agent.IsInQueue() == false) {
                    agent.MoveToQueue();
                    break;
                }
            }
        }
    }

    List<Transform> GetNearbyObjects(Agent agent) {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextCollider = Physics2D.OverlapCircleAll(agent.transform.position, neighRadius);
        foreach (Collider2D c in contextCollider) {
            if (c == agent.AgentCollider) 
                continue;
            
            context.Add(c.transform);
        }

        return context;
    }

}