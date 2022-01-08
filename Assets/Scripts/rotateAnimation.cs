using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAnimation : MonoBehaviour
{

    public float startAngle;

    public float endAngle;

    public float speed;

    public bool started = false;

    Rigidbody rigidBody;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.MoveRotation(Quaternion.Euler(new Vector3(0, startAngle, 0)));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var movement = startAngle < endAngle ? speed : -speed;
        var eulerAngleVelocity = new Vector3(0, movement, 0);
        if (started && (Mathf.Abs(startAngle - endAngle) > (Mathf.Abs(startAngle + movement - endAngle))))
        {
            Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("F")) start();
    }


    public void start()
    {
        started = true;
    }
}
