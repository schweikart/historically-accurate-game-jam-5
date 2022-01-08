using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public float moveSpeed = 6;

    Rigidbody rigidBody;
    Camera viewCamera;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velocity = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal")).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }
}
