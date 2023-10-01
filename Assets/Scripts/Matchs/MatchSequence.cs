using System.Collections.Generic;

namespace CasualA.Board
{
    public class MatchSequence
    {
        public IReadOnlyList<IGridSlot> MatchedGridSlots { get; }

        public MatchSequence(IReadOnlyList<IGridSlot> matchedGridSlots)
        {
            MatchedGridSlots = matchedGridSlots;
        }
    } 
}