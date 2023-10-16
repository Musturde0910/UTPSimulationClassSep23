using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// if MonoBehavior, cannot CreateAssetMenu
public abstract class FlockBehavior : ScriptableObject
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock );

}
