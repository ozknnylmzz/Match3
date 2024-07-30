using System.Collections.Generic;
using Match3.Items;

namespace Match3.Boards
{
    public class SlotClearDataPerMatch
    {
        public IEnumerable<IGridSlot> OriginalMatchSlots { get; }
        public IEnumerable<GridItem> ItemsToClear { get; }

        public SlotClearDataPerMatch(IEnumerable<IGridSlot> matchSlots,IEnumerable<GridItem> itemsToClear)
        {
            OriginalMatchSlots = matchSlots;
            ItemsToClear = itemsToClear;
        }
    }
}