using System.Collections.Generic;
using Match3.Boards;
using Match3.Items;
using Match3.Jobs;

namespace Match3.Strategy
{
    public class MatchClearStrategy
    {
        private readonly BoardClearStrategy _boardClearStrategy;

        private HashSet<IGridSlot> _allSlots;
        private HashSet<GridItem> _allItems;

        public MatchClearStrategy(BoardClearStrategy boardClearStrategy)
        {
            _boardClearStrategy = boardClearStrategy;
        }

        public void CalculateMatchStrategyJobs(BoardMatchData boardMatchData)
        {
            CalculateJobDatas(boardMatchData);

            SaveAllItems();

            _boardClearStrategy.ClearAllSlots(_allSlots);

            _boardClearStrategy.Refill(_allSlots, _allItems);
        }

        private void CalculateJobDatas(BoardMatchData boardMatchData)
        {
            InitializeAllCollections();

            _allSlots.UnionWith(boardMatchData.AllMatchedGridSlots);

            HashSet<GridItem> itemsToHide = BoardHelper.GetItemsOfSlots(_allSlots);

            JobsExecutor.AddJob(new ItemsHideJob(itemsToHide));
        }

        private void SaveAllItems()
        {
            _allItems = BoardHelper.GetItemsOfSlots(_allSlots);
        }

        private void InitializeAllCollections()
        {
            _allSlots = new();
        }
    }
}