using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Behavior/Cohesion")]
public class CohesionBehavior : FlockBehavior
{
  public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock ) {
    if (context.Count == 0) {
        return Vector2.zero;
    }

    Vector2 center = Vector2.zero;
    foreach (Transform t in context) {
        center += (Vector2) t.position;
    }
    center /= context.Count;

    Vector2 translation = center - (Vector2) agent.transform.position;
    translation.Normalize();
    
    return translation;
  }
}
