using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private AllItemsData _allItemsData;

        private ItemGenerator _itemGenerator;
        private MatchDataProvider _matchDataProvider;
        private IBoard _board;

        public void Initialize(IBoard board,ItemGenerator itemGenerator)
        {
            _board = board;
            _itemGenerator = itemGenerator;
        }


        public void SetConfigureTypes(int[] possibleConfigureTypes)
        {
            _itemGenerator.SetConfigureTypes(possibleConfigureTypes);
        }

        public List<ItemConfigureData> FillBoardWithItems()
        {
            for (int i = 0; i < _board.RowCount; i++)
            {
                for (int j = 0; j < _board.ColumnCount; j++)
                {
                    IGridSlot gridSlot = _board[i, j];

                    if (!gridSlot.CanSetItem)
                        continue;

                    SetItemWithoutMatch(_board, gridSlot);
                }
            }

            return GetBoardData();
        }

        private List<ItemConfigureData> GetBoardData()
        {
            List<ItemConfigureData> initialBoardData = new();

            foreach (IGridSlot slot in _board)
            {
                initialBoardData.Add(new ItemConfigureData(slot.Item.ItemType, slot.Item.ConfigureType));
            }

            return initialBoardData;
        }

        public void FillBoardWithReceivedData(ItemConfigureData[] itemDataList)
        {
            for (int i = 0; i < _board.RowCount; i++)
            {
                for (int j = 0; j < _board.ColumnCount; j++)
                {
                    ItemConfigureData itemConfigureData = itemDataList[i * _board.ColumnCount + j];

                    ItemData itemData = _allItemsData.GetItemDataOfType(itemConfigureData.ItemType);

                    IGridSlot currentSlot = _board[i, j];

                    GridItem currentItem = _itemGenerator.GetItemWithId(itemData.ItemPrefab.ItemType,
                        itemConfigureData.ConfigureType);


                    _itemGenerator.SetItemOnSlot(currentItem, currentSlot);
                }
            }
        }


        public void GenerateItemsPool(ItemType itemType)
        {
           
                ItemData itemData = _allItemsData.GetItemDataOfType(itemType);
                _itemGenerator.GeneratePool(itemData.ItemPrefab, itemData.ConfigureData.ItemPoolSize);
        }


        private GridItem SetItemWithoutMatch(IBoard board, IGridSlot slot)
        {
            while (true)
            {
                GridItem item = _itemGenerator.GetRandomNormalItem();

                _itemGenerator.SetItemOnSlot(item, slot);

                
                return item;
                // if (!boardMatchData.MatchExists) return item;

            }
        }
    }
}