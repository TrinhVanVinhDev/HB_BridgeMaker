using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bricksCollection;
    [SerializeField] private GameObject bricksZonePool;
    [SerializeField] private Transform platformTranform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Joystick joystick;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Material materialBlue;
    [SerializeField] private float movingSpeed;

    private List<GameObject> listBrick = new List<GameObject>();
    private int oldChild;
    private int listCount;


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
        Physics.Raycast(rayCastPoint, transform.TransformDirection(new Vector3(joystick.Direction.x, 0f, joystick.Direction.y)), out hit, 1f, layerMask);
        if(hit.collider != null)
        {
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
                if (listCount != listBrick.Count && !hit.collider.CompareTag("Blue"))
                {
                    if (listBrick.Count > 0 && bricksCollection.transform.childCount > 0)
                    {
                        listCount = listBrick.Count;
                        listBrick[listBrick.Count - 1].gameObject.SetActive(false);
                        listBrick[listBrick.Count - 1].gameObject.transform.SetParent(bricksZonePool.transform);
                        listBrick.RemoveAt(listBrick.Count - 1);
                        hit.collider.gameObject.tag = "Blue";
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material = materialBlue;
                        //if(bricksCollection.transform.childCount > listBrick.Count)
                        //{
                        //    bricksCollection.transform.GetChild(bricksCollection.transform.childCount - 1).SetParent(bricksZonePool.transform);
                        //}
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
    }

    private void OnMoving()
    {
        Vector3 movingDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        agent.Move(new Vector3(joystick.Horizontal, transform.position.y, joystick.Vertical) * Time.fixedDeltaTime * agent.speed);
        transform.position = Vector3.Lerp(transform.position, agent.nextPosition, 0.3f);
        agent.SetDestination(transform.position);
        transform.rotation = Quaternion.LookRotation(movingDirection, Vector3.up);
    }
}
