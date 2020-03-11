using System.Collections.Generic;
using UnityEngine;

public class GridPoint {
    public Vector2 Position { get; private set; }
    public GameObject Content { get; private set; }
    public GridPoint(Vector2 pos, GameObject content = null) {
        Position = pos;
        Content = content;
    }
}