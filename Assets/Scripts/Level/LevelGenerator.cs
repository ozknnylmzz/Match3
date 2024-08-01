using Match3.Boards;
using Match3.Data;
using Match3.Enums;
using Match3.Game;
using Match3.Items;
using Match3.Matchs;
using UnityEngine;

namespace Match3.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private AllItemsData _allItemsData;

        private ItemGenerator _itemGenerator;
        private MatchDataProvider _matchDataProvider;
        private IBoard _board;

        public void Initialize(IBoard board, ItemGenerator itemGenerator, GameConfig gameConfig)
        {
            _board = board;
            _itemGenerator = itemGenerator;
            _matchDataProvider = gameConfig.MatchDataProvider;
        }

        public void SetConfigureTypes(int[] possibleConfigureTypes)
        {
            _itemGenerator.SetConfigureTypes(possibleConfigureTypes);
        }

        public void FillBoardWithItems()
        {
            for (int i = 0; i < _board.RowCount; i++)
            {
                for (int j = 0; j < _board.ColumnCount; j++)
                {
                    IGridSlot gridSlot = _board[i, j];

                    if (!gridSlot.CanSetItem)
                        continue;

                    SetItemWithoutMatch(gridSlot, i);
                }
            }
        }

        public void GenerateItemsPool(ItemType itemType)
        {
            ItemData itemData = _allItemsData.GetItemDataOfType(itemType);
            _itemGenerator.GeneratePool(itemData.ItemPrefab, itemData.ConfigureData.ItemPoolSize);
        }

        private void SetItemWithoutMatch(IGridSlot slot, int colorType)
        {
            GridItem item = colorType == 4 ? _itemGenerator.GetRedNormalItem() : _itemGenerator.GetRandomNormalItem();

            _itemGenerator.SetItemOnSlot(item, slot);
        }
    }
}