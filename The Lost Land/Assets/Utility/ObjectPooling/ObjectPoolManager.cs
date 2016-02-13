using System.Collections.Generic;
using UnityEngine;

namespace LostLand.Utility.ObjectPool
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private static ObjectPoolManager instance;
        public static ObjectPoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Object obj = Resources.Load("ObjectPool");
                    if(obj)
                    {
                        GameObject go = Instantiate(obj) as GameObject;
                        if(go)
                        {
                            instance = go.GetComponent<ObjectPoolManager>();

                            if(instance)
                            {
                                instance.Init();
                            }
                        }
                    }
                }
                return instance;
            }
        }

        private Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup;
        private Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup;

        private Dictionary<string, ObjectPool<GameObject>> pathLookup;
        private Dictionary<string, GameObject> pathPrefabLookup;

        private bool dirty = false;

        void Awake()
        {
            Init();
        }

        private void Init()
        {
            prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
            instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();

            pathLookup = new Dictionary<string, ObjectPool<GameObject>>();
            pathPrefabLookup = new Dictionary<string, GameObject>();
        }

        #region Object spawning
        public void warmPool(GameObject prefab, int size)
        {
            if (prefabLookup.ContainsKey(prefab))
            {
                throw new System.Exception("Pool for prefab " + prefab.name + " has already been created");
            }
            var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); }, size);
            prefabLookup[prefab] = pool;

            dirty = true;
        }

        public GameObject spawnObject(GameObject prefab)
        {
            return spawnObject(prefab, Vector3.zero, Quaternion.identity);
        }

        public GameObject spawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!prefabLookup.ContainsKey(prefab))
            {
                WarmPool(prefab, 1);
            }

            var pool = prefabLookup[prefab];

            var clone = pool.GetItem();
            clone.transform.position = position;
            clone.transform.rotation = rotation;
            clone.SetActive(true);

            instanceLookup.Add(clone, pool);
            dirty = true;
            return clone;
        }

        private GameObject InstantiatePrefab(GameObject prefab)
        {
            var go = Instantiate(prefab) as GameObject;
            return go;
        }
        #endregion

        #region Path spawning
        public void warmPool(string path, int size)
        {
            if (pathLookup.ContainsKey(path))
            {
                throw new System.Exception("Pool for prefab at path " + path + " has already been created");
            }
            var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(path); }, size);
            pathLookup[path] = pool;

            dirty = true;
        }

        public GameObject spawnObject(string path)
        {
            return spawnObject(path, Vector3.zero, Quaternion.identity);
        }

        public GameObject spawnObject(string path, Vector3 position, Quaternion rotation)
        {
            if (!pathLookup.ContainsKey(path))
            {
                WarmPool(path, 1);
            }

            var pool = pathLookup[path];

            var clone = pool.GetItem();
            clone.transform.position = position;
            clone.transform.rotation = rotation;
            clone.SetActive(true);

            instanceLookup.Add(clone, pool);
            dirty = true;
            return clone;
        }

        private GameObject InstantiatePrefab(string path)
        {
            GameObject obj = null;
            if (!pathLookup.ContainsKey(path))
            {
                obj = Resources.Load(path) as GameObject;

                if(obj == null)
                {
                    Debug.LogError("Could not spawn object at path " + path);
                    return null;
                }

                pathPrefabLookup[path] = obj;
            }
            else
            {
                obj = pathPrefabLookup[path];
            }

            var go = Instantiate(obj) as GameObject;
            return go;
        }
        #endregion

        public void releaseObject(GameObject clone)
        {
            clone.SetActive(false);

            if (instanceLookup.ContainsKey(clone))
            {
                instanceLookup[clone].ReleaseItem(clone);
                instanceLookup.Remove(clone);
                dirty = true;
            }
            else
            {
                Debug.LogWarning("No pool contains the object: " + clone.name);
            }
        }

        public void PrintStatus()
        {
            foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in prefabLookup)
            {
                Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name, keyVal.Value.CountUsedItems, keyVal.Value.Count));
            }
        }

        #region Static API

        public static void WarmPool(GameObject prefab, int size)
        {
            Instance.warmPool(prefab, size);
        }

        public static GameObject SpawnObject(GameObject prefab)
        {
            return Instance.spawnObject(prefab);
        }

        public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Instance.spawnObject(prefab, position, rotation);
        }

        public static void WarmPool(string path, int size)
        {
            Instance.warmPool(path, size);
        }

        public static GameObject SpawnObject(string path)
        {
            return Instance.spawnObject(path);
        }

        public static GameObject SpawnObject(string path, Vector3 position, Quaternion rotation)
        {
            return Instance.spawnObject(path, position, rotation);
        }

        public static void ReleaseObject(GameObject clone)
        {
            Instance.releaseObject(clone);
        }

        #endregion
    }
}