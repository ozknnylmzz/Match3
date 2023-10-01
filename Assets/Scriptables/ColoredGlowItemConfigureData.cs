using UnityEngine;

namespace CasualA.Board
{
    [CreateAssetMenu(menuName = "Board/ConfigureData/ColoredGlowItemConfigureData")]
    public class ColoredGlowItemConfigureData : ConfigureData
    {
        public override ContentData[] ContentDatas => ColoredItemDatas;

        public ColoredGlowItemData[] ColoredItemDatas;
    }

    [System.Serializable]
    public class ColoredGlowItemData : ContentData
    {
        public ColorType colorType;
        public Sprite Sprite;
    }
}