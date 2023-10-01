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
        private HashSet<IGridSlot> _powerUpGenerationSlots;
        private HashSet<IGridSlot> _allSlots;
        private HashSet<GridItem> _allItems;

        public static event Func<IBoard, IEnumerable<IGridSlot>, IEnumerable<IGridSlot>> SideMatchItemRequest;

        public MatchClearStrategy(BoardClearStrategy boardClearStrategy, ItemGenerator itemGenerator)
        {
            _boardClearStrategy = boardClearStrategy;
            _itemGenerator = itemGenerator;
        }

        public void CalculateMatchStrategyJobs(IBoard board, BoardMatchData boardMatchData)
        {
            CalculateJobDatas(board, boardMatchData);

            SendDataEvents();

            SaveAllItems();

            _boardClearStrategy.ClearAllSlots(_allSlots);

            SetPowerUpsOnSlot();

            _boardClearStrategy.Refill(_allSlots, _allItems);
        }

     

        private void CalculateJobDatas(IBoard board, BoardMatchData boardMatchData, bool isLightballSwap = false)
        {
            InitializeAllCollections();

            _allSlots.UnionWith(boardMatchData.AllMatchedGridSlots);

            ItemSelectionManager.RemoveSelectedSlots(_allSlots);

            foreach (MatchData matchData in boardMatchData.MatchedDataList)
            {

                IEnumerable<IGridSlot> elementSlots = SideMatchItemRequest?.Invoke(board, matchData.MatchedGridSlots) ?? Enumerable.Empty<IGridSlot>();

                if (powerUpType == ItemType.None)
                {
                    if (isLightballSwap)
                    {
                        ItemSelectionManager.AddSlots(matchData.MatchedGridSlots);
                        continue;
                    }

                    CalculateHideJobs(board, matchData, elementSlots);
                }
             
            }
        }

        private void CalculateHideJobs(IBoard board, MatchData matchData, IEnumerable<IGridSlot> elementSlots)
        {
            HashSet<GridItem> itemsToHide = BoardHelper.GetItemsOfSlots(matchData.MatchedGridSlots);

            _boardClearStrategy.ClearSlots(board, matchData.MatchedGridSlots, elementSlots, _allSlots, _slotClearDataPerMatchList);

            JobsExecutor.AddJob(new ItemsHideJob(itemsToHide));
        }

        private void CalculatePowerUpJobs(IBoard board, MatchData matchData, IEnumerable<IGridSlot> elementSlots, ItemType powerUpType)
        {
            HashSet<GridItem> itemsToGeneratePowerUps = BoardHelper.GetItemsOfSlots(matchData.MatchedGridSlots);

            _boardClearStrategy.ClearSlots(board, matchData.MatchedGridSlots, elementSlots, _allSlots, _slotClearDataPerMatchList);

            PowerUpItem powerUpItem = (PowerUpItem)_itemGenerator.GetItemWithId(powerUpType);
            IGridSlot powerUpSlot = board[matchData.MatchPosition];

            _powerUpItemData.Add((powerUpItem, powerUpSlot));
            _powerUpGenerationSlots.UnionWith(matchData.MatchedGridSlots);

            JobsExecutor.AddJob(new ItemsPowerUpJob(itemsToGeneratePowerUps, powerUpItem));
        }

        private void SetPowerUpsOnSlot()
        {
            foreach (var (powerUpItem, powerUpSlot) in _powerUpItemData)
            {
                _itemGenerator.SetItemOnSlot(powerUpItem, powerUpSlot);
            }
        }

        private void SaveAllItems()
        {
            _allItems = BoardHelper.GetItemsOfSlots(_allSlots);
        }

        private void SendDataEvents()
        {
            EventManager<AllSlotsClearData>.Execute(BoardEvents.OnSlotsCalculated, new AllSlotsClearData(_slotClearDataPerMatchList));

            if (_powerUpItemData.Count > 0)
            {
                EventManager<ExtraMoveData>.Execute(BoardEvents.OnPowerUpDataCalculated, new ExtraMoveData(_powerUpGenerationSlots, _powerUpItemData[0].powerUpSlot));
                EventManager<List<(PowerUpItem powerUpItem, IGridSlot powerUpSlot)>>.Execute(BoardEvents.OnCreatedPowerUps, _powerUpItemData);
            }

        }

        private void InitializeAllCollections()
        {
            _slotClearDataPerMatchList = new();
            _powerUpItemData = new();
            _powerUpGenerationSlots = new();
            _allSlots = new();
        }
    }
}