using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CasualA.Board
{
    public class MatchDataProvider : IMatchDataProvider
    {
        private readonly IMatchDetector[] _matchDetectors;

        public MatchDataProvider(IMatchDetector[] matchDetectors)
        {
            _matchDetectors = matchDetectors;
        }

        // public BoardMatchData GetMatchDataOnBoardInit(IBoard board, params GridPosition[] gridPositions)
        // {
        //     MatchedDataAllSlots matchedDataAllSlots = new MatchedDataAllSlots();
        //
        //     foreach (GridPosition gridPosition in gridPositions)
        //     {
        //         if (ContainsPosition(matchedDataAllSlots.AllMatchedGridSlots, gridPosition))
        //         {
        //             continue;
        //         }
        //
        //         UnionSharedData(matchedDataAllSlots, gridPosition, board, gridPositions);
        //     }
        //
        //     return new BoardMatchData(matchedDataAllSlots.MatchDataList, matchedDataAllSlots.AllMatchedGridSlots);
        // }

        public BoardMatchData GetMatchData(IBoard board, params GridPosition[] gridPositions)
        {
            // MatchedDataAllSlots matchedDataAllSlots = new MatchedDataAllSlots();

            foreach (GridPosition gridPosition in gridPositions)
            {
                UnionSharedData(matchedDataAllSlots, gridPosition, board, gridPositions);
            }

            return new BoardMatchData(matchedDataAllSlots.MatchDataList, matchedDataAllSlots.AllMatchedGridSlots);
        }

        private void UnionSharedData( GridPosition gridPosition,
            IBoard board,
            params GridPosition[] gridPositions)
        {
            HashSet<MatchSequence> matchSequences = GetMatchSequences( gridPosition, board);
            if (matchSequences.Count > 0)
            {
                MatchData matchData = new MatchData(matchSequences, gridPosition, board[gridPosition].ItemId,
                    gridPositions.Length > 2);


                if (IsSharedMatchData(matchData, matchedDataAllSlots.MatchDataList,
                        out List<MatchData> sharedMatchDatas))
                {
                    foreach (MatchData sharedMatchData in sharedMatchDatas)
                    {
                        matchedDataAllSlots.MatchDataList.Remove(sharedMatchData);

                        matchSequences.UnionWith(sharedMatchData.MatchedSequences);

                        matchData.SetMatchDatas(matchSequences, gridPosition, board[gridPosition].ItemId,
                            gridPositions.Length > 2);
                    }
                }

            }
        }

        private HashSet<MatchSequence> GetMatchSequences(
            GridPosition gridPosition, IBoard board)
        {
            HashSet<MatchSequence> matchSequences = new HashSet<MatchSequence>();

            foreach (IMatchDetector matchDetector in _matchDetectors)
            {
                MatchSequence sequence = matchDetector.GetMatchSequence(board, gridPosition);

                if (sequence == null)
                {
                    continue;
                }


                matchSequences.Add(sequence);
            }

            return matchSequences;
        }


        private bool ContainsPosition(HashSet<IGridSlot> allMatchedGridSlots, GridPosition gridPosition)
        {
            return allMatchedGridSlots.Any(slot => slot.GridPosition == gridPosition);
        }

        private bool IsSharedMatchData(MatchData currentMatchData, List<MatchData> matchDataList,
            out List<MatchData> sharedMatchData)
        {
            bool isSharedFound = false;
            sharedMatchData = new List<MatchData>();
            foreach (MatchData matchData in matchDataList)
            {
                if (currentMatchData.MatchedGridSlots.Overlaps(matchData.MatchedGridSlots))
                {
                    sharedMatchData.Add(matchData);
                    isSharedFound = true;
                }
            }

            return isSharedFound;
        }
    }

    public class MatchedDataAllSlots
    {
        public List<MatchData> MatchDataList;
        public HashSet<IGridSlot> AllMatchedGridSlots;

        public MatchedDataAllSlots()
        {
            MatchDataList = new List<MatchData>();
            AllMatchedGridSlots = new HashSet<IGridSlot>();
        }
    }
}