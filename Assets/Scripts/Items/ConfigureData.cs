using UnityEngine;

namespace Match3.Data
{
    public abstract class ConfigureData : ScriptableObject
    {
        public abstract ContentData[] ContentDatas { get; }

        public int ItemPoolSize;
    }

    public abstract class ContentData
    {}


    //public abstract class ConfigureContentData
    //{
    //    public ItemConfigureData ItemConfigureData;
    //}
}