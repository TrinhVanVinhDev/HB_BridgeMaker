using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public int size;
        public GameObject prefabs;
        public GameObject parentObject;
    }

    public static SpawnObject Instaise;

    private void Awake()
    {
        Instaise = this;
    }

    [SerializeField] private List<Pool> pools;
    protected Dictionary<string, Queue<GameObject>> dictionaryPool;

    private void Start()
    {
        dictionaryPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                float randomPosx = Random.Range(-4, 4);
                float randomPosz = Random.Range(-4, 4);
                GameObject obj = Instantiate(pool.prefabs, new Vector3(randomPosx, -2f, randomPosz), Quaternion.identity);
                obj.transform.SetParent(pool.parentObject.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            dictionaryPool.Add(pool.tag, objectPool);
        }
    }

    public GameObject OnSpawnObject(string tag, Quaternion rotation)
    {
        if(!dictionaryPool.ContainsKey(tag))
        {
            Debug.LogWarning("is tag: " + tag + " not exist");
            return null;
        }

        GameObject objectToSpawn = dictionaryPool[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.rotation = rotation;
        dictionaryPool[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}