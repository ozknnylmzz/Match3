namespace CasualA.Board
{
    public interface IMatchDataProvider
    {
        public BoardMatchData GetMatchData(IBoard board, GridPosition[] gridPositions);
    }
}