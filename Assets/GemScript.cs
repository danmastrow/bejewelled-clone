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
    public GameObject background;
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    public void Hover()
    {
        if(background != null)
        {
            background.SetActive(true);
        }
    }
    public void Unhover()
    {
        if (background != null)
        {
            background.SetActive(false);
        }
    }
}