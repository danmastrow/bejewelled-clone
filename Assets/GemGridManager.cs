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

    private Grid Grid;
    void Awake() {
        GenerateGrid(startingPosition, numberOfRows, numberOfColumns);
    }

    private void GenerateGrid(Vector3 startingPosition, int numberOfRows, int numberOfColumns) {
        var gems = new List<GameObject> { RedGem, GreenGem, YellowGem, PurpleGem, BlueGem };
        var grid = new Grid(numberOfRows, numberOfColumns);
        var rand = new System.Random();

        foreach (var point in grid.GridPoints) {
            var randomGem = gems[rand.Next(0, gems.Count)];
            var gem = Instantiate(randomGem);
            grid.UpdatePointValue(point.Key, gem);
            startingPosition = new Vector3(point.Key.x, point.Key.y, startingPosition.z);
            gem.GetComponent<MoveScript>().MoveToPosition(startingPosition);
        }
        // TODO: REFACTOR: Average guess that this finds a good enough distance for the camera to see everything
        var centerGemPosition = new Vector3(numberOfRows / 2, numberOfColumns / 2, -(numberOfRows / 2 + numberOfColumns / 2) - 1);
        Camera.main.GetComponent<MoveScript>().MoveToPosition(centerGemPosition);

    }
}