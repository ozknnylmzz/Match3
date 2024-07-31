using System.Collections.Generic;
using Match3.Items;

namespace Match3.Boards
{
    public static class BoardHelper
    {
        public static HashSet<GridItem> GetItemsOfSlots(IEnumerable<IGridSlot> slotsToChooseFrom)
        {
            HashSet<GridItem> items = new();

            foreach (IGridSlot slot in slotsToChooseFrom)
            {
                items.Add(slot.Item);
            }

            return items;
        }
    }
}