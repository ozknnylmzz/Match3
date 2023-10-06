
namespace Match3
{
    public class LevelLoader
    {
        public void Initialize(LevelGenerator levelGenerator)
        {
            LoadLevel(levelGenerator);
        }

        private void LoadLevel(LevelGenerator levelGenerator)
        {

            int[] configureTypes = Constants.CONFIGURETYPES_PIECE_VALUE_4;

            levelGenerator.SetConfigureTypes(configureTypes);

            levelGenerator.GenerateItemsPool(ItemType.BoardItem);
            levelGenerator.FillBoardWithItems();

        }

    }
}