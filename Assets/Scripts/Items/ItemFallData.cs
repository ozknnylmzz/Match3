using Match3.Boards;

namespace Match3.Items
{
    public class ItemFallData
    {
        public GridItem Item { get; private set; }
        public IGridSlot DestinationSlot { get; }
        public int PathDistance { get; }

        public ItemFallData(GridItem item, IGridSlot destinationSlot, int pathDistance)
        {
            Item = item;
            DestinationSlot = destinationSlot;
            PathDistance = pathDistance;
            item.SetPathDistance(pathDistance);
        }
    } 
}