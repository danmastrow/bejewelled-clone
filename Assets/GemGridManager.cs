using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGridManager : MonoBehaviour {
    public GameObject RedGem, BlueGem, GreenGem, PurpleGem, YellowGem;

    [SerializeField]
    private Vector3 startingPosition;
    [SerializeField]
    private int numberOfRows;
    [SerializeField]
    private int numberOfColumns;
    private GemGrid grid;
    private List<GameObject> gems;
    void Awake() {
        gems = new List<GameObject> { RedGem, GreenGem, YellowGem, PurpleGem, BlueGem };
        grid = new GemGrid(numberOfRows, numberOfColumns);
        PopulateGridWithRandomGems(startingPosition, numberOfRows, numberOfColumns);
    }

    private void PopulateGridWithRandomGems(Vector3 startingPosition, int numberOfRows, int numberOfColumns) {
        var rand = new System.Random();
        foreach (var point in grid.GridPoints) {
            var randomGem = gems[rand.Next(0, gems.Count)];
            var gem = Instantiate(randomGem);
            grid.UpdateGridPointWithoutExploding(point.Position, gem);
            startingPosition = new Vector3(point.Position.x, point.Position.y, startingPosition.z);
            gem.GetComponent<MoveScript>().MoveToPosition(startingPosition);
            gem.name = $"{randomGem.name}";
        }

        // TODO: REFACTOR: Average guess that this finds a good enough distance for the camera to see everything
        var centerGemPosition = new Vector3(numberOfRows / 2, numberOfColumns / 2, -(numberOfRows / 2 + numberOfColumns / 2) - 1);
        Camera.main.GetComponent<MoveScript>().MoveToPosition(centerGemPosition);
        Debug.Log(grid.ToString());
        ExplodeSameNeighboursInGrid();
    }
    private void ExplodeSameNeighboursInGrid()
    {
        var neighboursToExplode = new List<GridPoint>();
        var neighbours = grid.GetAllAdjacent(grid.GridPoints[11]);

        foreach(var neighbour in neighbours)
        {

        }
        foreach (var neighbourToExplode in neighbours)
        {
            Debug.Log(neighbourToExplode.Position);
        }
    }
}