using UnityEngine;

namespace CasualA.Board
{
    public class BoardInputController : MonoBehaviour
    {
        private Match3Game _match3Game;
        private IBoard _board;

        private GridPosition _selectedGridPosition;
        private GridPosition _targetGridPosition;
        private bool _isDragMode;

        private int _counter = 0;

        public void Initialize(Match3Game match3Game, IBoard board)
        {
            _match3Game = match3Game;
            _board = board;
            SubcribeEvents();
        }

        public void SubcribeEvents()
        {
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        public void UnsubcribeEvents()
        {
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        public void OnPointerDown(Vector2 pointerWorldPos)
        {
            // if (!_match3Game.IsSwapAllowed)
            // {
            //     return;
            // }

            _isDragMode = false;
            if (_match3Game.IsPointerOnBoard(pointerWorldPos, out _selectedGridPosition))
            {
                _isDragMode = true;
                // _match3Game.SetMatchData(_selectedGridPosition);
            }
        }

        GridPosition? _lastGridPosition = new GridPosition();

        public void OnPointerDrag(Vector2 pointerWorldPos)
        {
            if (!_isDragMode)
                return;

            if (_match3Game.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
            {
                if (!_lastGridPosition.HasValue || !_lastGridPosition.Value.Equals(targetGridPosition))
                {
                    _lastGridPosition = targetGridPosition;
                    _match3Game.CheckDiagonalMove();
                    _match3Game.IsSameItem(targetGridPosition);

                    _counter++;
                    Debug.Log("_counter" + _counter);
                }
            }
            else
            {
                _lastGridPosition = null;
            }
        }


        public void OnPointerUp(Vector2 pointerWorldPos)
        {
            if (_match3Game.IsMatchDetected(_counter))
            {
                // _match3Game.SetDiagonalData();
                _match3Game.CheckMove(_counter);
                Debug.Log("IsMatchDetected");

                return;
            }

            // _match3Game.ClearMatchData();
            _isDragMode = false;
            _counter = 0;
        }
    }
}