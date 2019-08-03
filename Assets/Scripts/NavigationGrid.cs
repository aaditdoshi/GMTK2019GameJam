using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigationGrid : MonoBehaviour
{
    Tilemap TileMap;
    public class CellForAstar
    {
        public Vector3Int Cell;
        public CellForAstar(Vector3Int Cell)
        {
            this.Cell = Cell;
            g = 0.0f;
            f = 0.0f;
            h = 0.0f;
        }
        public CellForAstar Parent;
        public float g;
        public float f;
        public float h;
    }

    static List<CellForAstar> AlgoritmOpenList = new List<CellForAstar>();
    static List<Vector3Int> CloseList = new List<Vector3Int>();
    static List<Vector3Int> CurrentPath = new List<Vector3Int>();

    public int MaxHeight;

    public void Awake()
    {
        TileMap = GetComponentInChildren<Tilemap>();
    }

    public enum NodeDirection
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
    }

    public Vector3Int[] GetPath(Vector3 GoalFloat, Vector3 StartCellFloat, float ArrowAvoidance)
    {
        Vector3Int Goal = TileMap.WorldToCell(GoalFloat);
        Vector3Int startCell = TileMap.WorldToCell(StartCellFloat);

        AlgoritmOpenList.Clear();
        CloseList.Clear();
        CurrentPath.Clear();

        AlgoritmOpenList.Add(new CellForAstar(startCell));

        while (AlgoritmOpenList.Count > 0)
        {
            CellForAstar currentCell = AlgoritmOpenList[0];
            AlgoritmOpenList.RemoveAt(0);


            foreach (Vector3Int Node in IterateThroughNeighbors(currentCell.Cell))
            {
                if(!IsWalkable(Node, ArrowAvoidance))
                {
                    continue;
                }

                if (CloseList.Contains(Node))
                {
                    continue;
                }

                CellForAstar successorCell = new CellForAstar(Node);

                successorCell.Parent = currentCell;

                if (Node == Goal)
                {
                    TraceBackpath(successorCell);
                    break;
                }

                if (AlgoritmOpenList.Find(element => element.Cell == Node) == null)
                {
                    AlgoritmOpenList.Add(successorCell);
                }

            }

            if (CurrentPath.Count > 0)
            {
                break;
            }

            CloseList.Add(currentCell.Cell);
        }

        return CurrentPath.ToArray();
    }

    private bool IsWalkable(Vector3Int node, float ArrowAvoidance)
    {
     
        if(!GameRule.get.GetBounds().Contains(node))
        {
            return false;
        }

        RuleTile tile = TileMap.GetTile<RuleTile>(node);


        if (tile == null)
        {
            return false;
        }
        

        if(TileMap.GetColliderType(node) != Tile.ColliderType.None)
        {
            return false;
        }

        if(GameRule.get.HasStatue(TileMap.GetBoundsLocal(node)))
        {
            return false;
        }

        if(GameRule.get.NearBoomerang(node, ArrowAvoidance))
        {
            return false;
        }

        return true;
    }

    public static IEnumerable<Vector3Int> IterateThroughNeighbors(Vector3Int cell)
    {
        for (int i = 0; i <= (int)NodeDirection.Down; i++)
        {
            Vector3Int TestNode = cell;
            switch ((NodeDirection)i)
            {
                case NodeDirection.Left:
                    TestNode.x -= 1;
                    break;
                case NodeDirection.Right:
                    TestNode.x += 1;
                    break;
                case NodeDirection.Up:
                    TestNode.y += 1;
                    break;
                case NodeDirection.Down:
                    TestNode.y -= 1;
                    break;
            }
            yield return TestNode;
        }
    }

    private void TraceBackpath(CellForAstar successorCell)
    {
        CellForAstar currentNode = successorCell;
        while (currentNode != null)
        {
            CurrentPath.Add(currentNode.Cell);
            currentNode = currentNode.Parent;
        }
        CurrentPath.Reverse();

    }
}
