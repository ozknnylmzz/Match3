using System.Collections.Generic;
using Match3.Boards;
using Match3.Enums;

namespace Match3.Matchs
{
    public class MatchSequence
    {
        public IReadOnlyList<IGridSlot> MatchedGridSlots { get; }
        public MatchDetectorType MatchDetectorType { get; }

        public MatchSequence(IReadOnlyList<IGridSlot> matchedGridSlots,MatchDetectorType matchDetectorType)
        {
            MatchedGridSlots = matchedGridSlots;
            MatchDetectorType = matchDetectorType;
        }
    } 
}