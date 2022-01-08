using UnityEngine;
using UnityEngine.UIElements;

namespace Arminius
{
    public class MainMenuController : MonoBehaviour
    {
        public GameController gameController;

        void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            root.Q<Button>("PlayButton").RegisterCallback<ClickEvent>(OnPlayButtonClick);
            root.Q<Button>("BackgroundButton").RegisterCallback<ClickEvent>(OnBackgroundButtonClick);
            root.Q<Button>("ExitButton").RegisterCallback<ClickEvent>(OnExitButtonClick);
        }

        private void OnPlayButtonClick(ClickEvent evt)
        {
            gameController.StartGame();
        }

        private void OnBackgroundButtonClick(ClickEvent evt)
        {
            Debug.Log("Historical Background...");
        }

        private void OnExitButtonClick(ClickEvent evt)
        {
# if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
# else
            Application.Quit();
# endif
        }
    }
}
