using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Behavior/Alignment")]
public class AlignmentBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock ) {
        if (context.Count == 0) {
            return agent.transform.up;
        }

        Vector2 up = Vector2.zero;
        foreach (Transform t in context) {
            up += (Vector2) t.transform.up;
        }
        up /= context.Count;
        up.Normalize();

        return up;
      }

}
