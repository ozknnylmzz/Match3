namespace Match3
{
    public interface IMatchDetector
    {
        public MatchSequence GetMatchSequence(IBoard board, GridPosition gridPosition);
        public MatchDetectorType MatchDetectorType { get; }
    } 
}