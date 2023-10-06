

namespace Match3
{
    public class GameConfig
    {
        public MatchDataProvider MatchDataProvider { get; private set; }
        
        
        public void Initialize()
        {
            MatchDataProvider = new MatchDataProvider(GetMatchDetectors());
        
        }
        
        private IMatchDetector[] GetMatchDetectors()
        {
            return new IMatchDetector[]
            {
                new HorizontalMatchDetector(),
                new VerticalMatchDetector()
            };
        }
    }
}