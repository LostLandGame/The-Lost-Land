using UnityEngine;
using System.Collections;

using LostLand.Utility.ObjectPool;

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

        public void Init( MapTileData data)
        {
            tileData = data;

            InitModel(tileData.Path);
        }

        private void InitModel(string path)
        {
            visualRep = ObjectPoolManager.SpawnObject(path);

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
    }
}