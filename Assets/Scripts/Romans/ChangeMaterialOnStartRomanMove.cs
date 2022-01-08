using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ChangeMaterialOnStartRomanMove : MonoBehaviour
{
    public Material newMaterial;

    public void Awake()
    {
        FindObjectOfType<GameLogic.GameController>().onStartRomanMove += delegate { GetComponent<Renderer>().material = newMaterial;  };
    }
}
