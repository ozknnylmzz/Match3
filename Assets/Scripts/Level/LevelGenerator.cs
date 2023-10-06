using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private AllItemsData _allItemsData;

        private ItemGenerator _itemGenerator;
        private MatchDataProvider _matchDataProvider;
        private IBoard _board;

        public void Initialize(IBoard board,ItemGenerator itemGenerator,GameConfig gameConfig)
        {
            _board = board;
            _itemGenerator = itemGenerator;
            _matchDataProvider = gameConfig.MatchDataProvider;
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

                BoardMatchData boardMatchData = _matchDataProvider.GetMatchData(board, slot.GridPosition);
               
                if (!boardMatchData.MatchExists) return item;
                
                item.Hide();

            }
        }
    }
}