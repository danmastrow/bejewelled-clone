using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private GemGridManager gemGridManager;
    private Ray ray;
    private RaycastHit hit;
    private GameObject hoveredObject;
    private GameObject selectedObject;
    private void Awake()
    {
        gemGridManager = GetComponent<GemGridManager>();
    }

    void Update()
    {
        var unhover = true;
        if (gemGridManager.GridReady)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Gem")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectedObject == null)
                        {
                            selectedObject = hit.collider.gameObject;
                        } else if (selectedObject != hit.collider.gameObject)
                        {
                            var swapSucceeded = gemGridManager.TrySwap(selectedObject, hit.collider.gameObject);
                            if (swapSucceeded)
                            {
                                hoveredObject.GetComponent<GemScript>().Unhover();
                                selectedObject.GetComponent<GemScript>().Unhover();
                                selectedObject = null;
                                hoveredObject = null;
                            }
                        }
                    }
                    if (hoveredObject != null && hoveredObject != hit.collider.gameObject)
                    {
                        hoveredObject.GetComponent<GemScript>().Unhover();
                    }
                    hoveredObject = hit.collider.gameObject;
                    hoveredObject.GetComponent<GemScript>().Hover();
                    unhover = false;
                }
            }
            if (hoveredObject != null && unhover)
            {
                hoveredObject.GetComponent<GemScript>().Unhover();
            }
        }
    }
}
