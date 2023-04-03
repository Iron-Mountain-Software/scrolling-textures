using UnityEngine;
using UnityEngine.UI;

namespace SpellBoundAR.ScrollingTextures
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RawImage))]
    public class ScrollingRawImage : MonoBehaviour
    {
        private enum FillMethod { None, Stretch, Width, Height }
        private enum TimeMethod { Unscaled, Scaled }
        
        [SerializeField] private Vector2 speed;
        [SerializeField] private FillMethod fillMethod;
        [SerializeField] private float fillMultiplier = 1;
        [SerializeField] private TimeMethod timeMethod = TimeMethod.Unscaled; 

        [Header("Cache")]
        private RawImage _image;
        private RectTransform _rectTransform;

        private RawImage Image
        {
            get
            {
                if (!_image) _image = GetComponent<RawImage>();
                return _image;
            }
        }


        private RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private void Update()
        {
            switch (timeMethod)
            {
                case TimeMethod.Unscaled:
                    ApplyOffset(Time.unscaledTime * speed);
                    break;
                case TimeMethod.Scaled:
                    ApplyOffset(Time.time * speed);
                    break;
            }
        }

        private void ApplyOffset(Vector2 offset)
        {
            switch (fillMethod)
            {
                case FillMethod.None:
                    Image.uvRect = new Rect(offset.x, offset.y, Image.uvRect.width, Image.uvRect.height);
                    break;
                case FillMethod.Stretch:
                    Image.uvRect = new Rect(offset.x, offset.y, fillMultiplier, fillMultiplier);
                    break;
                case FillMethod.Width:
                    Image.uvRect = new Rect(offset.x, offset.y, fillMultiplier,
                        RectTransform.rect.size.y / RectTransform.rect.size.x * fillMultiplier);
                    break;
                case FillMethod.Height:
                    Image.uvRect = new Rect(offset.x, offset.y,
                        RectTransform.rect.size.x / RectTransform.rect.size.y * fillMultiplier,
                        fillMultiplier);
                    break;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            ApplyOffset(Vector2.zero);
        }

#endif
    }
}
