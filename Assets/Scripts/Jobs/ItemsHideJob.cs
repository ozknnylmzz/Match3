using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Enums;
using Match3.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class ItemsHideJob : Job
    {
        private const float ScaleDuration = 0.13f;

        private readonly IEnumerable<GridItem> _items;

        public ItemsHideJob(IEnumerable<GridItem> items)
        {
            _items = items;

            ItemStateManager.AddJobToItems(items, this);
        }

        public override async UniTask ExecuteAsync()
        {
            await UniTask.WhenAll(_items.Select(item => UniTask.WaitUntil(() => item.ItemState == ItemState.Rest)));

            Sequence sequence = DOTween.Sequence();

            foreach (GridItem item in _items)
            {

                _ = sequence.Join(item.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), ScaleDuration)
                            .SetEase(Ease.InOutSine));
            }

            await sequence;
            
            foreach (GridItem item in _items)
            {
                item.Kill(isSpecialKill: false);
            }

            await UniTask.Delay(64);

            JobCompleted = true;
        }
    }
}