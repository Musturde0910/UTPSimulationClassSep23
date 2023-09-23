using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock ) {
        if (context.Count == 0) {
            return Vector2.zero;
        }

        Vector2 avoidvec = Vector2.zero;
        int nAvoid = 0;
        foreach (Transform t in context) {

            Vector2 closestPoint = t.gameObject.GetComponent<Collider2D>().ClosestPoint(agent.transform.position);
            if (Vector2.SqrMagnitude(closestPoint - (Vector2) agent.transform.position) < flock.SquareAvoidanceRadius) {            
            //if (Vector2.SqrMagnitude(t.position - agent.transform.position) < flock.SquareAvoidanceRadius) {
                avoidvec += (Vector2) ((Vector2)agent.transform.position - closestPoint);                
                //avoidvec += (Vector2) (agent.transform.position - t.position);
                nAvoid++;
            }
        }

        if (nAvoid > 0) {
           avoidvec /= nAvoid;
           avoidvec.Normalize();
        }


        return avoidvec;
      }

}
