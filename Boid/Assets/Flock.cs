using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid boidPrefab;
    List<Boid> boidAgents = new List<Boid>();

    public int startingCount = 100;
    const float AgentDensity = 0.08f;    

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<startingCount; i++) {
            Boid agent = Instantiate(boidPrefab,
                    // how the boid will be distributed 
                    UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
                    // direction for each boid
                    Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)), 
                    transform);

            agent.name = "Boid-"+i;
            boidAgents.Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
