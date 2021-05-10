using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTouch : MonoBehaviour
{
    Rigidbody2D rd;
    public Joystick joystick;
    public float speed;

    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical);
        rd.MovePosition(rd.position + dir * speed * Time.deltaTime);
    }
}
