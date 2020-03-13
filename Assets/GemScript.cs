using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemColor {
    Red,
    Green,
    Blue,
    Purple,
    Yellow
}
public class GemScript : MonoBehaviour {

    public GemColor Color;
    public Shader gemShader;
    public Shader outlineShader;
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    public void Hover()
    {
        rend.material.shader = outlineShader;
    }
    public void Unhover()
    {
        rend.material.shader = gemShader;
    }
}