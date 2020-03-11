using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public int RowCount { get; private set; }
    public int ColumnCount { get; private set; }
    public List<GridPoint> GridPoints { get; private set; }
    public Grid(int rows, int columns) {
        GridPoints = new List<GridPoint>();
        RowCount = rows;
        ColumnCount = columns;
        for (int x = 0; x < RowCount; x++) {
            for (int y = 0; y < ColumnCount; y++) {
                GridPoints.Add(new GridPoint(new Vector2(x, y)));
            }
        }
    }

    public void UpdateGridPoint(Vector2 point, GameObject newValue) {

        // ExplodeSameNeighbours();
    }

    private void ExplodeSameNeighbours() {
        // For every point in the dictionary
        // Get every point in the the column and row of the point, in a row explode them

        foreach (var point in GridPoints) {
            var horizontalNeighbours = new List<Vector2>();

        }
    }
}