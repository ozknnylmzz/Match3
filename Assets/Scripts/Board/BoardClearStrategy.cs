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

        public void ClearSlots(IBoard board, IEnumerable<IGridSlot> slotsToDestroy)
        {
            // Debug.Log("slotsToClear"+slotsToClear.Count());
            IEnumerable<IGridSlot> slotsToClear = GetChainSlots(board, slotsToDestroy);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(slotsToClear);
            
            ClearAllSlots(slotsToClear);
            Refill(slotsToClear, itemsToClear);
        }
        public void ClearSlots(IBoard board, IEnumerable<IGridSlot> matchSlots, HashSet<IGridSlot> allSlots, ICollection<SlotClearDataPerMatch> slotClearDataPerMatchList)
        {
            IEnumerable<IGridSlot> chainSlots = GetChainSlots(board, matchSlots);
            IEnumerable<GridItem> itemsToClear = BoardHelper.GetItemsOfSlots(chainSlots);

            slotClearDataPerMatchList.Add(new SlotClearDataPerMatch(matchSlots, itemsToClear));

            allSlots.UnionWith(chainSlots);
            
            
        }
    


        public void Refill(IEnumerable<IGridSlot> allSlots, IEnumerable<GridItem> allItems)
        {
            if (allItems.Count()>0)
            {
                Debug.Log("allSlots"+allSlots.Count()+"allItems"+allItems.Count());
                _fillStrategy.AddFillJobs(allSlots, allItems);
            }
          
        }

        private HashSet<IGridSlot> GetChainSlots(IBoard board, IEnumerable<IGridSlot> initialSlots)
        {
            HashSet<IGridSlot> slotsToClear = new HashSet<IGridSlot>(initialSlots);
            HashSet<IGridSlot> chainedSlots =new HashSet<IGridSlot>(initialSlots);

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