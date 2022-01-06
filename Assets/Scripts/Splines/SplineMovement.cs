using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMovement : MonoBehaviour
{

    public event Action onFinished;

    public float delay;

    public float speed;

    public BezierSpline spline;

    public bool lookForward;

    public SplineWalkerMode mode;

    private float progress;
    private bool goingForward = true;
    private float splineLength;

    public void Awake()
    {
        splineLength = spline.GetSplineLength();
    }

    private float time;

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > delay)
        {
            if (goingForward)
            {
                progress += Time.deltaTime * speed / splineLength;
                if (progress > 1f)
                {
                    if (mode == SplineWalkerMode.Once)
                    {
                        progress = 1f;
                        if (onFinished != null)
                        {
                            onFinished();
                        }
                    }
                    else if (mode == SplineWalkerMode.Loop)
                    {
                        progress -= 1f;
                    }
                    else
                    {
                        progress = 2f - progress;
                        goingForward = false;
                    }
                }
            }
            else
            {
                progress -= Time.deltaTime  * speed / splineLength;
                if (progress < 0f)
                {
                    progress = -progress;
                    goingForward = true;
                }
            }

            Vector3 position = spline.GetPoint(progress);
            transform.localPosition = position;
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress));
            }
        }
    }
}
