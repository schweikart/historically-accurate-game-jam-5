using System;
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

    public GameObject DragGhost;

    void Start()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        _playButton = rootVisualElement.Q<Button>("PlayButton");
        _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);

        _timeSlider = rootVisualElement.Q<Slider>("TimeSlider");
        _timeSlider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChanged);

        _germanSelectorScrollView = rootVisualElement.Q<ScrollView>("GermanSelector");
        foreach (var germanSelector in _germanSelectorScrollView.contentContainer.Children())
        {
            Debug.Log(germanSelector.name);
            germanSelector.RegisterCallback<MouseDownEvent>(evt => OnGermanSelectorDragStart(evt, germanSelector), TrickleDown.TrickleDown);
            germanSelector.RegisterCallback<MouseUpEvent>(evt => OnGermanSelectorDragStop(evt, germanSelector));
            germanSelector.RegisterCallback<MouseMoveEvent>(evt => OnGermanSelectorDragMove(evt, germanSelector));
        }
    }

    private void OnGermanSelectorDragStart(MouseDownEvent evt, VisualElement germanSelector)
    {
        _currentGermanSelectorDrag = germanSelector;
        DragGhost.SetActive(true);
        Debug.Log("Dragging start: " + germanSelector.name);
    }

    private void OnGermanSelectorDragStop(MouseUpEvent evt, VisualElement germanSelector)
    {
        _currentGermanSelectorDrag = null;
        Instantiate(DragGhost);
        DragGhost.SetActive(false);
        Debug.Log("Dragging stop: " + germanSelector.name);
    }

    private void OnGermanSelectorDragMove(MouseMoveEvent evt, VisualElement germanSelector)
    {
        if (_currentGermanSelectorDrag != null)
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit);
            DragGhost.transform.position = hit.point;
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
