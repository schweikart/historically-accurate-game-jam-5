using System;
using Arminius;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIController : MonoBehaviour
{
    public float Speed = 10;

    private bool _playing = false;
    public bool Playing
    {
        get => _playing;
        private set
        {
            _playing = value;
            UpdatePlayButton();
        }
    }

    private Button _playButton;
    private Slider _timeSlider;
    private ScrollView _germanSelectorScrollView;

    private VisualElement _currentGermanSelectorDrag = null;

    private GameObject _dragGhost;
    private int _dragGhostOriginalLayer = -1;
    public LevelData Level;

    void Start()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        _playButton = rootVisualElement.Q<Button>("PlayButton");
        _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);

        _timeSlider = rootVisualElement.Q<Slider>("TimeSlider");
        _timeSlider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChanged);

        _germanSelectorScrollView = rootVisualElement.Q<ScrollView>("GermanSelector");
        foreach (var germane in Level.Germanes)
        {
            var card = new GermaneCardElement {GermaneData = germane.Germane};
            card.RegisterCallback<MouseDownEvent>(evt => OnGermanSelectorDragStart(evt, card));
            card.RegisterCallback<MouseUpEvent>(evt => OnGermanSelectorDragStop(evt, card));
            card.RegisterCallback<MouseMoveEvent>(evt => OnGermanSelectorDragMove(evt, card));
            _germanSelectorScrollView.Add(card);
        }
    }

    private void OnGermanSelectorDragStart(MouseDownEvent evt, GermaneCardElement germanSelector)
    {
        evt.target.CaptureMouse();
        _currentGermanSelectorDrag = germanSelector;
        _dragGhost = Instantiate(germanSelector.GermaneData.FigurePrefab);

        // disable raycasts on ghost
        _dragGhostOriginalLayer = _dragGhost.layer;
        _dragGhost.layer = 2;
    }

    private void OnGermanSelectorDragStop(MouseUpEvent evt, GermaneCardElement germanSelector)
    {
        _currentGermanSelectorDrag = null;
        _dragGhost.layer = _dragGhostOriginalLayer;

        _dragGhost = null;
        _dragGhostOriginalLayer = -1;

        evt.target.ReleaseMouse();
    }

    private void OnGermanSelectorDragMove(MouseMoveEvent evt, GermaneCardElement germanSelector)
    {
        if (_currentGermanSelectorDrag != null)
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit);
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
            }

            _timeSlider.value = newTime;
        }
    }

    private void UpdatePlayButton()
    {
        _playButton.text = (Playing ? "Pause" : "Play") + " (" + _timeSlider.value + ")";
    }

    private void OnPlayButtonClick(ClickEvent evt)
    {
        if (!Playing && Math.Abs(_timeSlider.value - _timeSlider.highValue) < 0.1)
        {
            _timeSlider.value = _timeSlider.lowValue;
        }

        Playing = !Playing;
    }

    private void OnSliderValueChanged(ChangeEvent<float> evt)
    {
        UpdatePlayButton();
    }
}
