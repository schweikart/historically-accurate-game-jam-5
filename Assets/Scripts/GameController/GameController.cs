using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public event Action onStartRomanMove;

    public void Awake()
    {
        //FindObjectOfType<Button>().onClick.AddListener(delegate { StartRomanMove(); });
    }

    public void StartRomanMove()
    {
        onStartRomanMove?.Invoke();
    }

    public void GermanDetected()
    {
        Debug.Log("loose");
    }
}
