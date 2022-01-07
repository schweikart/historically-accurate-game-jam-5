using System;
using Arminius;
using UnityEngine;
using UnityEngine.UIElements;

public struct GermaneStockEntry
{
    public GermaneData Germane;
    public int AmountInStock;
}

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
            _timeSlider.SetEnabled(!_playing);
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
            var card = new GermaneCardElement {StockEntry = stock};
            card.RegisterCallback<MouseDownEvent>(evt => OnGermanSelectorDragStart(evt, card));
            card.RegisterCallback<MouseUpEvent>(evt => OnGermanSelectorDragStop(evt, card));
            card.RegisterCallback<MouseMoveEvent>(evt => OnGermanSelectorDragMove(evt, card));
            _germanSelectorScrollView.Add(card);
        }
    }

    private void OnGermanSelectorDragStart(MouseDownEvent evt, GermaneCardElement germaneCard)
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
        _playButton.text = Playing ? "Reset" : "Start";
    }

    private void OnPlayButtonClick(ClickEvent evt)
    {
        Playing = !Playing;
        _timeSlider.value = _timeSlider.lowValue;
    }

    private void OnSliderValueChanged(ChangeEvent<float> evt)
    {
        // set time
    }
}
