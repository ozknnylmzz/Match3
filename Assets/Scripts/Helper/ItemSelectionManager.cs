using System.Collections.Generic;
using System.Linq;

namespace Match3
{
    public static class ItemSelectionManager
    {
        private static readonly List<ISelectorItem> _selectorItems = new();

        private static readonly HashSet<IGridSlot> _selectableSlots = new();
        private static readonly HashSet<IGridSlot> _selectedSlots = new();


        public static IEnumerable<IGridSlot> ExecuteSelectionRequests(IBoard board, IEnumerable<IGridSlot> unselectableSlots)
        {
            if (_selectorItems.Count == 0)
            {
                return Enumerable.Empty<IGridSlot>();
            }
            
            _selectorItems.Sort((x, y) => x.SelectionPriority.CompareTo(y.SelectionPriority));

            RemoveSelectedSlots(unselectableSlots);

            foreach (ISelectorItem item in _selectorItems)
            {
                IEnumerable<IGridSlot> itemSelectedSlots = item.SelectSlotsFrom(board, _selectableSlots);

                RemoveSelectedSlots(itemSelectedSlots);
            }

            _selectorItems.Clear();

            return _selectedSlots;
        }

        public static void RemoveSelectedSlots(IEnumerable<IGridSlot> slots)
        {
            _selectableSlots.ExceptWith(slots);
            _selectedSlots.UnionWith(slots);
        }


        public static void Reset(IBoard board)
        {
            _selectableSlots.Clear();
            _selectorItems.Clear();
            _selectedSlots.Clear();
            _selectableSlots.UnionWith(board);
        }
    }
}