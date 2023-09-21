using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour, IPooledObject
{
    private List<Vector3> listPosition = new List<Vector3>();
    public void OnObjectSpawn()
    {
        float xBrick = Random.Range(1, 20);
        float zBrick = Random.Range(1, 20);

        Vector3 positionBrick = new Vector3(xBrick, 0f, zBrick);
        if (!listPosition.Contains(positionBrick) && listPosition.Count < 10)
        {
            listPosition.Add(positionBrick);
        }
    }
}
