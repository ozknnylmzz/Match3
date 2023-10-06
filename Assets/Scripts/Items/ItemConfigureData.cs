namespace Match3
{
    [System.Serializable]
    public struct ItemConfigureData
    {
        public ItemType ItemType;
        public int ConfigureType;

        public ItemConfigureData(int itemType, int configureType)
        {
            ItemType = (ItemType)itemType;
            ConfigureType = configureType;
        }

        public ItemConfigureData(ItemType itemType, int configureType)
        {
            ItemType = itemType;
            ConfigureType = configureType;
        }

        public static ItemConfigureData Empty { get; } = new ItemConfigureData(-1, -1);
    }
}