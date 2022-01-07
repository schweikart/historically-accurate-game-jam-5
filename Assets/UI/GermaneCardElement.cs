using UnityEngine;
using UnityEngine.UIElements;

namespace Arminius
{
    public class GermaneCardElement : VisualElement
    {

        public new class UxmlFactory : UxmlFactory<GermaneCardElement, UxmlTraits> {}

        public new class UxmlTraits : VisualElement.UxmlTraits { } //TODO implement germane data as uxml attribute

        private GermaneData _germaneData = null;

        public GermaneData GermaneData
        {
            get => _germaneData;
            set {
                _germaneData = value;
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            _image.sprite = GermaneData.CardSprite;
            _text.text = GermaneData.TypeName;
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
    }
}
