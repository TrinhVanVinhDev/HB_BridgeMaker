using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bricksCollection;
    [SerializeField] private GameObject bricksZonePool;
    [SerializeField] private Transform platformTranformLv1;
    [SerializeField] private Transform platformTranformLv2;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Joystick joystick;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Material materialBlue;
    [SerializeField] private float movingSpeed;

    private List<GameObject> listBrick = new List<GameObject>();
    private int oldChild;
    private int listCount;

    private Transform platformTranform;
    private SpawnObject spawnObject;
    private Vector3 movingDirection;

    public static PlayerController Instaise;

    private void Awake()
    {
        Instaise = this;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 rayCastPoint = player.transform.position;
        Physics.Raycast(rayCastPoint, new Vector3(joystick.Direction.x, 0f, joystick.Direction.z), out hit, 1f, layerMask);

        movingDirection = new Vector3(joystick.Horizontal, transform.position.y, joystick.Vertical);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Door"))
            {

                platformTranform = platformTranformLv2;
                spawnObject.OnSpawnObject("blue", platformTranform.position.y);
                for (int i = 0; i < listBrick.Count; i++)
                {
                    listBrick[i].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[i].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                }
            }
            oldChild = bricksCollection.transform.childCount;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Blue"))
            {
                hit.collider.gameObject.transform.SetParent(bricksCollection.transform);
                if(oldChild != bricksCollection.transform.childCount)
                {
                    listBrick.Add(hit.collider.gameObject);

                    if(listBrick.Count == 1)
                    {
                        listBrick[0].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[0].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                    } else if(listBrick.Count > 1)
                    {
                        listBrick[listBrick.Count - 1].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[listBrick.Count - 2].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                    }
                }
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bridge"))
            {
                if(listBrick.Count == 0 && !hit.collider.CompareTag("Blue"))
                {
                    movingDirection = Vector3.zero;
                }
                if (listCount != listBrick.Count && !hit.collider.CompareTag("Blue"))
                {
                    if (listBrick.Count > 0 && bricksCollection.transform.childCount > 0)
                    {
                        listCount = listBrick.Count;
                        listBrick[listBrick.Count - 1].gameObject.transform.SetParent(bricksZonePool.transform);
                        listBrick[listBrick.Count - 1].gameObject.transform.position = CreateRandomPosition();
                        listBrick[listBrick.Count - 1].gameObject.transform.rotation = Quaternion.identity;
                        listBrick.RemoveAt(listBrick.Count - 1);
                        hit.collider.gameObject.tag = "Blue";
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material = materialBlue;
                    }
                }
            }
        }

        if (joystick.Horizontal != 0)
        {
            OnMoving();
        }
    }

    public void OnInit()
    {
        bricksCollection.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
        platformTranform = platformTranformLv1;
        spawnObject = SpawnObject.Instaise;
    }

    private void OnMoving()
    {
        agent.Move(movingDirection * Time.fixedDeltaTime * agent.speed);
        transform.rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal, 0f, joystick.Vertical), Vector3.up);
    }


    private Vector3 CreateRandomPosition()
    {
        float xPosition;
        float yPosition;
        float zPosition;
        if (platformTranform == platformTranformLv2)
        {
            xPosition = Random.Range(1, 20);
            zPosition = Random.Range(60, 80);
            yPosition = 8.5f;

        }
        else
        {
            xPosition = Random.Range(1, 20);
            zPosition = Random.Range(1, 20);
            yPosition = -2f;
        }

        Vector3 position = new Vector3(xPosition, yPosition, zPosition);
        return position;
    }
}
