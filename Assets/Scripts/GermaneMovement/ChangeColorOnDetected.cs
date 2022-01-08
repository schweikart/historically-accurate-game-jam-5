using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnDetected : MonoBehaviour, IDetected
{
    public Material detectedMaterial;
    public Material notDetectedMaterial;

    bool detected = false;
    public void Detected()
    {
        detected = true;
    }

    public void Update()
    {
        if (detected == true)
        {
            detected = false;
            GetComponent<MeshRenderer>().material = detectedMaterial;
        } else
        {
            GetComponent<MeshRenderer>().material = notDetectedMaterial;
        }
    }

}
