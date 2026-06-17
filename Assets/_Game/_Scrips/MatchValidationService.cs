using System.Collections.Generic;

namespace Game.Onet
{
    public sealed class MatchValidationService : IMatchValidationService
    {
        private const int MaximumTurnCount = 2;

        private static readonly DirectionOffset[] DirectionOffsets =
        {
            new DirectionOffset(OnetDirection.Up, -1, 0),
            new DirectionOffset(OnetDirection.Down, 1, 0),
            new DirectionOffset(OnetDirection.Left, 0, -1),
            new DirectionOffset(OnetDirection.Right, 0, 1)
        };

        public MatchResult TryMatch(BoardState boardState, BoardCoord startCoord, BoardCoord targetCoord)
        {
            if (!CanAttemptMatch(boardState, startCoord, targetCoord))
            {
                return MatchResult.Failed();
            }

            return TryFindPath(boardState, startCoord, targetCoord, out MatchPath path)
                ? MatchResult.Succeeded(path)
                : MatchResult.Failed();
        }

        private static bool CanAttemptMatch(BoardState boardState, BoardCoord startCoord, BoardCoord targetCoord)
        {
            if (boardState == null)
            {
                return false;
            }

            if (!boardState.IsPlayable(startCoord) || !boardState.IsPlayable(targetCoord))
            {
                return false;
            }

            if (startCoord == targetCoord)
            {
                return false;
            }

            TileData startTile = boardState.GetTile(startCoord);
            TileData targetTile = boardState.GetTile(targetCoord);

            if (startTile.IsEmpty || targetTile.IsEmpty)
            {
                return false;
            }

            return startTile.IconId == targetTile.IconId;
        }

        private static bool TryFindPath(BoardState boardState, BoardCoord startCoord, BoardCoord targetCoord, out MatchPath path)
        {
            Queue<PathNode> frontier = new Queue<PathNode>();
            Dictionary<PathNodeKey, int> bestTurnCounts = new Dictionary<PathNodeKey, int>();

            frontier.Enqueue(PathNode.CreateStart(startCoord));
            bestTurnCounts[new PathNodeKey(startCoord, OnetDirection.None)] = 0;

            while (frontier.Count > 0)
            {
                PathNode currentNode = frontier.Dequeue();

                foreach (DirectionOffset directionOffset in DirectionOffsets)
                {
                    int nextTurnCount = CalculateTurnCount(currentNode.Direction, directionOffset.Direction, currentNode.TurnCount);
                    if (nextTurnCount > MaximumTurnCount)
                    {
                        continue;
                    }

                    BoardCoord nextCoord = directionOffset.Move(currentNode.Coord);
                    while (boardState.IsInside(nextCoord) && CanTraverse(boardState, nextCoord, targetCoord))
                    {
                        PathNodeKey nextKey = new PathNodeKey(nextCoord, directionOffset.Direction);
                        if (HasBetterOrEqualState(bestTurnCounts, nextKey, nextTurnCount))
                        {
                            nextCoord = directionOffset.Move(nextCoord);
                            continue;
                        }

                        bestTurnCounts[nextKey] = nextTurnCount;
                        PathNode nextNode = new PathNode(nextCoord, directionOffset.Direction, nextTurnCount, currentNode);

                        if (nextCoord == targetCoord)
                        {
                            path = BuildPath(nextNode);
                            return true;
                        }

                        frontier.Enqueue(nextNode);
                        nextCoord = directionOffset.Move(nextCoord);
                    }
                }
            }

            path = null;
            return false;
        }

        private static int CalculateTurnCount(OnetDirection currentDirection, OnetDirection nextDirection, int currentTurnCount)
        {
            if (currentDirection == OnetDirection.None || currentDirection == nextDirection)
            {
                return currentTurnCount;
            }

            return currentTurnCount + 1;
        }

        private static bool CanTraverse(BoardState boardState, BoardCoord coord, BoardCoord targetCoord)
        {
            return coord == targetCoord || boardState.IsEmpty(coord);
        }

        private static bool HasBetterOrEqualState(Dictionary<PathNodeKey, int> bestTurnCounts, PathNodeKey nextKey, int nextTurnCount)
        {
            return bestTurnCounts.TryGetValue(nextKey, out int bestTurnCount) && bestTurnCount <= nextTurnCount;
        }

        private static MatchPath BuildPath(PathNode endNode)
        {
            List<BoardCoord> reversedPoints = new List<BoardCoord>();
            PathNode currentNode = endNode;

            while (currentNode != null)
            {
                reversedPoints.Add(currentNode.Coord);
                currentNode = currentNode.Previous;
            }

            reversedPoints.Reverse();
            List<BoardCoord> simplifiedPoints = SimplifyPoints(reversedPoints);
            int turnCount = simplifiedPoints.Count - 2;
            return new MatchPath(simplifiedPoints, turnCount);
        }

        private static List<BoardCoord> SimplifyPoints(List<BoardCoord> points)
        {
            if (points.Count <= 2)
            {
                return points;
            }

            List<BoardCoord> simplifiedPoints = new List<BoardCoord>
            {
                points[0]
            };

            for (int index = 1; index < points.Count - 1; index++)
            {
                BoardCoord previous = points[index - 1];
                BoardCoord current = points[index];
                BoardCoord next = points[index + 1];

                bool isStraightRow = previous.Row == current.Row && current.Row == next.Row;
                bool isStraightColumn = previous.Column == current.Column && current.Column == next.Column;
                if (isStraightRow || isStraightColumn)
                {
                    continue;
                }

                simplifiedPoints.Add(current);
            }

            simplifiedPoints.Add(points[points.Count - 1]);
            return simplifiedPoints;
        }

        private sealed class PathNode
        {
            public PathNode(BoardCoord coord, OnetDirection direction, int turnCount, PathNode previous)
            {
                Coord = coord;
                Direction = direction;
                TurnCount = turnCount;
                Previous = previous;
            }

            public BoardCoord Coord { get; }

            public OnetDirection Direction { get; }

            public int TurnCount { get; }

            public PathNode Previous { get; }

            public static PathNode CreateStart(BoardCoord coord)
            {
                return new PathNode(coord, OnetDirection.None, 0, null);
            }
        }

        private readonly struct PathNodeKey
        {
            public PathNodeKey(BoardCoord coord, OnetDirection direction)
            {
                Coord = coord;
                Direction = direction;
            }

            public BoardCoord Coord { get; }

            public OnetDirection Direction { get; }
        }
    }
}
