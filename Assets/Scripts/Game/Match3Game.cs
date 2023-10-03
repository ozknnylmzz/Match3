using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
{
    public class Match3Game : MonoBehaviour
    {
        private bool _isSwapAllowed = true;

        private IBoard _board;
        private MatchData _matchData;
        private MatchClearStrategy _matchClearStrategy;
        private BoardClearStrategy _boardClearStrategy;
        private JobsExecutor _jobsExecutor;


        public bool IsSwapAllowed => _isSwapAllowed;

        public void Initialize(StrategyConfig strategyConfig, MatchData matchData, IBoard board)
        {
            _board = board;

            _boardClearStrategy = strategyConfig.BoardClearStrategy;
            _matchClearStrategy = strategyConfig.MatchClearStrategy;
            _jobsExecutor = new JobsExecutor();

            _matchData = matchData;
        }

        private void OnEnable()
        {
            EventManager.Subscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }


        public async void SwapItemsAsync()
        {
            await DoSwap();
        }

        private async UniTask DoSwap()
        {
            EnableSwap();
        }


        private async void StartJobs()
        {
            EventManager.Execute(BoardEvents.OnBeforeJobsStart);
            await _jobsExecutor.ExecuteJobsAsync();

            EventManager.Execute(BoardEvents.OnAfterJobsCompleted);
        }

       

        public void IsSameItem(GridPosition gridPosition)
        {
            IGridSlot gridSlot = _board[gridPosition];
            // Debug.Log("SetMatchDatas" + gridSlot.Item.ColorType);
            _matchData.SetMatchDatas(gridSlot);
        }

    

        public void CheckDiagonalMove()
        {
            if (_matchData.CheckDiagonalPosition(_board))
            {
                _matchData.IsDiagonalMove = true;
                _matchData.DiagonelMoveCount++;
            }
        }
        public void CheckMove(int counter)
        {
            if (_matchData.MatchedDataList.Count<3)
            {
                return;
            }

            if (_matchData.IsDiagonalMove)
            {
                _matchData.DiagonalLogic(counter);
            }
           
            else if (_matchData.CheckOrthogonalMatch(counter))
            {
                Debug.Log("CheckOrthogonalMatch");

                _matchData.OrthogonalLogic(counter);
            }
            else
            {
                Debug.Log("match yok ");

                return;
            }
          
            SwapItemsAsync();
        }
        
        public bool IsMatchDetected(int counter)
        {
            if (counter<3)
            {
                return false;
            }

            return true;
        }

       

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        #region Swap

        public void EnableSwap()
        {
            _isSwapAllowed = true;
            Debug.Log("CalculateMatchStrategyJobs" + _matchData.MatchedDataList.Count);

            _matchClearStrategy.CalculateMatchStrategyJobs(_board, _matchData);
            StartJobs();
        }

        public void DisableSwap()
        {
            _isSwapAllowed = false;
        }

        #endregion
    }
}