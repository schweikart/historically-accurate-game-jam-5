using UnityEngine;
using UnityEngine.UIElements;

namespace Arminius
{
    public class GermaneCardElement : VisualElement
    {

        public new class UxmlFactory : UxmlFactory<GermaneCardElement, UxmlTraits> {}

        public new class UxmlTraits : VisualElement.UxmlTraits { } //TODO implement germane data as uxml attribute

        private GermaneStockEntry _germane;

        public GermaneStockEntry StockEntry
        {
            get => _germane;
            set {
                _germane = value;
                UpdateUi();
            }
        }

        private readonly Image _image;
        private readonly TextElement _text;

        public GermaneCardElement()
        {
            _image = new Image();
            _image.style.width = 128;
            _image.style.height = 128;
            _image.style.marginLeft = 5;
            _image.style.marginRight = 5;
            Add(_image);

            _text = new TextElement {text = "unknown123"};
            Add(_text);
        }

        public void UpdateUi()
        {
            _image.sprite = StockEntry.Germane.CardSprite;
            _text.text = $"{StockEntry.Germane.TypeName} ({StockEntry.AmountInStock})";
        }
    }
}
