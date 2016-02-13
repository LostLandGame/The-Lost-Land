using UnityEngine;
using System.Collections;

namespace LostLand.Combat.Ship
{
    public class MovementWaypoint
    {
        public float DistanceRemaining;
        public float DistanceTravelled;

        public Vector2 CurrentPosition;
        public Vector2 FacingDirection;
    }
}