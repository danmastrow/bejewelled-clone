using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    [SerializeField]
    private int newGemMoveSpeed;
    [SerializeField]
    private int explodingCheckRate;
    private GemGrid grid;
    private List<GameObject> gems;
    private IEnumerator explodeGridPointsCoroutine;

    public bool GridReady { get; private set; }
    void Awake() {
        gems = new List<GameObject> { RedGem, GreenGem, YellowGem, PurpleGem, BlueGem };
        grid = new GemGrid(numberOfRows, numberOfColumns);

        RegenerateGrid();
    }

    private void PopulateGridWithRandomGems(Vector3 startingPosition, int numberOfRows, int numberOfColumns) {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
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
        stopWatch.Stop();

        TimeSpan ts = stopWatch.Elapsed;
        UnityEngine.Debug.Log($"Time taken to generate grid: {ts.ToString()}\n {grid.ToString()}");
    }
    private void DestroyGridContent()
    {
        foreach (var point in grid.GridPoints)
            if (point.Content != null)
                Destroy(point.Content);
    }

    public void RegenerateGrid()
    {
        DestroyGridContent();
        grid = new GemGrid(numberOfRows, numberOfColumns);
        explodeGridPointsCoroutine = ExplodeAdjacentNeighbors(explodingCheckRate);
        PopulateGridWithRandomGems(startingPosition, numberOfRows, numberOfColumns);
        StartCoroutine(explodeGridPointsCoroutine);
    }

    public IEnumerator ExplodeAdjacentNeighbors(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        var gridPointsToExplode = new List<GridPoint>();

        foreach (var point in grid.GridPoints)
        {
            var topNeighbours = GetValidExplodingNeighbours(point, grid.GetTopNeighbour);
            var bottomNeighbours = GetValidExplodingNeighbours(point, grid.GetBottomNeighour);
            var leftNeighbours = GetValidExplodingNeighbours(point, grid.GetLeftNeighbour);
            var rightNeighbours = GetValidExplodingNeighbours(point, grid.GetRightNeighbour);
            gridPointsToExplode.AddRange(topNeighbours);
            gridPointsToExplode.AddRange(bottomNeighbours);
            gridPointsToExplode.AddRange(leftNeighbours);
            gridPointsToExplode.AddRange(rightNeighbours);
        }
        gridPointsToExplode = gridPointsToExplode.GroupBy(x => x.Position).Select(x => x.First()).ToList();

        foreach (var gridPoint in gridPointsToExplode) {
            if (gridPoint.Content != null)
            {
                Destroy(gridPoint.Content);
                grid.UpdateGridPoint(gridPoint.Position);
            }
        }
        if(gridPointsToExplode.Count > 0)
        {
            GridReady = false;
            PopulateEmptyGridSpots();
        } else
        {
            GridReady = true;
            UnityEngine.Debug.Log("Grid ready");
            StopCoroutine(explodeGridPointsCoroutine);
        }
        stopWatch.Stop();

        TimeSpan ts = stopWatch.Elapsed;
        UnityEngine.Debug.Log($"Time taken to explode neighbours: {ts.ToString()}");

    }

    private void PopulateEmptyGridSpots()
    {
        var rand = new System.Random();
        while (grid.HasEmptyContent())
        {
            foreach (var gridPoint in grid.GridPoints)
            {
                if (gridPoint.Content == null)
                {
                    var topNeighbour = grid.GetTopNeighbour(gridPoint);
                    if (topNeighbour != null && topNeighbour.Content != null)
                    {
                        grid.UpdateGridPoint(gridPoint.Position, topNeighbour.Content);
                        topNeighbour.Content.GetComponent<MoveScript>().MoveToPosition(gridPoint.Position, newGemMoveSpeed);
                        grid.UpdateGridPoint(topNeighbour.Position);
                    }
                    else if (topNeighbour == null)
                    {
                        var randomGem = gems[rand.Next(0, gems.Count)];
                        var gem = Instantiate(randomGem);
                        grid.UpdateGridPoint(gridPoint.Position, gem);
                        gem.GetComponent<MoveScript>().MoveToPosition(gridPoint.Position, newGemMoveSpeed);
                        gem.name = $"{randomGem.name}";
                    }
                }
            }
        }
        UnityEngine.Debug.Log("Exited looping");

    }


    private List<GridPoint> GetValidExplodingNeighbours(GridPoint point, Func<GridPoint, GridPoint> getNextNeighbour)
    {
        var explodingNeighbours = new List<GridPoint>();

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
            explodingNeighbours.AddRange(validNeighbours);

        return explodingNeighbours;
    }
}