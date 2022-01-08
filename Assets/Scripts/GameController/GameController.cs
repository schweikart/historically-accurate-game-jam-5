using Arminius;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameLogic { 
    public class GameController : MonoBehaviour
    {
        public event Action onStartRomanMove;

        private GameMenuController controller;

        public void StartRomanMove()
        {
            onStartRomanMove?.Invoke();
        }

        public void GermanDetected()
        {
            Debug.Log("loose");
        }

        public void Update()
        {
            if (GlobalRomanManager.Instance.Romans.Count == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}

