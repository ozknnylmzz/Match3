using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Board : MonoBehaviour, IBoard
    {
        private const float _cellSize = 1f;

        [SerializeField] private int _rowCount;
        [SerializeField] private int _columnCount;

        [SerializeField] private float _cellSpacing = 0.05f;

        [SerializeField] private GridSlot _grid;

        private Vector3 _originPos;

        private IGridSlot[,] _gridSlots;
        private GridPosition[] _allGridPositions;

        public IGridSlot this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public IGridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public GridPosition[] AllGridPositions => _allGridPositions;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        public void Initialize()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {

            _gridSlots = new IGridSlot[_rowCount, _columnCount];
            _allGridPositions = new GridPosition[_rowCount * _columnCount];

            _originPos = GetOriginPosition(_rowCount, _columnCount);

            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    int iteration = i * _columnCount + j;
                    Vector3 slotPosition = GridToWorldPosition(i, j);

                    GridSlot gridSlot = Instantiate(_grid, slotPosition, Quaternion.identity, transform);
                    gridSlot.name = "(" + i + " , " + j + ")";

                    GridPosition gridPosition = new GridPosition(i, j);
                    gridSlot.SetPosition(gridPosition, slotPosition);

                    _gridSlots[i, j] = gridSlot;
                    _allGridPositions[iteration] = gridPosition;
                }
            }
        }

       

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition gridPosition)
        {
            gridPosition = WorldToGridPosition(pointerWorldPos);
            return IsPositionOnBoard(gridPosition);
        }

        public bool IsPositionInBounds(GridPosition gridPosition)
        {
            return gridPosition.IsPositionInBounds(_rowCount, _columnCount);
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionInBounds(gridPosition);
        }

        public bool IsPositionOnItem(GridPosition gridPosition)
        {
            return IsPositionInBounds(gridPosition) && this[gridPosition].CanContainItem;
        }

        public GridPosition WorldToGridPosition(Vector3 pointerWorldPos)
        {
            Vector2 gridPos = (pointerWorldPos - _originPos) / (_cellSize + _cellSpacing);

            int rowIndex = Mathf.FloorToInt(gridPos.y + _cellSize / 2);
            int columnIndex = Mathf.FloorToInt(gridPos.x + _cellSize / 2);

            return new GridPosition(rowIndex, columnIndex);
        }

        public Vector3 GridToWorldPosition(GridPosition gridPosition)
        {
            return GridToWorldPosition(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        private Vector3 GridToWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, rowIndex) * (_cellSize + _cellSpacing) + _originPos;
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            float gridWidth = columnCount * _cellSize + (columnCount - 1) * _cellSpacing;
            float gridHeight = rowCount * _cellSize + (rowCount - 1) * _cellSpacing;

            float originX = -gridWidth / 2.0f + _cellSize / 2;
            float originY = -gridHeight / 2.0f + _cellSize / 2;

            return new Vector3(originX, originY);
        }

      

        public IEnumerator<IGridSlot> GetEnumerator()
        {
            foreach (IGridSlot slot in _gridSlots)
            {
                yield return slot;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}