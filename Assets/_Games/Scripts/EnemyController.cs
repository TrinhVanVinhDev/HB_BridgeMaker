using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public static EnemyController Instaise;

    private void Awake()
    {
        Instaise = this;
    }

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject bricksCollection;
    [SerializeField] private GameObject bricksZonePool;
    [SerializeField] private Transform platformTranformLv1;
    [SerializeField] private Transform platformTranformLv2;
    [SerializeField] private Transform victoryZone;
    [SerializeField] private Transform bridgesToVictory;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private List<Transform> bridges;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Material materialRed;
    [SerializeField] private float movingSpeed;

    private float minDistane;
    private Vector3 positionMoving;
    private Vector3 movingDirection;
    private Vector3 bridgeTaget;
    private Transform platformTranform;
    private Transform platformTranformNext;
    private int listCount = 0;
    private int oldChild;
    private bool movingInPlatform = true;

    private List<GameObject> listBrick = new List<GameObject>();

    private SpawnObject spawnObject;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 pointEnemy = enemy.transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(agent.velocity.x, 0f, agent.velocity.z));
        Physics.Raycast(pointEnemy, transform.TransformDirection(Vector3.forward), out hit, 1f, layerMask);

        Debug.DrawRay(pointEnemy, transform.TransformDirection(Vector3.forward), Color.green);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                platformTranform = platformTranformLv2;
                platformTranformNext = victoryZone;
                bridgeTaget = bridgesToVictory.position;
                movingInPlatform = true;
                spawnObject.OnSpawnObject("red", platformTranform.position.y);
                for(int i = 0; i < listBrick.Count; i++)
                {
                    listBrick[i].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[i].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                }
            }
        }
        if (Mathf.Abs(platformTranform.position.y - transform.position.y) < 0.5 && movingInPlatform)
        {
            if (SpawnObject.listObjRed.Count != 0)
            {
                GetPositionMovingToBrick();

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Red"))
                    {
                        oldChild = bricksCollection.transform.childCount;
                        hit.collider.gameObject.transform.SetParent(bricksCollection.transform);
                        if (oldChild != bricksCollection.transform.childCount)
                        {
                            listBrick.Add(hit.collider.gameObject);

                            if (listBrick.Count == 1)
                            {
                                listBrick[0].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[0].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                            }
                            else if (listBrick.Count > 1)
                            {
                                listBrick[listBrick.Count - 1].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[listBrick.Count - 2].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                            }
                            SpawnObject.listObjRed.Remove(hit.collider.gameObject);
                        }
                    }
                }
            } else
            {
                GetPositionMovingToBridge();
                if(hit.collider != null)
                {
                    if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Bridge"))
                    {
                        positionMoving = new Vector3(enemy.transform.position.x, platformTranformNext.position.y, platformTranformNext.position.z);
                        movingInPlatform = false;
                    }
                }
            }
        } else
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bridge") && listBrick.Count != 0)
                {
                    if (listCount != listBrick.Count && !hit.collider.CompareTag("Red"))
                    {
                        listCount = listBrick.Count;
                        SpawnObject.listObjRed.Add(listBrick[listBrick.Count - 1].gameObject);
                        listBrick[listBrick.Count - 1].gameObject.transform.SetParent(bricksZonePool.transform);
                        listBrick[listBrick.Count - 1].gameObject.transform.position = CreateRandomPosition();
                        listBrick[listBrick.Count - 1].gameObject.transform.rotation = Quaternion.identity;
                        listBrick.RemoveAt(listBrick.Count - 1);
                        hit.collider.gameObject.tag = "Red";
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material = materialRed;
                    }
                } else
                {
                    positionMoving = new Vector3(enemy.transform.position.x, platformTranform.position.y, platformTranform.position.z);
                    movingInPlatform = true;
                }

            }
        }
        OnMoving(positionMoving, movingDirection);
    }
    private void OnMoving(Vector3 positionMoving, Vector3 movingDirection)
    {
        agent.SetDestination(positionMoving);
        agent.Move(movingDirection * Time.deltaTime * agent.speed);
    }

    private Vector3 GetPositionMovingToBrick()
    {
        minDistane = 100f;
        listCount = SpawnObject.listObjRed.Count;
        for (int i = 0; i < listCount; i++)
        {
            float distane = Vector3.Distance(transform.position, SpawnObject.listObjRed[i].transform.position);
            if (distane < minDistane)
            {
                minDistane = distane;
                positionMoving = SpawnObject.listObjRed[i].transform.position;
            }
        }
        return positionMoving;
    }
    private void GetPositionMovingToBridge()
    {
        minDistane = 100f;
        if (bridgeTaget != Vector3.zero)
        {
            positionMoving = bridgeTaget;
        } else
        {
            for (int j = 0; j < bridges.Count; j++)
            {
                float distaneBridge = Vector3.Distance(transform.position, bridges[j].transform.position);
                if (distaneBridge < minDistane)
                {
                    minDistane = distaneBridge;
                    positionMoving = bridges[j].transform.position;
                }
            }
            bridgeTaget = positionMoving;
        }
    }

    public void OnInit()
    {
        platformTranform = platformTranformLv1;
        platformTranformNext = platformTranformLv2;
        spawnObject = SpawnObject.Instaise;
    }
    private Vector3 CreateRandomPosition()
    {
        float xPosition;
        float yPosition;
        float zPosition;
        if (platformTranform == platformTranformLv2)
        {
            xPosition = Random.Range(-20, 20);
            zPosition = Random.Range(60, 90);
            yPosition = 8.5f;

        } else
        {
            xPosition = Random.Range(-20, 20);
            zPosition = Random.Range(0, 25);
            yPosition = -2f;
        }

        Vector3 position = new Vector3(xPosition, yPosition, zPosition);
        return position;
    }
}
