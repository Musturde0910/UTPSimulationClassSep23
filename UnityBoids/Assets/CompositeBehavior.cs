using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    public FlockBehavior[] behaviors;
    public float[] weight;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock ) {
        if (context.Count == 0) {
            return agent.transform.up;
        }

        if (weight.Length != behaviors.Length) {
            Debug.LogError("Data mismatch", this);
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;
        for (int i=0; i< behaviors.Length; i++) {
            FlockBehavior fb = behaviors[i];
            float wgt = weight[i];
            Vector2 dir = fb.CalculateMove(agent, context, flock);
            move += (dir * wgt);
        }
        return move;
      }
}
