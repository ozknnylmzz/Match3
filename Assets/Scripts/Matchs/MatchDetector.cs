using System.Collections.Generic;

namespace CasualA.Board
{
    public class MatchDetector
    {
        public int MinMatchAmount { get; } = 3;

        private MatchData _matchData;

        public MatchDetector(MatchData matchData)
        {
            _matchData = matchData;
        }
        protected bool IsMatchAvailable(IBoard board, List<IGridSlot> initialSlot, out IGridSlot currentSlot)
        {
            currentSlot = null;

            // currentSlot = board[newPosition];

            // return IsSameSlot(initialSlot, currentSlot);
            return true;
        }
        public MatchSequence GetMatchSequence(IBoard board, params GridPosition[] gridPositions)
        {
            List<IGridSlot> matchedGridSlots=new List<IGridSlot>();
            foreach (var gridPosition in gridPositions)
            {
                matchedGridSlots.Add(board[gridPosition]);
            }

            if (matchedGridSlots.Count<3)
            {
                return null;
            }
        

            if (IsEnoughMatch(matchedGridSlots))
            {
                return new MatchSequence(matchedGridSlots);
            }

            return null;
        }

        protected bool IsEnoughMatch(IReadOnlyList<IGridSlot> matchedGridSlots)
        {
            return matchedGridSlots.Count >= MinMatchAmount;
        }

        private bool IsSameSlot(IGridSlot initialSlot, IGridSlot currentSlot)
        {
            if (!currentSlot.HasItem || !currentSlot.Item.IsMatchable)
            {
                return false;
            }

            return initialSlot.ItemId == currentSlot.ItemId;
        }
    }
}