using Match3.Boards;

namespace Match3.Matchs
{
    public interface IMatchDataProvider
    {
        public BoardMatchData GetMatchData(IBoard board, GridPosition[] gridPositions);
    }
}