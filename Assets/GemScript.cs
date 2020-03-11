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

    [SerializeField]
    private GemColor Color;

}