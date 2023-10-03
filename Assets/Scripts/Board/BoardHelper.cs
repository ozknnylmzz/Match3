using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CasualA.Board
{
    public static class BoardHelper
    {
        private readonly static Dictionary<ColorType, int> _itemColorCounts;

        static BoardHelper()
        {
            _itemColorCounts = new();
        }

        public static IGridSlot GetRandomSlotOnBoard(IBoard board)
        {
            return board[Random.Range(0, board.RowCount), Random.Range(0, board.ColumnCount)];
        }

        public static HashSet<IGridSlot> GetRowSlots(IBoard board, int rowIndex)
        {
            HashSet<IGridSlot> slotsToDestroy = new HashSet<IGridSlot>();

            for (int i = 0; i < board.ColumnCount; i++)
            {
                IGridSlot slot = board[rowIndex, i];

                if (slot.CanContainItem)
                {
                    slotsToDestroy.Add(slot);
                }
            }

            return slotsToDestroy;
        }

        public static HashSet<IGridSlot> GetColumnSlots(IBoard board, int columnIndex)
        {
            HashSet<IGridSlot> slotsToDestroy = new HashSet<IGridSlot>();

            for (int i = 0; i < board.RowCount; i++)
            {
                IGridSlot slot = board[i, columnIndex];

                if (slot.CanContainItem)
                {
                    slotsToDestroy.Add(slot);
                }
            }

            return slotsToDestroy;
        }

        public static HashSet<IGridSlot> GetSideSlots(IGridSlot powerUpSlot, IBoard board, int distance = 1)
        {
            return GetDirectionalSlots(powerUpSlot, board, GridPosition.SideDirections, distance);
        }

        public static HashSet<IGridSlot> GetDiagonalSlots(IGridSlot diagonel, IBoard board, int distance = 1)
        {
            return GetDirectionalSlots(diagonel, board, GridPosition.DiagonalDirections, distance);
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

        public static HashSet<IGridSlot> GetBombSlots(IBoard board, IGridSlot targetSlot, int distance)
        {
            HashSet<IGridSlot> slotsToDestroy = GetNeighbourSlots(targetSlot.GridPosition, board, distance);
            slotsToDestroy.UnionWith(GetSideSlots(targetSlot, board, distance + 1));

            return slotsToDestroy;
        }

        public static HashSet<IGridSlot> GetRandomSlots(HashSet<IGridSlot> slotsToChoose, int randomSlotCount)
        {
            HashSet<IGridSlot> slotsToDestroy = new HashSet<IGridSlot>();

            if (slotsToChoose.Count > randomSlotCount)
            {
                for (int i = 0; i < randomSlotCount; i++)
                {
                    IGridSlot gridSlot = slotsToChoose.Pop();

                    slotsToDestroy.Add(gridSlot);
                }
            }
            else
            {
                slotsToDestroy.UnionWith(slotsToChoose);
            }

            return slotsToDestroy;
        }

        public static HashSet<IGridSlot> GetSlotsOfType<T>(IBoard board, IEnumerable<IGridSlot> slotsToChooseFrom = null) where T : GridItem
        {
            return GetFilteredSlots(board, slot => slot.Item is T, slotsToChooseFrom);
        }

        public static HashSet<IGridSlot> GetSlotsOfColor(IBoard board, ColorType color, IEnumerable<IGridSlot> slotsToChooseFrom = null)
        {
            return GetFilteredSlots(board, slot => slot.Item.ColorType == color, slotsToChooseFrom);
        }

        public static HashSet<IGridSlot> GetFilteredSlots(IBoard board, Func<IGridSlot, bool> filterCondition, IEnumerable<IGridSlot> slotsToFilterFrom = null)
        {
            HashSet<IGridSlot> slots = new();

            slotsToFilterFrom ??= board;

            foreach (IGridSlot slot in slotsToFilterFrom)
            {
                if (filterCondition(slot))
                {
                    slots.Add(slot);
                }
            }

            return slots;
        }

      

        private static ColorType GetTopMostColor(Dictionary<ColorType, int> itemColorCounts, int order)
        {
            if (itemColorCounts.Count == 0)
            {
                return ColorType.None;
            }

            var sortedColors = itemColorCounts.OrderByDescending(kvp => kvp.Value);
            return sortedColors.ElementAt(order - 1).Key;
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

        public static IEnumerable<IGridSlot> GetCornerSlots(IBoard board)
        {
            var cornerSlots = new List<IGridSlot>
            {
                board[0, 0],
                board[0, board.ColumnCount - 1],
                board[board.RowCount - 1, 0],
                board[board.RowCount - 1, board.ColumnCount - 1]
            };

            return cornerSlots;
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