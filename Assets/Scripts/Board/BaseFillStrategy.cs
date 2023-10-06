using System.Collections.Generic;

namespace Match3
{
    public abstract class BaseFillStrategy 
    {
        protected readonly IBoard _board;
        protected readonly ItemGenerator _itemGenerator;

        protected BaseFillStrategy(IBoard board, ItemGenerator itemGenerator)
        {
            _board = board;
            _itemGenerator = itemGenerator;
        }

        public abstract void AddFillJobs(IEnumerable<IGridSlot> allSlots, IEnumerable<GridItem> allItems);
    }
}