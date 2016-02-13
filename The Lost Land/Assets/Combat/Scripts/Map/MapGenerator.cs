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
        
        private static MapGenerator mapInstance;

        public static MapGenerator GetMapGenerator()
        {
            return mapInstance;
        }

        #region Generation
        void Awake()
        {
            mapInstance = this;

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

            if (map != null)
            {
                for (int i = 0; i < map.Length; ++i)
                {
                    DestroyImmediate(map[i].gameObject);
                }
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

        #region Distance Calculation
        public static float GetDistance(Vector2 start, Vector2 end)
        {
            return mapInstance.getDistance(start, end);
        }

        public static Vector2 GetEndPoint(Vector2 start, Vector2 direction, float maxDistance)
        {
            return mapInstance.getEndPoint(start, direction, maxDistance);
        }

        #region Helpers
        private static float margin = 0.01f;

        public float getDistance(Vector2 start, Vector2 end)
        {
            int x = Mathf.FloorToInt(start.x / TileWidth);
            int y = Mathf.FloorToInt(start.y / TileWidth);

            MapTile tile = map[x + y * MapWidth];
            float cost = tile.GetMovementCost();

            Vector2 intermediate = GetNextBoundaryPoint(start, end);

            float distToNext = Vector2.Distance(start, intermediate);
            float distToGoal = Vector2.Distance(start, end);
            if (distToGoal < distToNext)
            {
                return distToGoal * cost;
            }
            else
            {
                return distToNext * cost + getDistance(intermediate, end);
            }
        }

        public Vector2 HelpGetEndPoint(Vector2 start, Vector2 direction, float maxDistance)
        {
            return getEndPoint(start, direction.normalized, maxDistance);
        }

        public Vector2 getEndPoint(Vector2 start, Vector2 direction, float maxDistance)
        {
            int x = Mathf.FloorToInt(start.x / TileWidth);
            int y = Mathf.FloorToInt(start.y / TileWidth);

            int index = x + y * MapWidth;
            if(x < 0 || x >= MapWidth || y < 0 || y >= MapHeight) // Going off the map
            {
                return start;
            }

            MapTile tile = map[index];
            float cost = tile.GetMovementCost();

            Vector2 intermediate = GetNextBoundaryPointWithDirection(start, direction);

            float distToNext = Vector2.Distance(start, intermediate) * cost;
            if (maxDistance < distToNext)
            {
                return start + maxDistance * direction / cost; // Max distance reached
            }
            else
            {
                return getEndPoint(intermediate, direction, maxDistance - distToNext * cost); // Recurse
            }
        }

        private Vector2 GetNextBoundaryPoint(Vector2 start, Vector2 end)
        {
            Vector2 dir = (end - start).normalized;
            return GetNextBoundaryPointWithDirection(start, dir);
        }

        private Vector2 GetNextBoundaryPointWithDirection(Vector2 start, Vector2 direction)
        {
            Vector2 result = Vector2.zero;
            
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y)) // Horizontal movement
            {
                float max = 0f;
                if (direction.x >= 0f)
                {
                    max = Mathf.Ceil(start.x / TileWidth) * TileWidth;
                }
                else
                {
                    max = Mathf.Floor(start.x / TileWidth) * TileWidth;
                }

                float diff = Mathf.Abs(max - start.x);
                result = start + (diff + margin) * direction / Mathf.Abs(direction.x);
            }
            else // Vertical movement
            {
                float max = 0f;
                if (direction.y >= 0f)
                {
                    max = Mathf.Ceil(start.y / TileWidth) * TileWidth;
                }
                else
                {
                    max = Mathf.Floor(start.y / TileWidth) * TileWidth;
                }

                float diff = Mathf.Abs(max - start.y);
                result = start + (diff + margin) * direction / Mathf.Abs(direction.y);
            }

            return result;
        }
        #endregion
        #endregion
    }
}