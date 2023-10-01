using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CasualA.Board
{
    public class JobsExecutor : IDisposable
    {
        public static IEnumerable<Job> Jobs => _jobs;

        private static List<Job> _jobs;

        public static Dictionary<int, List<ItemsFallJob>> _fallJobPairs;

        public JobsExecutor()
        {
            _jobs = new List<Job>();
            _fallJobPairs = new();
            DisposeManager.Add(this);
        }

        public async UniTask ExecuteJobsAsync()
        {
            ChainFallJobs();

            ItemStateManager.SetAllItemsState();
            
            await UniTask.WhenAll(_jobs.Select(job => job.ExecuteAsync()));

            ClearJobs();
        }

        public static void AddJob(Job job)
        {
            _jobs.Add(job);
        }

        public static void AddFallJob(ItemsFallJob fallJob, int columnIndex)
        {
            if (_fallJobPairs.ContainsKey(columnIndex))
            {
                _fallJobPairs[columnIndex].Add(fallJob);
            }
            else
            {
                _fallJobPairs[columnIndex] = new() { fallJob };
            }
        }

        private void ChainFallJobs()
        {
            foreach (var fallJobPerColumn in _fallJobPairs)
            {
                List<ItemsFallJob> fallJobList = fallJobPerColumn.Value;

                AddJob(fallJobList[0]);

                for (int i = 0; i < fallJobList.Count; i++)
                {
                    if(i + 1 < fallJobList.Count)
                    {
                        fallJobList[i].SetNextFallJob(fallJobList[i + 1]);
                    }             
                }
            }
        }

        public static List<ItemsFallJob> GetFallJobs(IBoard board)
        {
            List<ItemsFallJob> newFallJobs = new();
            
            for (int i = 0; i < board.ColumnCount; i++)
            {
                if (_fallJobPairs.ContainsKey(i))
                {
                    newFallJobs.AddRange(_fallJobPairs[i]);
                }
            }

            return newFallJobs;
        }

        private void ClearJobs()
        {
            _jobs.Clear();
            _fallJobPairs.Clear();
        }

        public void Dispose()
        {
            ClearJobs();
        }
    }
}