using Match3.Boards;
using UnityEngine;

namespace match3.data
{
    [CreateAssetMenu(menuName = "Board/BoardConfigData", order = 1)]
    public class BoardConfigData : ScriptableObject
    {
        public int RowCount;
        public int ColumnCount;
        public float CellSpacing;
        public GridSlot Grid;
    }
}
