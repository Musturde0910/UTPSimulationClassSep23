using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> flockAgents = new List<FlockAgent>();
    public FlockBehavior behavior;

    public int startingCount = 240;
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
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighRadius = neighRadius * neighRadius;
        squareAvoidanceRadius = squareNeighRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    
        for (int i=0; i<startingCount; i++) {
            //Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation, parenttransform);
            FlockAgent agent = Instantiate(agentPrefab, 
                    //Returns a random point inside or on a circle with radius 1.0
                    UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,

                    // Quaternion Euler(float x, float y, float z);
                    // Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis; applied in that order.
                                    // Vector3(0, 0, 1).
                    Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)), 
                    transform);
            agent.name = "Agent-"+i;
            flockAgents.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in flockAgents) {
            List<Transform> context = GetNearbyObjects(agent);
            agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6.0f);

            Console.WriteLine("Behavior: "+behavior);
            Vector2 move = behavior.CalculateMove(agent, context, this);

            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed) {
                move = move.normalized * maxSpeed;
            }

            if (agent.NearObstacle(SquareAvoidanceRadius*5)) {
                move = move * 0.001f;
            }

            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) {
        List<Transform> context = new List<Transform>();
                                        // Get a list of all Colliders that fall within a circular area.
        Collider2D[] contextCollider = Physics2D.OverlapCircleAll(agent.transform.position, neighRadius);
        foreach (Collider2D c in contextCollider) {
            if (c.gameObject.CompareTag("Obstacle") == true)
                    continue;
                    
            if (c == agent.AgentCollider) 
                continue;
            
            context.Add(c.transform);
        }

        return context;
    }

}
