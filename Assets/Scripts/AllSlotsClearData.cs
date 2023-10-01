using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
{
    public class AllSlotsClearData
    {
        public IEnumerable<SlotClearDataPerMatch> SlotClearDataPerMatchList { get; }
        public IReadOnlyCollection<GridItem> AllItems { get; }


        public AllSlotsClearData(IEnumerable<SlotClearDataPerMatch> slotClearDataPerMatchList)
        {
            SlotClearDataPerMatchList = slotClearDataPerMatchList;

            AllItems = GetAllItems();
        }

        private IReadOnlyCollection<GridItem> GetAllItems()
        {
            HashSet<GridItem> allItems = new();

            foreach (SlotClearDataPerMatch slotClearDataPerMatch in SlotClearDataPerMatchList)
            {
                allItems.UnionWith(slotClearDataPerMatch.ItemsToClear);
            }

            return allItems;
        }
    }
}