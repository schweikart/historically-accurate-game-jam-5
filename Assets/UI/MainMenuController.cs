using UnityEngine;
using UnityEngine.UIElements;

namespace Arminius
{
    public class MainMenuController : MonoBehaviour
    {
        public GameController gameController;

        private bool _backgroundHistoryShown;

        private bool BackgroundHistoryShown
        {
            get => _backgroundHistoryShown;
            set
            {
                _backgroundHistoryShown = value;
                if (_backgroundHistoryShown)
                {
                    _backgroundHistoryPanel.style.opacity = 1;
                }
                else
                {
                    _backgroundHistoryPanel.style.opacity = 0;
                }
            }
        }

        private VisualElement _backgroundHistoryPanel;

        void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            _backgroundHistoryPanel = root.Q<VisualElement>("BackgroundTextContainer");
            BackgroundHistoryShown = false;

            root.Q<Button>("PlayButton").RegisterCallback<ClickEvent>(OnPlayButtonClick);
            root.Q<Button>("BackgroundButton").RegisterCallback<ClickEvent>(evt => BackgroundHistoryShown = !BackgroundHistoryShown);
            root.Q<Button>("ExitButton").RegisterCallback<ClickEvent>(OnExitButtonClick);
            root.Q<Button>("LearnMoreButton").RegisterCallback<ClickEvent>(evt => Application.OpenURL("https://en.wikipedia.org/wiki/Battle_of_the_Teutoburg_Forest"));
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
