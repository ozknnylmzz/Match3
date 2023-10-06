using System.Collections.Generic;
using System.Linq;

namespace Match3
{
    public class BoardClearStrategy
    {
        private readonly BaseFillStrategy _fillStrategy;

        public BoardClearStrategy(BaseFillStrategy fillStrategy)
        {
            _fillStrategy = fillStrategy;
        }

        public void ClearSlots(IBoard board, IEnumerable<IGridSlot> matchSlots, IEnumerable<IGridSlot> elementSlots,
            HashSet<IGridSlot> allSlots, ICollection<SlotClearDataPerMatch> slotClearDataPerMatchList)
        {
            IEnumerable<IGridSlot> slotsToClear = matchSlots.Union(elementSlots);
            IEnumerable<IGridSlot> chainSlots = GetChainSlots(board, slotsToClear);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(chainSlots);

            slotClearDataPerMatchList.Add(new SlotClearDataPerMatch(matchSlots, itemsToClear));

            allSlots.UnionWith(chainSlots);
        }

        public void Refill(IEnumerable<IGridSlot> allSlots, IEnumerable<GridItem> allItems)
        {
            _fillStrategy.AddFillJobs(allSlots, allItems);
        }

        private HashSet<IGridSlot> GetChainSlots(IBoard board, IEnumerable<IGridSlot> initialSlots)
        {
            HashSet<IGridSlot> slotsToClear = new HashSet<IGridSlot>(initialSlots);
            HashSet<IGridSlot> chainedSlots = new HashSet<IGridSlot>(initialSlots);

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
                    var selectionSlots = ItemSelectionManager.ExecuteSelectionRequests(board, chainedSlots);
                    if (selectionSlots.Count() == 0)
                    {
                        break;
                    }

                    slotsToClear.Clear();
                    slotsToClear.Clear();
                    slotsToClear.UnionWith(selectionSlots);
                    chainedSlots.UnionWith(selectionSlots);
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