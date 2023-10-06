using System.Collections.Generic;

namespace Match3
{
    public interface IChainable
    {
        public void Use(IBoard board, out HashSet<IGridSlot> slotsToDestroy);
    }

    public interface ISideMatchItem : IChainable
    {
        public void SetMatchSlots(IEnumerable<IGridSlot> matchSlots);
    }

    public interface ISelectorItem : IChainable
    {
        public int SelectionPriority { get; }
        
        public void SendSelectionRequest();

        public IEnumerable<IGridSlot> SelectSlotsFrom(IBoard board, IEnumerable<IGridSlot> selectableSlots);
    }
}