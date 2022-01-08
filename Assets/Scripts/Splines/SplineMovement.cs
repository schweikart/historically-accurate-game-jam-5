using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMovement : MonoBehaviour
{
    public float offset;

    public event Action onFinished;

    public float delay;

    public float speed;

    public BezierSpline spline;

    public bool lookForward;

    public float stopAt = 1;

    public SplineWalkerMode mode;

    private float progress;
    public float Progress { get { return progress; } }
    private bool goingForward = true;
    private float splineLength;
    public float SplineLength { get { return splineLength; } }

    public void Awake()
    {
        splineLength = spline.GetSplineLength();
    }

    public bool stopped = false;
    public void Stop()
    {
        stopped = true;
    }

    public void Resume()
    {
        stopped = false;
    }

    private float time;

    private void FixedUpdate()
    {
        if (!stopped)
        {
            time += Time.deltaTime;
            if (time > delay)
            {
                if (goingForward)
                {
                    progress += Time.deltaTime * speed / splineLength;
                    if (progress > stopAt)
                    {
                        if (mode == SplineWalkerMode.Once)
                        {
                            progress = stopAt;
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
                    progress -= Time.deltaTime * speed / splineLength;
                    if (progress < 0f)
                    {
                        progress = -progress;
                        goingForward = true;
                    }
                }

                MoveTo(progress);
            }
        }
    }

    public void MoveTo(float amount)
    {
        Vector3 position = spline.GetPoint(amount);
        Vector3 direction = spline.GetDirection(amount);
        if (offset == 0)
        {
            transform.localPosition = position;
        }
        else
        {
            float degrees = Mathf.Atan2(direction.z, direction.x);
            degrees = degrees - (float)1 / 2 * Mathf.PI;

            transform.localPosition = new Vector3(position.x + ((float)Mathf.Cos(degrees) * offset), position.y, position.z + (float)(Mathf.Sin(degrees) * offset));
        }

        if (lookForward)
        {
            transform.LookAt(position + direction);
        }
    }
}
