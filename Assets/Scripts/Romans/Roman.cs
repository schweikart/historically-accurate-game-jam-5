using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roman : MonoBehaviour
{

    public void Awake()
    {
        GlobalRomanManager.Instance.Romans.Add(this);
    }

    public void OnDestroy()
    {
        GlobalRomanManager.Instance.Romans.Remove(this);
    }
}
