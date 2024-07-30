using System.Collections.Generic;
using Match3.Matchs;

namespace Match3.Boards
{
    public class BoardMatchData
    {
        public IReadOnlyList<MatchData> MatchedDataList { get; }

        public IReadOnlyCollection<IGridSlot> AllMatchedGridSlots { get; }

        public bool MatchExists => MatchedDataList.Count != 0;

        public BoardMatchData(IReadOnlyList<MatchData> matchedDataList, IReadOnlyCollection<IGridSlot> allMatchedGridSlots)
        {
            MatchedDataList = matchedDataList;
            AllMatchedGridSlots = allMatchedGridSlots;
        }
    } 
}