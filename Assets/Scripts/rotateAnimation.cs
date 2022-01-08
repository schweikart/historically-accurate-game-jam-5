using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAnimation : MonoBehaviour
{

    public float moveAngle;

    [Range(0,100)]
    public int stepsize;

    int actualStepSize;

    public float degreesToGo;

    public bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        actualStepSize = (moveAngle < 0) ? -stepsize : stepsize;
        degreesToGo = Mathf.Abs(moveAngle);
    }

    // Update is called once per frame
    void Update()
    {

        if (started && degreesToGo > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + actualStepSize, transform.rotation.eulerAngles.z);
            degreesToGo -= stepsize;
        }
            
    }


    public void start()
    {
        started = true;
    }
}
