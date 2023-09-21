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

    public static List<GameObject> listObjRed = new List<GameObject>();

    protected Dictionary<string, Queue<GameObject>> dictionaryPool;

    public void OnInit()
    {
        dictionaryPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefabs);
                obj.transform.SetParent(pool.parentObject.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);

                if (pool.tag == "red")
                {
                    listObjRed.Add(obj);
                }
            }

            dictionaryPool.Add(pool.tag, objectPool);
        }
    }

    public GameObject OnSpawnObject(string tag, float yPosition = -2f)
    {
        if(!dictionaryPool.ContainsKey(tag))
        {
            Debug.LogWarning("is tag: " + tag + " not exist");
            return null;
        }

        GameObject objectToSpawn = dictionaryPool[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = CreateRandomPosition(yPosition);
        objectToSpawn.transform.rotation = Quaternion.identity;


        dictionaryPool[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public static void RemoveItem(int index)
    {
        listObjRed.RemoveAt(index);
    }
    private Vector3 CreateRandomPosition(float yPosition)
    {
        float xPosition;
        float zPosition;
        if (yPosition == -2f)
        {
            xPosition = Random.Range(1, 20);
            zPosition = Random.Range(1, 20);

        }
        else
        {
            xPosition = Random.Range(1, 20);
            zPosition = Random.Range(60, 80);
        }

        Vector3 position = new Vector3(xPosition, yPosition, zPosition);
        return position;
    }
}
