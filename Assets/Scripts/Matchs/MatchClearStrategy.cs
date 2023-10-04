using System;
using System.Collections.Generic;
using System.Linq;

namespace CasualA.Board
{
    public class MatchClearStrategy
    {
        private readonly BoardClearStrategy _boardClearStrategy;
        private readonly ItemGenerator _itemGenerator;

        private List<SlotClearDataPerMatch> _slotClearDataPerMatchList;
        private HashSet<IGridSlot> _allSlots;
        private HashSet<GridItem> _allItems;
        public static event Func<IBoard, IEnumerable<IGridSlot>, IEnumerable<IGridSlot>> SideMatchItemRequest;
        private MatchData _matchData;

        public MatchClearStrategy(BoardClearStrategy boardClearStrategy, ItemGenerator itemGenerator,
            MatchData matchData)
        {
            _boardClearStrategy = boardClearStrategy;
            _itemGenerator = itemGenerator;
            _matchData = matchData;
        }

        public void CalculateMatchStrategyJobs(IBoard board, MatchData boardMatchData)
        {
            CalculateJobDatas(board, boardMatchData);

            SaveAllItems();

            _boardClearStrategy.ClearAllSlots(_allSlots);

            _boardClearStrategy.Refill(_allSlots, _allItems);
        }


        private void CalculateJobDatas(IBoard board, MatchData matchData)
        {
            InitializeAllCollections();
            _allSlots.UnionWith(matchData.GetSendMatchedDataListAsSet());
            ItemSelectionManager.RemoveSelectedSlots(_allSlots);
            CalculateHideJobs(board, matchData);
        }

        private void CalculateHideJobs(IBoard board, MatchData matchData)
        {
            HashSet<IGridSlot> allSlots = new HashSet<IGridSlot>();
            allSlots.UnionWith(matchData.GetSendMatchedDataListAsSet()) ;
            
            List<GridItem> allItems = allSlots.Select(slot => slot.Item).ToList();
            _boardClearStrategy.ClearSlots(board, allSlots,_allSlots,_slotClearDataPerMatchList);

            JobsExecutor.AddJob(new ItemsHideJob(allItems));
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