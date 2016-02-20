using UnityEngine;
using System.Collections;

using LostLand.Combat.Map;

namespace LostLand.Combat.Ship
{
    public class Ship : MonoBehaviour
    {
        public static Ship ActiveShip;

        [SerializeField]
        private ShipMovement engine;

        // TODO: Add weapons module array

        [SerializeField]
        private float health;

        void Awake()
        {
            ActiveShip = this;

            MapGenerator.OnMapCreated += Init;
        }

        void OnDestroy()
        {
            MapGenerator.OnMapCreated -= Init;
        }

        private void Init()
        {
            if(engine)
            {
                engine.Init();
            }
        }

        public void ApplyMovement()
        {
            if(engine)
            {
                engine.ApplyMovement();
            }
        }
    }
}