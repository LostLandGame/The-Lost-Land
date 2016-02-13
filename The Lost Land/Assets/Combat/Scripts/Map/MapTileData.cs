using UnityEngine;
using System.Collections;

namespace LostLand.Combat.Map
{
    [System.Serializable]
    public class MapTileData
    {
        public enum MapTileType
        {
            WATER = 0,
            SLUDGE = 1
        }

        public MapTileType TileType;
        public int TraversalCost;
        public float Chance;

        public string Path;
    }
}