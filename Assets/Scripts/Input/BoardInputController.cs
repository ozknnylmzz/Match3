using Match3.Boards;
using Match3.Enums;
using Match3.Game;
using UnityEngine;

namespace Match3.Input
{
    public class BoardInputController : MonoBehaviour
    {
        private Match3Game _match3Game;

        private GridPosition _selectedGridPosition;

        private bool _isDragMode;

        public void Initialize(Match3Game match3Game)
        {
            _match3Game = match3Game;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        public void UnsubscribeEvents()
        {
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        private void OnPointerDown(Vector2 pointerWorldPos)
        {
            if (!_match3Game.IsSwapAllowed)
            {
                return;
            }

            _isDragMode = false;

            if (_match3Game.IsPointerOnBoard(pointerWorldPos, out _selectedGridPosition))
            {
                _isDragMode = true;
            }
        }

        private void OnPointerDrag(Vector2 pointerWorldPos)
        {
            if (!_isDragMode)
                return;

            if (!_match3Game.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
            {
                _isDragMode = false;
                return;
            }

            if (!IsSideGrid(targetGridPosition))
            {
                return;
            }

            _isDragMode = false;

            SwapAsync((_selectedGridPosition, targetGridPosition));
        }

        private void OnPointerUp(Vector2 pointerWorldPos)
        {
            _isDragMode = false;
            if (!_match3Game.IsSwapAllowed)
            {
                return;
            }

            if (!_match3Game.IsPointerOnBoard(pointerWorldPos, out _selectedGridPosition))
            {
                return;
            }

            _isDragMode = false;
        }

        private bool IsSideGrid(GridPosition gridPosition)
        {
            bool isSideGrid = gridPosition.Equals(_selectedGridPosition + GridPosition.Up) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Down) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Left) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Right);

            return isSideGrid;
        }
        
        private void SwapAsync((GridPosition selectedGridPosition, GridPosition targetGridPosition) swapInput)
        {
            _match3Game.DisableSwap();
            _match3Game.SwapItemsAsync(swapInput.selectedGridPosition, swapInput.targetGridPosition);
        }
    }
}