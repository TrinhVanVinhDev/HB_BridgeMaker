using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0, 10, -10);

    // Update is called once per frame
    void Update()
    {
        CameraFollowPlayer();
    }

    private Vector3 CameraFollowPlayer()
    {
        transform.position = player.transform.position + offset;
        return transform.position;
    }
}
