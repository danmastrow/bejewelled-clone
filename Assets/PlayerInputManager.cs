using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private GemGridManager gemGridManager;
    private Ray ray;
    private RaycastHit hit;
    //private GameObject hoveredObject;
    private GameObject selectedObject;
    private void Awake()
    {
        gemGridManager = GetComponent<GemGridManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<GemScript>().Unhover();
                selectedObject = null;
            }

        }
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
                            selectedObject.GetComponent<GemScript>().Hover();
                        }
                        else if (selectedObject != hit.collider.gameObject)
                        {
                            var swapSucceeded = gemGridManager.TrySwap(selectedObject, hit.collider.gameObject);
                            if (swapSucceeded)
                            {
                                selectedObject.GetComponent<GemScript>().Unhover();
                                selectedObject = null;
                            } else
                            {
                                Debug.LogWarning("TODO: Handle Swap failed");
                            }
                        }
                    }
                }
            }
        }
    }
}
