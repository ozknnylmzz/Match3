using Match3.Boards;
using Match3.Enums;

namespace Match3.Matchs
{
    public interface IMatchDetector
    {
        public MatchSequence GetMatchSequence(IBoard board, GridPosition gridPosition);
        public MatchDetectorType MatchDetectorType { get; }
    } 
}