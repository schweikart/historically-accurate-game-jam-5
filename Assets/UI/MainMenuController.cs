using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

namespace Arminius
{
    public class MainMenuController : MonoBehaviour
    {
        private bool _backgroundHistoryShown;

        public rotateAnimation Germane;
        public rotateAnimation Roman;

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

            root.Q<Button>("PlayButton").RegisterCallback<ClickEvent>(evt => StartCoroutine("PlaySequence"));
            root.Q<Button>("BackgroundButton").RegisterCallback<ClickEvent>(evt => BackgroundHistoryShown = !BackgroundHistoryShown);
            root.Q<Button>("ExitButton").RegisterCallback<ClickEvent>(OnExitButtonClick);
            root.Q<Button>("LearnMoreButton").RegisterCallback<ClickEvent>(evt => Application.OpenURL("https://en.wikipedia.org/wiki/Battle_of_the_Teutoburg_Forest"));
        }

        private IEnumerator PlaySequence()
        {
            Germane.start();
            Roman.start();
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Level1");
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
