using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace LostLand.Combat.Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private int MapWidth;
        [SerializeField]
        private int MapHeight;

        [SerializeField]
        private int TileWidth;

        [SerializeField]
        private GameObject TilePrefab;

        [SerializeField]
        private MapTileData[] Data;
        private Dictionary<MapTileData.MapTileType, MapTileData> tileData;

        private MapTile[] map;

        #region Generation
        void Awake()
        {
            InitData();

            CreateMap();
        }

        private void InitData()
        {
            tileData = new Dictionary<MapTileData.MapTileType, MapTileData>();

            foreach (MapTileData data in Data)
            {
                if (!tileData.ContainsKey(data.TileType))
                {
                    tileData.Add(data.TileType, data);
                }
            }
        }

        [ContextMenu("Create Map")]
        private void CreateMap()
        {
            if(tileData == null)
            {
                InitData();
            }

            Vector3 startPosition = Vector3.zero;
            Vector3 xDir = Vector3.right * TileWidth;
            Vector3 zDir = Vector3.forward * TileWidth;

            System.Array ar = System.Enum.GetValues(typeof(MapTileData.MapTileType));
            float rand = 0f;
            float acc = 0f;
            MapTileData d = null;

            map = new MapTile[MapWidth * MapHeight];
            GameObject go = null;
            MapTile tile = null;

            for (int x = 0; x < MapWidth; ++x)
            {
                for(int y = 0; y < MapHeight; ++y)
                {
                    acc = 0f;
                    rand = Random.Range(0f, 1f);
                    foreach(MapTileData.MapTileType r in ar)
                    {
                        acc += tileData[r].Chance;
                        if(rand <= acc)
                        {
                            d = tileData[r];
                            break;
                        }
                    }

                    go = Instantiate(TilePrefab);
                    if(go)
                    {
                        go.transform.SetParent(transform, true);
                        go.transform.position = startPosition + xDir * x + zDir * y;

                        tile = go.GetComponent<MapTile>();
                        if(tile)
                        {
                            tile.Init(d);
                            tile.SetPosition(TileWidth);

                            map[x + y * MapWidth] = tile;
                        }
                    }
                }
            }
        }
        #endregion
    }
}