using UnityEngine;
using System.Collections;

using LostLand.Combat.Map;

namespace LostLand.Combat.Interaction
{
    public class PlayerInput : MonoBehaviour
    {
        public static System.Action<Vector2> OnMapClicked;

        private int mapLayer = 0;

        private Vector2 oldPos = Vector2.zero;

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

            if(Input.GetMouseButtonDown(1))
            {
                oldPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                Vector2 currPos = Input.mousePosition;
                transform.Translate(new Vector3((currPos.x - oldPos.x) / Screen.width, 0f, (currPos.y - oldPos.y) / Screen.height), Space.World);

                oldPos = currPos;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (!Mathf.Approximately(scroll, 0f))
            {
                transform.Translate(Vector3.up * scroll, Space.World);
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