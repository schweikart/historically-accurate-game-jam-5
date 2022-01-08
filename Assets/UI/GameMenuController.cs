using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Arminius
{
    public struct GermaneStockEntry
    {
        public GermaneData Germane;
        public int AmountInStock;
    }

    public class GameMenuController : MonoBehaviour
    {
        public GameController gameController;

        private bool _editorMode = true;
        public bool EditorMode
        {
            get => _editorMode;
            set
            {
                _editorMode = value;
                Playing = !_editorMode;

                if (_editorMode)
                {
                    HideModal();
                    _timeSlider.value = _timeSlider.lowValue;
                }
                else
                {
                    FindObjectOfType<GameLogic.GameController>().StartRomanMove();
                }
            }
        }

        public float Speed = 10;

        private bool _playing = false;
        public bool Playing
        {
            get => _playing;
            private set
            {
                _playing = value;
                _timeSlider.SetEnabled(!_playing);
                UpdatePlayButton();
            }
        }

        private Button _playButton;
        private Slider _timeSlider;
        private ScrollView _germanSelectorScrollView;
        private Label _modal;

        private VisualElement _currentGermanSelectorDrag = null;

        private GameObject _dragGhost;
        private int _dragGhostOriginalLayer = -1;
        public LevelData Level;

        void OnEnable()
        {
            var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

            _playButton = rootVisualElement.Q<Button>("PlayButton");
            _playButton.RegisterCallback<ClickEvent>(evt => EditorMode = !EditorMode);

            _timeSlider = rootVisualElement.Q<Slider>("TimeSlider");
            _timeSlider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChanged);

            _modal = rootVisualElement.Q<Label>("Modal");

            rootVisualElement.Q<Button>("RestartLevelButton").RegisterCallback<ClickEvent>(OnRestartLevelButtonClick);
            rootVisualElement.Q<Button>("MainMenuButton").RegisterCallback<ClickEvent>(OnMainMenuButtonClick);

            _germanSelectorScrollView = rootVisualElement.Q<ScrollView>("GermanSelector");
            Restock();

            EditorMode = true;
        }

        private void Restock()
        {
            while (_germanSelectorScrollView.childCount != 0)
            {
                _germanSelectorScrollView.RemoveAt(0);
            }

            foreach (var germane in Level.Germanes)
            {
                var stock = new GermaneStockEntry()
                {
                    Germane = germane.Germane,
                    AmountInStock = germane.Amount,
                };
                var card = new GermaneCardElement { StockEntry = stock };
                card.RegisterCallback<MouseDownEvent>(evt => OnGermanSelectorDragStart(evt, card));
                card.RegisterCallback<MouseUpEvent>(evt => OnGermanSelectorDragStop(evt, card));
                card.RegisterCallback<MouseMoveEvent>(evt => OnGermanSelectorDragMove(evt, card));
                _germanSelectorScrollView.Add(card);
            }
        }

        private void OnRestartLevelButtonClick(ClickEvent evt)
        {
            Restock();

            foreach (ChangeColorOnDetected o in FindObjectsOfType<ChangeColorOnDetected>())
            {
                Destroy(o.gameObject);
            }
        }

        private void OnMainMenuButtonClick(ClickEvent evt)
        {
            //gameController.OpenMainMenu();
            OnVictory();
        }

        private void OnGermanSelectorDragStart(MouseDownEvent evt, GermaneCardElement germaneCard)
        {
            if (EditorMode)
            {
                // only create new germane the stock is not empty
                if (germaneCard.StockEntry.AmountInStock == 0) return;

                evt.target.CaptureMouse();
                _currentGermanSelectorDrag = germaneCard;
                _dragGhost = Instantiate(germaneCard.StockEntry.Germane.FigurePrefab);
                germaneCard.StockEntry = new GermaneStockEntry()
                {
                    AmountInStock = germaneCard.StockEntry.AmountInStock - 1,
                    Germane = germaneCard.StockEntry.Germane,
                };

                // disable ray casts on ghost
                _dragGhostOriginalLayer = _dragGhost.layer;
                _dragGhost.layer = 2;
            }
        }

        private void OnGermanSelectorDragStop(MouseUpEvent evt, GermaneCardElement germanSelector)
        {
            if (EditorMode)
            {
                _currentGermanSelectorDrag = null;
                _dragGhost.layer = _dragGhostOriginalLayer;

                _dragGhost = null;
                _dragGhostOriginalLayer = -1;

                evt.target.ReleaseMouse();
            }
        }

        private void OnGermanSelectorDragMove(MouseMoveEvent evt, GermaneCardElement germanSelector)
        {
            if (EditorMode && _currentGermanSelectorDrag != null)
            {
                Ray ray = gameController.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("ground"));
                _dragGhost.transform.position = hit.point;
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            }
        }

        private void Update()
        {
            if (Playing)
            {
                var newTime = _timeSlider.value + Speed * Time.deltaTime;
                if (newTime >= _timeSlider.highValue)
                {
                    newTime = _timeSlider.highValue;
                    Playing = false;
                    // at this point, the fight is probably still running
                }

                _timeSlider.value = newTime;
            }
        }

        private void UpdatePlayButton()
        {
            _playButton.text = Playing ? "Reset" : "Start";
        }


        private void OnPlayButtonClick(ClickEvent evt)
        {
            Playing = !Playing;
            _timeSlider.value = _timeSlider.lowValue;
            if (Playing)
            {
                FindObjectOfType<GameLogic.GameController>().StartRomanMove();
            } else
            {
                StartAttackManager attackmanager = FindObjectOfType<StartAttackManager>();

                foreach (ChangeColorOnDetected o in FindObjectsOfType<ChangeColorOnDetected>())
                {
                    o.GetComponent<StateMachine>().ChangeState(new WaitToAttackState(attackmanager));
                    o.GetComponent<LooseIfDetected>().active = false;
                    o.GetComponent<NavMeshAgent>().isStopped = true;
                }
                ResetRomans();
            }
        }

        public Material defaultROman;

        private void ResetRomans()
        {
            foreach(Roman roman in FindObjectsOfType<Roman>())
            {
                roman.GetComponent<Renderer>().material = defaultROman;
            }
        }


        private void OnSliderValueChanged(ChangeEvent<float> evt)
        {
            FindObjectOfType<ViewPrediction>().MoveRomans(evt.newValue);
            if (Playing)
            {
                if (evt.newValue > 0.95)
                {
                    FindObjectOfType<StartAttackManager>().StartAttack();
                }
            }
        }

        public void OnVictory()
        {
            ShowModal("Victory!");
        }

        public void OnDefeat()
        {
            ShowModal("Defeat");
        }

        private void ShowModal(string text)
        {
            _modal.text = text;
            _modal.RemoveFromClassList("hidden");
        }

        private void HideModal()
        {
            _modal.AddToClassList("hidden");
        }
    }

}