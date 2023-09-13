using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnObject spawnObject;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
        spawnObject = SpawnObject.Instaise;
        PlayerController.Instaise.OnInit();


    }

    // Update is called once per frame
    void Update()
    {
        spawnObject.OnSpawnObject("cube", Quaternion.identity);
    }

    public void OnInit() { }
}
