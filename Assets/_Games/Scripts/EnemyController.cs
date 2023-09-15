using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject bricksCollection;
    [SerializeField] private GameObject bricksZonePool;
    [SerializeField] private Transform platformTranform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Material materialBlue;
    [SerializeField] private float movingSpeed;

    private float minDistane = 100f;
    private Vector3 positionMoving;
    private int indexList;
    private bool blocked = true;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SpawnObject.listObjRed.Count; i++)
        {
            float distane = Vector3.Distance(transform.position, SpawnObject.listObjRed[i].transform.position);
            if(distane < minDistane)
            {
                minDistane = distane;
                indexList = i;
                positionMoving = SpawnObject.listObjRed[i].transform.position;
            }
        }


        if(Vector3.Distance(transform.position, positionMoving) < 1)
        {
            SpawnObject.listObjRed[indexList].transform.SetParent(bricksCollection.transform);
            SpawnObject.listObjRed.RemoveAt(indexList);
        }

        //NavMeshHit hit;
        //Vector3 rayCastPoint = new Vector3(enemy.transform.position.x, enemy.transform.position.y - 1f, enemy.transform.position.z);
        //blocked = NavMesh.Raycast(rayCastPoint, rayCastPoint + new Vector3(1,1,1), out hit, layerMask);
        //Debug.DrawRay(rayCastPoint, rayCastPoint + new Vector3(1, 1, 1))
        //if(blocked)
        //{

        //}
        RaycastHit hit;
        Vector3 rayCastPoint = new Vector3(enemy.transform.position.x, enemy.transform.position.y - 1f, enemy.transform.position.z);
        Physics.Raycast(rayCastPoint, transform.TransformDirection(positionMoving), out hit, 1f, layerMask);
        Debug.DrawRay(rayCastPoint, transform.TransformDirection( positionMoving), Color.green);
        //if (hit.collider != null)
        //{
        //    oldChild = bricksCollection.transform.childCount;
        //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Red"))
        //    {
        //        hit.collider.gameObject.transform.SetParent(bricksCollection.transform);
        //        if (oldChild != bricksCollection.transform.childCount)
        //        {
        //            listBrick.Add(hit.collider.gameObject);

        //            if (listBrick.Count == 1)
        //            {
        //                listBrick[0].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[0].transform.position.y + 0.2f, bricksCollection.transform.position.z);
        //            }
        //            else if (listBrick.Count > 1)
        //            {
        //                listBrick[listBrick.Count - 1].transform.position = new Vector3(bricksCollection.transform.position.x, listBrick[listBrick.Count - 2].transform.position.y + 0.2f, bricksCollection.transform.position.z);
        //            }
        //        }
        //    }

        //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bridge"))
        //    {
        //        if (listCount != listBrick.Count && !hit.collider.CompareTag("Blue"))
        //        {
        //            if (listBrick.Count > 0 && bricksCollection.transform.childCount > 0)
        //            {
        //                listCount = listBrick.Count;
        //                listBrick[listBrick.Count - 1].gameObject.SetActive(false);
        //                listBrick[listBrick.Count - 1].gameObject.transform.SetParent(bricksZonePool.transform);
        //                listBrick.RemoveAt(listBrick.Count - 1);
        //                hit.collider.gameObject.tag = "Red";
        //                hit.collider.gameObject.GetComponent<MeshRenderer>().material = materialBlue;
        //            }
        //        }
        //    }
        //}

        OnMoving(positionMoving);
    }
    private void OnMoving(Vector3 movingDirection)
    {
        //transform.position = Vector3.Lerp(transform.position, agent.nextPosition, 0.3f);
        agent.SetDestination(positionMoving);
        //agent.Move(movingDirection * Time.fixedDeltaTime * agent.speed);
        //transform.rotation = Quaternion.LookRotation(movingDirection, Vector3.up);
    }
}
