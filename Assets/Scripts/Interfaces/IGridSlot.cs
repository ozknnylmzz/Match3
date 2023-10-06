using UnityEngine;

namespace Match3
{
    public interface IGridSlot
    {
        public bool CanSetItem { get; }
        
        public int ItemId { get; }
                
        public bool HasItem { get; }

        public bool IsMovable { get; }
        
        public bool CanContainItem { get; }

        public bool IsFound { get; }

        public bool IsItemDroppedTo { get; }

        public GridPosition GridPosition { get; }

        public Vector3 WorldPosition { get; }

        public GridItem Item { get; }


        public void SetItem(GridItem item);

        public void ClearSlot();

        public void SetFound(bool isFound);

        public void SetItemDrop(bool value);

     
    }
}