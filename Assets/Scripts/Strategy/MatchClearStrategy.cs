using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Boards;
using Match3.Items;
using Match3.Jobs;
using Match3.Matchs;

namespace Match3.Strategy
{
    public class MatchClearStrategy
    {
        private readonly BoardClearStrategy _boardClearStrategy;

        private List<SlotClearDataPerMatch> _slotClearDataPerMatchList;
        private HashSet<IGridSlot> _allSlots;
        private HashSet<GridItem> _allItems;
        public static event Func<IBoard, IEnumerable<IGridSlot>, IEnumerable<IGridSlot>> SideMatchItemRequest;

        public MatchClearStrategy(BoardClearStrategy boardClearStrategy)
        {
            _boardClearStrategy = boardClearStrategy;
        }

        public void CalculateMatchStrategyJobs(IBoard board, BoardMatchData boardMatchData)
        {
            CalculateJobDatas(board, boardMatchData);

            SaveAllItems();

            _boardClearStrategy.ClearAllSlots(_allSlots);

            _boardClearStrategy.Refill(_allSlots, _allItems);
        }


        private void CalculateJobDatas(IBoard board, BoardMatchData boardMatchData)
        {
            InitializeAllCollections();
            _allSlots.UnionWith(boardMatchData.AllMatchedGridSlots);

            foreach (MatchData matchData in boardMatchData.MatchedDataList)
            {
                IEnumerable<IGridSlot> elementSlots = SideMatchItemRequest?.Invoke(board, matchData.MatchedGridSlots) ?? Enumerable.Empty<IGridSlot>();

                CalculateHideJobs(board, matchData,elementSlots);
            }
        }

        private void CalculateHideJobs(IBoard board, MatchData matchData, IEnumerable<IGridSlot> elementSlots)
        {
            HashSet<GridItem> itemsToHide = BoardHelper.GetItemsOfSlots(matchData.MatchedGridSlots);

            _boardClearStrategy.ClearSlots(board, matchData.MatchedGridSlots, elementSlots, _allSlots, _slotClearDataPerMatchList);

            JobsExecutor.AddJob(new ItemsHideJob(itemsToHide));
        }

        private void SaveAllItems()
        {
            _allItems = BoardHelper.GetItemsOfSlots(_allSlots);
        }

        private void InitializeAllCollections()
        {
            _slotClearDataPerMatchList = new();
            _allSlots = new();
        }
    }
}