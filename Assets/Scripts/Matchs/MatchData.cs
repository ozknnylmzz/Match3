using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public class MatchData
    {
        public HashSet<MatchSequence> MatchedSequences;
        public GridPosition MatchPosition;
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


        private List<IGridSlot> FindMaxRowSlots()
        {
            List<int> maxRowIndexes = MatchedGridSlots.Select(slot => slot.GridPosition.RowIndex).ToList();
            var maxRowIndex = maxRowIndexes.Max();
            List<IGridSlot> maxRowSlots =
                MatchedGridSlots.Where(slot =>
                        slot.GridPosition.RowIndex == maxRowIndex && slot.Item.ItemState == ItemState.WaitingToFall)
                    .ToList();
            return maxRowSlots;
        }

        private IGridSlot FindMinColumSlot(List<IGridSlot> maxRowSlots)
        {
            List<int> minColumIndexes = maxRowSlots.Select(slot => slot.GridPosition.ColumnIndex).ToList();
            int minColumIndex = minColumIndexes.Min();

            IGridSlot minColSlot =
                maxRowSlots.FirstOrDefault(slot => slot.GridPosition.ColumnIndex == minColumIndex);

            return minColSlot;
        }

        private IGridSlot FindLongMatchedGridSlotsForRow()
        {
            int maxRepeatedValue = -1;

            List<int> rowList = MatchedGridSlots.Select(matchData => matchData.GridPosition.RowIndex).ToList();

            maxRepeatedValue = rowList
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .FirstOrDefault();

            List<IGridSlot> longMatchedGridSlots = MatchedGridSlots
                .Where(matchDataSlot => maxRepeatedValue == matchDataSlot.GridPosition.RowIndex).ToList();

            List<int> columnList =
                longMatchedGridSlots.Select(matchData => matchData.GridPosition.ColumnIndex).ToList();

            int middleValue = columnList.OrderBy(x => x).Skip((columnList.Count - 1) / 2).First();

            IGridSlot boosterAutomatchPos =
                longMatchedGridSlots.FirstOrDefault(slot =>
                    slot.GridPosition.ColumnIndex == middleValue && slot.GridPosition.RowIndex == maxRepeatedValue);


            return boosterAutomatchPos;
        }
    }
}