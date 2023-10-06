using UnityEngine;

namespace Match3
{
    public class NormalItem : SpriteItem
    {
        [SerializeField] private ColoredGlowItemConfigureData _configureData;

        public override ItemType ItemType => ItemType.BoardItem;
        public override int DefaultScore => 0;

        public override void ConfigureItem(int configureType)
        {
            SetConfigureType(configureType);
            SetContentData(_configureData.ColoredItemDatas[configureType]);
        }

        public override void ResetItem()
        {
            base.ResetItem();
        }

        public override void Explode()
        {
            base.Explode();
            Kill();
        }

        public override void Kill(bool shouldPlayExplosion = true, bool isSpecialKill = true)
        {
            base.Kill();
        }

        private void SetContentData(ColoredGlowItemData itemContentData)
        {
            SetColorType(itemContentData.colorType);
            SetSprite(itemContentData.Sprite);
        }



      
      
    } 
}