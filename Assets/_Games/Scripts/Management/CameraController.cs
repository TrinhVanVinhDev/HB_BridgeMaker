using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0, 10, -10);
    private float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        CameraFollowPlayer();
    }

    private Vector3 CameraFollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * speed);
        return transform.position;
    }
}
