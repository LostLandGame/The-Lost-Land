using UnityEngine;
using System.Collections;

using LostLand.Combat.Map;

namespace LostLand.Combat.Interaction
{
    public class PlayerInput : MonoBehaviour
    {
        public static System.Action<Vector2> OnMapClicked;

        private int mapLayer = 0;

        void Awake()
        {
            mapLayer = LayerMask.NameToLayer("MapTile");
        }

        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(!MapRaycast(Input.mousePosition))
                {
                    // TODO: Do other raycasts
                }
            }
        }

        private bool MapRaycast(Vector3 position)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mapLayer))
            {
                if(OnMapClicked != null)
                {
                    OnMapClicked(new Vector2(hit.point.x, hit.point.z));
                }

                return true;
            }

            return false;
        }
    }
}