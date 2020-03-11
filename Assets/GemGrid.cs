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

    public void UpdateGridPoint(Vector2 point, GameObject newContent) {
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

    public List<GridPoint> GetAllAdjacent(GridPoint point)
    {
        var neighbours = new List<GridPoint>();
        for (int y = Convert.ToInt32(point.Position.y); y < ColumnCount; y++) // Get all neighbours up
        {
            var neighbour = GridPoints.Where(z => z.Position == new Vector2(point.Position.x, Convert.ToSingle(y))).FirstOrDefault();
            if (neighbour != null)
                neighbours.Add(neighbour);
        }
        for (int x = Convert.ToInt32(point.Position.x); x < RowCount; x++) // Get all neighbours to the right
        {
            var neighbour = GridPoints.Where(z => z.Position == new Vector2(Convert.ToSingle(x), point.Position.y)).FirstOrDefault();
            if(neighbour != null)
                neighbours.Add(neighbour);
        }

        for (int y = Convert.ToInt32(point.Position.y); y >= 0; y--) // Get all neighbours down
        {
            var neighbour = GridPoints.Where(z => z.Position == new Vector2(point.Position.x, Convert.ToSingle(y))).FirstOrDefault();
            if (neighbour != null)
                neighbours.Add(neighbour);
        }

        for (int x = Convert.ToInt32(point.Position.x); x >= 0; x--) // Get all neighbours to the left
        {
            var neighbour = GridPoints.Where(z => z.Position == new Vector2(Convert.ToSingle(x), point.Position.y)).FirstOrDefault();
            if (neighbour != null)
                neighbours.Add(neighbour);
        }
        return neighbours;
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