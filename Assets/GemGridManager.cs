using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        GenerateGrid();
    }

    private void PopulateGridWithRandomGems(Vector3 startingPosition, int numberOfRows, int numberOfColumns) {
        var rand = new System.Random();
        foreach (var point in grid.GridPoints) {
            var randomGem = gems[rand.Next(0, gems.Count)];
            var gem = Instantiate(randomGem);
            grid.UpdateGridPoint(point.Position, gem);
            startingPosition = new Vector3(point.Position.x, point.Position.y, startingPosition.z);
            gem.GetComponent<MoveScript>().MoveToPosition(startingPosition);
            gem.name = $"{randomGem.name}";
        }

        // TODO: REFACTOR: Average guess that this finds a good enough distance for the camera to see everything
        var centerGemPosition = new Vector3(numberOfRows / 2, numberOfColumns / 2, -(numberOfRows / 2 + numberOfColumns / 2) - 1);
        Camera.main.GetComponent<MoveScript>().MoveToPosition(centerGemPosition);
        Debug.Log(grid.ToString());
    }

    public void GenerateGrid()
    {
        DestroyGridContent();
        grid = new GemGrid(numberOfRows, numberOfColumns);
        PopulateGridWithRandomGems(startingPosition, numberOfRows, numberOfColumns);
    }
    public void DestroyGridContent()
    {
        foreach (var point in grid.GridPoints)
            if (point.Content != null)
                Destroy(point.Content);
    }
    public void ExplodeAdjacentNeighbors()
    {
        var gemsToExplode = new List<GridPoint>();
        foreach (var point in grid.GridPoints)
        {
    
                var topNeighbours = GetValidExplodingNeighbours(point, grid.GetTopNeighbour);
                var bottomNeighbours = GetValidExplodingNeighbours(point, grid.GetBottomNeighour);
                var leftNeighbours = GetValidExplodingNeighbours(point, grid.GetLeftNeighbour);
                var rightNeighbours = GetValidExplodingNeighbours(point, grid.GetRightNeighbour);
                gemsToExplode.AddRange(topNeighbours);
                gemsToExplode.AddRange(bottomNeighbours);
                gemsToExplode.AddRange(leftNeighbours);
                gemsToExplode.AddRange(rightNeighbours);

        }
        gemsToExplode = gemsToExplode.GroupBy(x => x.Position).Select(x => x.First()).ToList();


        foreach (var gem in gemsToExplode) {
            if (gem.Content != null)
            {
                Destroy(gem.Content);
                grid.UpdateGridPoint(gem.Position, null);
            }
        }

    }


    private List<GridPoint> GetValidExplodingNeighbours(GridPoint point, Func<GridPoint, GridPoint> getNextNeighbour)
    {
        var gemsToExplode = new List<GridPoint>();

        var validNeighbours = new List<GridPoint> { point };
        var adjacentNeighbour = getNextNeighbour(point);
        while (point != null 
            && point.Content != null 
            && adjacentNeighbour != null 
            && adjacentNeighbour.Content != null 
            && adjacentNeighbour.Content.GetComponent<GemScript>().Color == point.Content.GetComponent<GemScript>().Color)
        {
            validNeighbours.Add(adjacentNeighbour);
            adjacentNeighbour = getNextNeighbour(adjacentNeighbour);
        }
        if (validNeighbours.Count >= 3)
            gemsToExplode.AddRange(validNeighbours);

        return gemsToExplode;
    }
}