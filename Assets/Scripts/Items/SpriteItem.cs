using UnityEngine;

namespace Match3
{
    public abstract class SpriteItem : GridItem
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        private int _initialSortingOrder;

        protected void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public override void Initialize()
        {
            base.Initialize();
            _initialSortingOrder = _spriteRenderer.sortingOrder;
        }

        public override void ResetItem()
        {
            base.ResetItem();
            ResetLayer();
        }

        public override void ChangeColor(Color color)
        {
            _spriteRenderer.color = color;
        }


        private void ResetLayer()
        {
            _spriteRenderer.sortingOrder = _initialSortingOrder;
        }
    }
}