using System;
using System.Collections.Generic;
using Match3.Enums;

namespace Match3.Items
{
    public static class DisposeManager
    {
        private static readonly List<IDisposable> _disposables;
        static DisposeManager()
        {
            _disposables = new();

            EventBus.Instance.Subscribe(BoardEvents.OnBoardDestroyed, DisposeAll);
        }

        public static void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        private static void DisposeAll()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }
    }
}