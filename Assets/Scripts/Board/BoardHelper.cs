using System.Collections.Generic;


namespace Match3
{
    public static class BoardHelper
    {
        private readonly static Dictionary<ColorType, int> _itemColorCounts;

        static BoardHelper()
        {
            _itemColorCounts = new();
        }

     
        public static HashSet<IGridSlot> GetSideSlots(IGridSlot powerUpSlot, IBoard board, int distance = 1)
        {
            return GetDirectionalSlots(powerUpSlot, board, GridPosition.SideDirections, distance);
        }


        public static HashSet<IGridSlot> GetNeighbourSlots(GridPosition powerUpPosition, IBoard board, int distance = 1)
        {
            HashSet<IGridSlot> slotsToDestroy = new HashSet<IGridSlot>();

            for (int i = powerUpPosition.RowIndex - distance; i <= powerUpPosition.RowIndex + distance; i++)
            {
                for (int j = powerUpPosition.ColumnIndex - distance; j <= powerUpPosition.ColumnIndex + distance; j++)
                {
                    if (board.IsPositionOnBoard(new GridPosition(i, j)))
                    {
                        slotsToDestroy.Add(board[i, j]);
                    }
                }
            }

            return slotsToDestroy;
        }

        public static HashSet<GridItem> GetItemsOfSlots(IEnumerable<IGridSlot> slotsToChooseFrom)
        {
            HashSet<GridItem> items = new();

            foreach (IGridSlot slot in slotsToChooseFrom)
            {
                items.Add(slot.Item);
            }

            return items;
        }


        private static HashSet<IGridSlot> GetDirectionalSlots(IGridSlot directionalSlots, IBoard board, GridPosition[] directions, int distance = 1)
        {
            HashSet<IGridSlot> slotsToDestroy = new HashSet<IGridSlot>();

            foreach (GridPosition direction in directions)
            {
                GridPosition newPosition = directionalSlots.GridPosition + distance * direction;

                if (board.IsPositionOnBoard(newPosition))
                {
                    IGridSlot newSlot = board[newPosition];
                    slotsToDestroy.Add(newSlot);
                }
            }

            return slotsToDestroy;
        }
    }
}