using Match3.Boards;
using Match3.Items;

namespace Match3.Strategy
{
    public class StrategyConfig
    {
        private FallDownFillStrategy FallDownFillStrategy { get; set; }
        private BoardClearStrategy BoardClearStrategy { get; set; }
        public MatchClearStrategy MatchClearStrategy { get; private set; }

        public void Initialize(IBoard board,ItemGenerator itemGenerator)
        {
            FallDownFillStrategy = new FallDownFillStrategy(board, itemGenerator);
            BoardClearStrategy = new BoardClearStrategy(FallDownFillStrategy);
            MatchClearStrategy = new MatchClearStrategy(BoardClearStrategy);
        }
    }
}