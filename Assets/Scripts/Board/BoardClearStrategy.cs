using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CasualA.Board
{
    public class BoardClearStrategy
    {
        private readonly BaseFillStrategy _fillStrategy;

        public BoardClearStrategy(BaseFillStrategy fillStrategy)
        {
            _fillStrategy = fillStrategy;
        }

        public void ClearSlots(IBoard board, IEnumerable<IGridSlot> matchSlots, IEnumerable<IGridSlot> elementSlots, HashSet<IGridSlot> allSlots, ICollection<SlotClearDataPerMatch> slotClearDataPerMatchList)
        {
            IEnumerable<IGridSlot> slotsToClear = matchSlots.Union(elementSlots);
            IEnumerable<IGridSlot> chainSlots = GetChainSlots(board, slotsToClear);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(chainSlots);

            slotClearDataPerMatchList.Add(new SlotClearDataPerMatch(matchSlots, itemsToClear));

            allSlots.UnionWith(chainSlots);
        }

        public void ClearSlotsOnPowerUpSwapWithMatch(IBoard board, IGridSlot powerUpSlot, HashSet<IGridSlot> allSlots, ICollection<SlotClearDataPerMatch> slotClearDataPerMatchList)
        {
            HashSet<IGridSlot> slotsToDestroy = new() { powerUpSlot };

            HashSet<IGridSlot> slotsToClear = GetChainSlots(board, slotsToDestroy);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(slotsToClear);

            slotClearDataPerMatchList.Add(new SlotClearDataPerMatch(slotsToDestroy, itemsToClear));

            allSlots.UnionWith(slotsToClear);
        }

        public void ClearSlots(IBoard board, IGridSlot comboSlot, IEnumerable<IGridSlot> slotsToDestroy)
        {
            IEnumerable<IGridSlot> slotsToClear = GetChainSlots(board, slotsToDestroy);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(slotsToClear);

            SlotClearDataPerMatch slotClearDataPerMatch = new SlotClearDataPerMatch(comboSlot == null ? slotsToDestroy : new HashSet<IGridSlot> { comboSlot }, itemsToClear);
            EventManager<AllSlotsClearData>.Execute(BoardEvents.OnSlotsCalculated, new AllSlotsClearData(new List<SlotClearDataPerMatch> { slotClearDataPerMatch }));

            ClearAllSlots(slotsToClear);
            Refill(slotsToClear, itemsToClear);
        }

        public void ClearSlots(IBoard board, IEnumerable<IGridSlot> slotsToDestroy, IGridSlot comboSlot = null)
        {
            IEnumerable<IGridSlot> slotsToClear = GetChainSlots(board, slotsToDestroy);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(slotsToClear);

            SlotClearDataPerMatch slotClearDataPerMatch = new SlotClearDataPerMatch(comboSlot == null ? slotsToDestroy : new HashSet<IGridSlot> { comboSlot }, itemsToClear);
            EventManager<AllSlotsClearData>.Execute(BoardEvents.OnSlotsCalculated, new AllSlotsClearData(new List<SlotClearDataPerMatch> { slotClearDataPerMatch }));

            ClearAllSlots(slotsToClear);
            Refill(slotsToClear, itemsToClear);
        }


        public void Refill(IEnumerable<IGridSlot> allSlots, IEnumerable<GridItem> allItems)
        {
            _fillStrategy.AddFillJobs(allSlots, allItems);
        }

        private HashSet<IGridSlot> GetChainSlots(IBoard board, IEnumerable<IGridSlot> initialSlots)
        {
            HashSet<IGridSlot> slotsToClear = new(initialSlots);
            HashSet<IGridSlot> chainedSlots = new(initialSlots);

            while (true)
            {
                HashSet<IGridSlot> slotsToDestroy = new();

                foreach (IGridSlot currentSlot in slotsToClear)
                {
                    if (currentSlot.IsFound)
                    {
                        continue;
                    }

                    currentSlot.SetFound(true);

                 
                }

                if (slotsToDestroy.Count == 0)
                {
                    slotsToClear.Clear();
                 
                }
                else
                {
                    slotsToClear = slotsToDestroy;
                    chainedSlots.UnionWith(slotsToDestroy);
                }
            }

            return chainedSlots;
        }

        public void ClearAllSlots(IEnumerable<IGridSlot> allSlots)
        {
            foreach (IGridSlot slot in allSlots)
            {
                slot.Item.ReturnToPool();
                slot.ClearSlot();
            }
        }
    }
}