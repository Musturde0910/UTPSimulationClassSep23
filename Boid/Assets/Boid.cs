using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Boid : MonoBehaviour
{
    Collider2D agentCollider;
    public Collider2D AgentCollider {get { return agentCollider; } }

    Vector2 randmotion;

    // Start is called before the first frame update
    void Start()
    {
       agentCollider = GetComponent<Collider2D>();         
       randmotion = new Vector2(UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5));
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3) randmotion; 
    }

    public void Move(Vector2 velocity) {
        transform.up = velocity;
        transform.position += (Vector3) velocity * Time.deltaTime;
    }

}
