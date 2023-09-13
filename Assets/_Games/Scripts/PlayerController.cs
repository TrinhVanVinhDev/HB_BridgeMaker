using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bricksCollection;
    [SerializeField] private Transform platformTranform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Joystick joystick;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float movingSpeed;

    private List<GameObject> listBrick = new List<GameObject>();
    private int oldChild;


    public static PlayerController Instaise;

    private void Awake()
    {
        Instaise = this;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 rayCastPoint = new Vector3(player.transform.position.x, platformTranform.position.y, player.transform.position.z);
        Physics.Raycast(rayCastPoint, transform.TransformDirection(new Vector3(joystick.Direction.x, 0f, joystick.Direction.y)), out hit, 1f, layerMask);
        if(hit.collider != null)
        {
            if (hit.collider.CompareTag("blue"))
            {
                oldChild = bricksCollection.transform.childCount;
                hit.collider.gameObject.transform.SetParent(bricksCollection.transform);
                if(oldChild != bricksCollection.transform.childCount)
                {
                    listBrick.Add(hit.collider.gameObject);
                    listBrick[listBrick.Count].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[listBrick.Count-1].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                    //for (int i = 0; i < listBrick.Count; i++)
                    //{
                    //    listBrick[i].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[i].transform.position.y + 0.2f, bricksCollection.transform.position.z);
                    //}
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
