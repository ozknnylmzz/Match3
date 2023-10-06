using System.Collections.Generic;

namespace Match3
{
    public interface ISelectorItem 
    {
        public int SelectionPriority { get; }
        
        public IEnumerable<IGridSlot> SelectSlotsFrom(IBoard board, IEnumerable<IGridSlot> selectableSlots);
    }
}