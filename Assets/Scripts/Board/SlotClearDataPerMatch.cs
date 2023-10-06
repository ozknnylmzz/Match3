using System.Collections.Generic;

namespace Match3
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