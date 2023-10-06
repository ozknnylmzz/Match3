namespace Match3
{
    public interface IMatchDataProvider
    {
        public BoardMatchData GetMatchData(IBoard board, GridPosition[] gridPositions);
    }
}