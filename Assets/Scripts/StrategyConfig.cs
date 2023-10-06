namespace Match3
{
    public class StrategyConfig
    {
        public FallDownFillStrategy FallDownFillStrategy { get; private set; }

        public BoardClearStrategy BoardClearStrategy { get; private set; }
        public MatchClearStrategy MatchClearStrategy { get; private set; }

        public void Initialize(IBoard board,ItemGenerator itemGenerator)
        {
            FallDownFillStrategy = new FallDownFillStrategy(board, itemGenerator);
            BoardClearStrategy = new BoardClearStrategy(FallDownFillStrategy);
            MatchClearStrategy = new MatchClearStrategy(BoardClearStrategy);
        }
    }
}