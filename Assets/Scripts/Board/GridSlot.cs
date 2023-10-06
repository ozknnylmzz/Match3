using UnityEngine;

namespace Match3
{
  
    public class GridSlot : MonoBehaviour, IGridSlot
    {
        public bool CanSetItem => !HasItem;

        public int ItemId => (int)Item.ColorType;

        public bool HasItem => Item != null;

        public bool CanContainItem => true;

        public bool IsFound { get; private set; } = false;

        public bool IsMovable => HasItem;

        public bool IsItemDroppedTo { get; private set; }

        public GridItem Item { get; private set; }

        public GridPosition GridPosition { get; private set; }

        public Vector3 WorldPosition { get; private set; }



        public void SetPosition(GridPosition gridPosition, Vector3 worldPosition)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
        }

        public void SetItem(GridItem item)
        {
            Item = item;
            Item.SetSlot(this);
        }

        public void ClearSlot()
        {
            Item = default;
            SetFound(false);
        }

        public void SetFound(bool isFound)
        {
            IsFound = isFound;
        }

        public void SetItemDrop(bool value)
        {
            IsItemDroppedTo = value;
        }

        public override string ToString()
        {
            return GridPosition.ToString();
        }
    } 
}