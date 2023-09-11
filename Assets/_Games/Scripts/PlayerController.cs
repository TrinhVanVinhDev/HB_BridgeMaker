using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float movingSpeed;

    private bool moving;
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(joystick.Horizontal * movingSpeed, rb.velocity.y, joystick.Vertical * movingSpeed);
    }
}
