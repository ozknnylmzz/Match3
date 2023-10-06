using System;
using System.Collections.Generic;

namespace Match3
{
    public static class DisposeManager
    {
        private static readonly List<IDisposable> _disposables;

        static DisposeManager()
        {
            _disposables = new();

            EventManager.Subscribe(BoardEvents.OnBoardDestroyed, DisposeAll);
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