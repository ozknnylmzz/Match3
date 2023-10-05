using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
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
            Debug.Log("SetMatchDatas" + gridSlot.Item.ColorType);
            _matchData.SetMatchDatas(gridSlot);
        }

        public bool CheckMove(int counter)
        {
            if (!_matchData.CheckMatchData(counter))
            {
                return false;
            }
            
            _matchData.SendMatchData(counter);
            if (_matchData.SendDataList.Count<3)
            {
                return false;
            }
            
            return true;
        }

       

        public bool IsMatchDetected()
        {
            return false;
        }


        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        #region Swap

        public void EnableSwap()
        {
            _isSwapAllowed = true;
            Debug.Log("_matchData.SendDataList.Count" + _matchData.SendDataList.Count);

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