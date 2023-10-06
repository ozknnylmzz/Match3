using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(menuName = "Board/AllItemsData")]
    public class AllItemsData : ScriptableObject
    {
        public ItemData[] ItemDatas;

        public ItemData GetItemDataOfType(ItemType itemType)
        {
            foreach (ItemData itemData in ItemDatas)
            {
                if (itemData.ItemPrefab.ItemType == itemType)
                {
                    return itemData;
                }
            }

            Debug.LogError("Could not find given Item type. Check scriptable object!");

            return null;
        }
    }

    [System.Serializable]
    public class ItemData
    {
        public GridItem ItemPrefab;
        public ConfigureData ConfigureData;
    }
}