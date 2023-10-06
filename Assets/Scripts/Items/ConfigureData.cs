using UnityEngine;

namespace Match3
{
    public abstract class ConfigureData : ScriptableObject
    {
        public abstract ContentData[] ContentDatas { get; }

        public int ItemPoolSize;
    }

    public abstract class ContentData
    {}


    public abstract class EffectConfigureData : ScriptableObject
    {
        public abstract ConfigureContentData[] ContentDatas { get; }

        public ConfigureContentData GetContentData(ItemConfigureData itemConfigureData)
        {
            foreach (var contentData in ContentDatas)
            {
                if (contentData.ItemConfigureData.Equals(itemConfigureData))
                {
                    return contentData;
                }
            }

            Debug.LogError("Could not find any content data. Check scriptable object");

            return null;
        }
    }

    public abstract class ConfigureContentData
    {
        public ItemConfigureData ItemConfigureData;
    }
}