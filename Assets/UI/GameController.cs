using UnityEngine;
using System.Collections;

namespace Arminius
{
    public class GameController : MonoBehaviour
    {
        public MainMenuController mainMenuController;
        public GameMenuController gameMenuController;

        void OnEnable()
        {
            OpenMainMenu();
        }

        public void StartGame()
        {
            mainMenuController.gameObject.SetActive(false);
            gameMenuController.gameObject.SetActive(true);
        }

        public void OpenMainMenu()
        {
            mainMenuController.gameObject.SetActive(true);
            gameMenuController.gameObject.SetActive(false);
        }
    }
}
