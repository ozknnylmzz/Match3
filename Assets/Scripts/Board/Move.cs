namespace Match3
{
    public class Move
    {
        public IGridSlot SelectedSlot { get; private set; }
        public IGridSlot TargetSlot { get; private set; }
        public BoardMatchData BoardMatchData { get; private set; }
        public int Priority { get; private set; }
        public bool IsTapMove { get; private set; } = false;

        public Move(IGridSlot selectedSlot, IGridSlot targetSlot, BoardMatchData boardMatchData)
        {
            SelectedSlot = selectedSlot;
            TargetSlot = targetSlot;
            BoardMatchData = boardMatchData;
        }

        public Move(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            SelectedSlot = selectedSlot;
            TargetSlot = targetSlot;
        }

        public Move(IGridSlot selectedSlot)
        {
            SelectedSlot = selectedSlot;
            IsTapMove = true;
        }

        public void SetPriority(int value)
        {
            Priority = value;
        }
    }
}