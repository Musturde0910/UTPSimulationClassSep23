using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class FlockAgent : MonoBehaviour
{
    Collider2D agentCollider;
    public Collider2D AgentCollider {get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
       agentCollider = GetComponent<Collider2D>();         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

GameObject[] FindGameObjectsInLayer(int layer)
{
    var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
    var goList = new System.Collections.Generic.List<GameObject>();
    for (int i = 0; i < goArray.Length; i++)
    {
        if (goArray[i].layer == layer)
        {
            goList.Add(goArray[i]);
        }
    }
    if (goList.Count == 0)
    {
        return null;
    }
    return goList.ToArray();
}

    public bool NearObstacle(float ProbeLgth) {
        var mask = LayerMask.GetMask("Obstacle");        
        GameObject[] candidate = FindGameObjectsInLayer(8);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up*ProbeLgth, 
                                            ProbeLgth, mask);
        Debug.DrawRay(transform.position, transform.up*ProbeLgth, Color.red, 2, false);
        if (hit.collider != null) {
                return true;
        }
        return false;        
    }

    public void Move(Vector2 velocity) {
        transform.up = velocity;
        transform.position += (Vector3) velocity * Time.deltaTime;
    }

}
