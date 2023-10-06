using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public class Match3Game : MonoBehaviour
    {
        private bool _isSwapAllowed = true;
        private IMatchDataProvider _matchDataProvider;
        private IBoard _board;
        private MatchData _matchData;
        private MatchClearStrategy _matchClearStrategy;
        private JobsExecutor _jobsExecutor;

        private ItemSwapper _itemSwapper;
        public bool IsSwapAllowed => _isSwapAllowed;

        public void Initialize(StrategyConfig strategyConfig, GameConfig gameConfig, IBoard board)
        {
            _board = board;

            _matchClearStrategy = strategyConfig.MatchClearStrategy;
            _jobsExecutor = new JobsExecutor();
            _itemSwapper = new ItemSwapper();
            _matchDataProvider = gameConfig.MatchDataProvider;
        }

        private void OnEnable()
        {
            EventManager.Subscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }


        public async void SwapItemsAsync(GridPosition selectedPosition, GridPosition targetPosition)
        {
            IGridSlot selectedSlot = _board[selectedPosition];
            IGridSlot targetSlot = _board[targetPosition];
            await DoNormalSwap(selectedSlot, targetSlot);
        }
        private async UniTask DoNormalSwap(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);

            if (IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition, targetSlot.GridPosition))
            {
                EventManager.Execute(BoardEvents.OnSwapSuccess);

                ItemSelectionManager.Reset(_board);
                _matchClearStrategy.CalculateMatchStrategyJobs(_board, boardMatchData);

                CheckAutoMatch();
                StartJobs();
            }
            else
            {
                SwapItemsBack(selectedSlot, targetSlot);
            }
        }
      

        private async void StartJobs()
        {
            EventManager.Execute(BoardEvents.OnBeforeJobsStart);
            await _jobsExecutor.ExecuteJobsAsync();

            EventManager.Execute(BoardEvents.OnAfterJobsCompleted);
            EnableSwap();
        }

        
        public void CheckAutoMatch()
        {
            EventManager.Execute(BoardEvents.OnSequenceDataCalculated);

            while (IsMatchDetected(out BoardMatchData boardMatchData, _board.AllGridPositions) )
            {
                ItemSelectionManager.Reset(_board);

                _matchClearStrategy.CalculateMatchStrategyJobs(_board, boardMatchData);
                EventManager.Execute(BoardEvents.OnSequenceDataCalculated);
            }
        }
        
        public bool IsMatchDetected(out BoardMatchData boardMatchData, params GridPosition[] gridPositions)
        {
            boardMatchData = _matchDataProvider.GetMatchData(_board, gridPositions);

            return boardMatchData.MatchExists;
        }   
     
        
        private UniTask SwapItemsAnimation(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            return _itemSwapper.SwapItems(selectedSlot, targetSlot, this);
        }

        private async void SwapItemsBack(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);
            EnableSwap();
        }
      
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        #region Swap

        public void EnableSwap()
        {
            _isSwapAllowed = true;
        }

        public void DisableSwap()
        {
            _isSwapAllowed = false;
        }

        #endregion
    }
}