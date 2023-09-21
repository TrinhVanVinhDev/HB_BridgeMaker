using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnObject spawnObject;

    private List<Vector3> listPosition = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        spawnObject = SpawnObject.Instaise;
        spawnObject.OnInit();
        
        PlayerController.Instaise.OnInit();
        EnemyController.Instaise.OnInit();
        for (int i = 0; i < 10; i++)
        {
            OnInit();
        }
    }

    void OnInit()
    {
        spawnObject.OnSpawnObject("blue");
        spawnObject.OnSpawnObject("red");
    }
}
