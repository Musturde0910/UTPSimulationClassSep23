using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid boidPrefab;
    List<Boid> boidAgents = new List<Boid>();
    public FlockBehavior behavior;

    public int startingCount = 100;
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
        for (int i=0; i<startingCount; i++) {
            Boid agent = Instantiate(boidPrefab, // duplicate the prefab
                    // position of the duplicate
                    UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
                    // rotation / orientation
                    Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
                    // the position, orientation etc of the flock itself 
                    transform);

            agent.name = "Boid-"+i;
            boidAgents.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Boid agent in boidAgents) {
            // get the positions of the surrounding neighbors
            List<Transform> neighs = GetNearbyObjects(agent);
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6.0f);

            Vector2 move = behavior.CalculateMove(agent, neighs, this);

            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed) {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move);
        }        
    }

    List<Transform> GetNearbyObjects(Boid agent) {
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
