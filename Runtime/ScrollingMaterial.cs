using UnityEngine;

namespace IronMountain.ScrollingTextures
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class ScrollingMaterial : MonoBehaviour
    {
        [SerializeField] private Vector2 tiling = Vector2.one;
        [SerializeField] private Vector2 speed;
        [SerializeField] private string propertyName = "_MainTex_ST";
        [SerializeField] private TimeMethod timeMethod = TimeMethod.Unscaled; 

        [Header("Cache")]
        private Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;

        private Renderer Renderer
        {
            get
            {
                if (!_renderer) _renderer = GetComponent<Renderer>();
                return _renderer;
            }
        }
        
        private MaterialPropertyBlock MaterialPropertyBlock => _propertyBlock ??= new MaterialPropertyBlock();

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
            if (!Renderer || !Renderer.enabled || !gameObject.activeInHierarchy) return;
            MaterialPropertyBlock.SetVector(propertyName, new Vector4(tiling.x, tiling.y, offset.x, offset.y));
            Renderer.SetPropertyBlock(MaterialPropertyBlock);
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            ApplyOffset(Vector2.zero);
        }

#endif
        
    }
}