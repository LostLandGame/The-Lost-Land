using UnityEngine;
using System.Collections.Generic;

namespace LostLand.Combat.Map
{
    public class MapTile : MonoBehaviour
    {
        [SerializeField]
        private Transform ModelTarget;

        private GameObject visualRep;

        private MapTileData tileData;

        private int minPosX;
        private int minPosY;
        private int maxPosX;
        private int maxPosY;

        private static Dictionary<MapTileData.MapTileType, GameObject> tileDict = new Dictionary<MapTileData.MapTileType, GameObject>();

        public void Init( MapTileData data)
        {
            tileData = data;

            InitModel();
        }

        private void InitModel()
        {
            if (tileData == null)
                return;

            GameObject go = null;
            if(tileDict.ContainsKey(tileData.TileType))
            {
                go = tileDict[tileData.TileType];
            }
            else
            {
                go = Resources.Load(tileData.Path) as GameObject;
                if(go == null)
                {
                    Debug.LogError("Could not load tile at path " + tileData.Path);
                    return;
                }

                tileDict.Add(tileData.TileType, go);
            }

            visualRep = Instantiate(go);

            if(visualRep != null)
            {
                visualRep.transform.parent = ModelTarget;
                visualRep.transform.localPosition = Vector3.zero;
            }
        }

        public void SetPosition(int width)
        {
            minPosX = (int)transform.position.x;
            minPosY = (int)transform.position.z;
            maxPosX = minPosX + width;
            maxPosY = minPosY + width;
        }

        public float GetMovementCost()
        {
            if (tileData == null)
                return 1f;

            return tileData.TraversalCost;
        }
    }
}