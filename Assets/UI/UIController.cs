using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private bool playing = false;
    private Button playButton;
    private VisualTreeAsset tree;

    void Start()
    {
        playButton.RegisterCallback<MouseDownEvent>(OnPlayButtonClick);
    }

    private void OnPlayButtonClick(MouseDownEvent evt)
    {
        playing = !playing;
        playButton.text = playing ? "Pause" : "Play";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
