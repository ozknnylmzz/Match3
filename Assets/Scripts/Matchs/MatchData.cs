using System.Collections.Generic;


namespace Match3
{
    public class MatchData
    {
        public HashSet<MatchSequence> MatchedSequences;
        public int MatchItemId;
        public HashSet<IGridSlot> MatchedGridSlots;

        #region Variables

        private HashSet<MatchSequence> _matchedSequences
        {
            set => MatchedSequences = value;
        }

        private HashSet<IGridSlot> _matchedGridSlots
        {
            set => MatchedGridSlots = value;
        }


        private int _matchItemId
        {
            set => MatchItemId = value;
        }

        #endregion

        public MatchData(HashSet<MatchSequence> matchedSequences, int matchItemId)
        {
            SetMatchDatas(matchedSequences, matchItemId);
        }


        public void SetMatchDatas(HashSet<MatchSequence> matchedSequences, int matchItemId)
        {
            _matchedSequences = matchedSequences;
            _matchItemId = matchItemId;
            _matchedGridSlots = GetMatchedGridSlots();
        }


        private HashSet<IGridSlot> GetMatchedGridSlots()
        {
            HashSet<IGridSlot> matchedGridSlots = new();

            foreach (MatchSequence sequence in MatchedSequences)
            {
                matchedGridSlots.UnionWith(sequence.MatchedGridSlots);
            }


            return matchedGridSlots;
        }
      
    }
}