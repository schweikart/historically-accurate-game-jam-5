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

        private bool attacking = false;

        public void Awake()
        {
            FindObjectOfType<StartAttackManager>().attackStarts += delegate { attacking = true; };
        }

        public void GermanDetected()
        {
            if (!attacking)
            FindObjectOfType<GameMenuController>().OnDefeat();
            StartCoroutine(WaitS());
        }

        public IEnumerator WaitS()
        {
            yield return new WaitForSeconds(3);
            FindObjectOfType<GameMenuController>().ResetGermans();
            FindObjectOfType<GameMenuController>().ResetRomans();
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

