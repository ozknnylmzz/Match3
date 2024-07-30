using System.Collections.Generic;
using Match3.Boards;

namespace Match3.Items
{
    public interface ISelectorItem 
    {
        public int SelectionPriority { get; }
        
        public IEnumerable<IGridSlot> SelectSlotsFrom(IBoard board, IEnumerable<IGridSlot> selectableSlots);
    }
}