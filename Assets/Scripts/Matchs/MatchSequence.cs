using System.Collections.Generic;

namespace Match3
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