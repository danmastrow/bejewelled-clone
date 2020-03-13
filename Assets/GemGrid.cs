using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemGrid {
    public int RowCount { get; private set; }
    public int ColumnCount { get; private set; }
    public List<GridPoint> GridPoints { get; set; }
    public GemGrid(int rows, int columns) {
        GridPoints = new List<GridPoint>();
        RowCount = rows;
        ColumnCount = columns;
        for (int x = 0; x < RowCount; x++) {
            for (int y = 0; y < ColumnCount; y++) {
                GridPoints.Add(new GridPoint(new Vector2(x, y)));
            }
        }
    }

    public void UpdateGridPoint(Vector2 point, GameObject newContent = null) {
        var existingPoint = GridPoints.Where(x => x.Position == point).FirstOrDefault();
        if (existingPoint != null)
        {
            existingPoint.Content = newContent;
        }
        else
        {
            Debug.LogWarning($"Could not find point on grid: {point.x}:{point.y}");
        }
    }
    public void UpdateGridPoint(Vector2 point, GridPoint newGridPoint)
    {
        var existingGridPoint = GridPoints.Where(x => x.Position == point).FirstOrDefault();
        if (existingGridPoint != null)
        {
            existingGridPoint = newGridPoint;
        }
        else
        {
            Debug.LogWarning($"Could not find point on grid: {point.x}:{point.y}");
        }
    }

    public List<GridPoint> TrySwap(GameObject content1, GameObject content2)
    {
        var result = new List<GridPoint>();
        var point1 = GridPoints.Where(x => x.Content == content1).FirstOrDefault();
        var point2 = GridPoints.Where(x => x.Content == content2).FirstOrDefault();

        if(point1 == null || point2 == null)
        {
            Debug.LogWarning("Cannot swap as points do not exist in Grid");
        } else
        {
            var newPos1 = new Vector2(point2.Position.x, point2.Position.y);
            var newPos2 = new Vector2(point1.Position.x, point1.Position.y);
            point1 = new GridPoint(newPos1, point1.Content);
            point2 = new GridPoint(newPos2, point2.Content);
            result = new List<GridPoint> { point1, point2 };
            UpdateGridPoint(newPos1, point1.Content);
            UpdateGridPoint(newPos2, point2.Content);
        }

        return result;
    }

    public GridPoint GetTopNeighbour(GridPoint point)
    {
        var neighbour = GridPoints.Where(a => a.Position == new Vector2(point.Position.x, point.Position.y + 1)).FirstOrDefault();
        return neighbour;
    }

    public GridPoint GetRightNeighbour(GridPoint point)
    {
        var neighbour = GridPoints.Where(a => a.Position == new Vector2(point.Position.x + 1, point.Position.y)).FirstOrDefault();
        return neighbour;
    }

    public GridPoint GetLeftNeighbour(GridPoint point)
    {
        var neighbour = GridPoints.Where(a => a.Position == new Vector2(point.Position.x - 1, point.Position.y)).FirstOrDefault();
        return neighbour;
    }

    public GridPoint GetBottomNeighour(GridPoint point)
    {
        var neighbour = GridPoints.Where(a => a.Position == new Vector2(point.Position.x, point.Position.y + -1)).FirstOrDefault();
        return neighbour;
    }

    public List<GridPoint> GetVerticalNeighbours(GridPoint point)
    {
        var neighbours = new List<GridPoint>();
        for (int y = Convert.ToInt32(point.Position.y); y < ColumnCount; y++) // Get all neighbours up
        {
            var neighbour = GridPoints.Where(z => z.Position == new Vector2(point.Position.x, Convert.ToSingle(y))).FirstOrDefault();
            if (neighbour != null)
                neighbours.Add(neighbour);
        }

        for (int y = Convert.ToInt32(point.Position.y); y >= 0; y--) // Get all neighbours down
        {
            var neighbour = GridPoints.Where(z => z.Position == new Vector2(point.Position.x, Convert.ToSingle(y))).FirstOrDefault();
            if (neighbour != null)
                neighbours.Add(neighbour);
        }


        return neighbours;
    }

    public bool HasEmptyContent()
    {
        return GridPoints.Where(x => x.Content == null).Count() > 0;
    }
    public override string ToString()
    {
        var result = "";
        foreach (var gridpoint in GridPoints)
        {
            result += $"{gridpoint?.Position.x}:{gridpoint?.Position.y} = {gridpoint?.Content?.name}\n";
        }
        return result;
    }
}