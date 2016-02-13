using UnityEngine;
using System.Collections;

namespace LostLand.Combat.Ship
{
    public class MovementWaypoint
    {
        public float DistanceRemaining;
        public float DistanceTravelled;

        public Vector3 CurrentPosition;
        public Vector3 FacingDirection;
    }
}