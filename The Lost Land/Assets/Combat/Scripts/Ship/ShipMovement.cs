﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LostLand.Combat.Interaction;
using LostLand.Combat.Map;

namespace LostLand.Combat.Ship
{
    public class ShipMovement : TurnbasedModule
    {
        [SerializeField]
        private float MoveDistance;

        [SerializeField]
        private float TurnSpeed;

        [SerializeField]
        private int MaxWaypoints;

        protected Vector3 startPosition;
        protected float distanceTravelled = 0f;
        protected int currentIndex = 0; // Use this to determine how far you already went, and to roll back changes

        protected MovementWaypoint[] waypoints;

        // TODO: Move indicator

        void Awake()
        {
            PlayerInput.OnMapClicked += OnMapClicked;

            waypoints = new MovementWaypoint[MaxWaypoints];
        }

        void OnDestroy()
        {
            PlayerInput.OnMapClicked -= OnMapClicked;
        }

        protected override void ResetValues()
        {
            base.ResetValues();

            distanceTravelled = 0f;
            startPosition = transform.position;

            waypoints[0].DistanceTravelled = 0f;
            waypoints[0].DistanceRemaining = MoveDistance;
            waypoints[0].FacingDirection = transform.forward;

            currentIndex = 0;
        }

        private void OnMapClicked(Vector2 position)
        {
            if(currentIndex >= waypoints.Length - 1)
            {
                // Max waypoints reached, don't do anything
                return;
            }
            
            float distance = MapGenerator.GetDistance(waypoints[currentIndex].CurrentPosition, position);
            float totalDistance = waypoints[currentIndex].DistanceTravelled + distance;

            Vector2 direction = (position - waypoints[currentIndex].CurrentPosition).normalized;

            if (totalDistance > MoveDistance)
            {
                position = waypoints[currentIndex].CurrentPosition + direction * (waypoints[currentIndex].DistanceRemaining);
                totalDistance = MoveDistance;
            }

            currentIndex++;

            waypoints[currentIndex].DistanceTravelled = totalDistance;
            waypoints[currentIndex].DistanceRemaining = totalDistance - MoveDistance;

            waypoints[currentIndex].CurrentPosition = position;
            waypoints[currentIndex].FacingDirection = direction;
        }

        public void ShowMoveIndicator()
        {

        }

        public void HideMoveIndicator()
        {

        }

        public void RemoveWaypoint()
        {
            if(currentIndex <= 0)
            {
                // TODO: Abort movement
                return;
            }

            currentIndex--;

            distanceTravelled = waypoints[currentIndex].DistanceTravelled;
        }

        public void ResetWaypoints()
        {

        }
    }
}