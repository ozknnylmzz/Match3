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

                    SetItemWithoutMatch(_board, gridSlot);
                }
            }
        }

        public void GenerateItemsPool(ItemType itemType)
        {
            ItemData itemData = _allItemsData.GetItemDataOfType(itemType);
            _itemGenerator.GeneratePool(itemData.ItemPrefab, itemData.ConfigureData.ItemPoolSize);
        }

        private void SetItemWithoutMatch(IBoard board, IGridSlot slot)
        {
            while (true)
            {
                GridItem item = _itemGenerator.GetRandomNormalItem();

                _itemGenerator.SetItemOnSlot(item, slot);

                BoardMatchData boardMatchData = _matchDataProvider.GetMatchData(board, slot.GridPosition);

                if (!boardMatchData.MatchExists) return ;

                item.Hide();

            }
        }
    }
}