using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {
    private const int fallingGemPhysicsLayer = 8;
    private Vector3 desiredPosition;
    private BoxCollider boxCollider;
    private float movementSpeed;
    private System.Random rand;
    public bool IsMoving { get; private set; }
    private bool IsShrinking;

    private void Awake()
    {
        boxCollider = transform.GetComponent<BoxCollider>();
        rand = new System.Random();
    }
    public void MoveToPosition(Vector3 position, float speed = 1.0f) {
        desiredPosition = position;
        movementSpeed = speed;
    }

    public void ExpodeAndShrink()
    {
        var randX = rand.Next(-100, 100);
        var randY = rand.Next(-100, 100);
        var randZ = rand.Next(-100, 100);
        desiredPosition = new Vector3(randX, randY, randZ);
        movementSpeed = 0.1f;
        IsShrinking = true;
        gameObject.layer = fallingGemPhysicsLayer;
        StartCoroutine("Fade");
    }

    private void Update() {
        if (transform.position != desiredPosition) {
            IsMoving = true;
            var newPosForFrame = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * movementSpeed);
            transform.position = newPosForFrame;
        } else {
            IsMoving = false;
        }
    }

    IEnumerator Fade()
    {
        while (IsShrinking)
        {
            if (transform.localScale.x - 0.03f <= 0 || transform.localScale.y - 0.03 <= 0 || transform.localScale.z - 0.03 <= 0)
            {
                IsShrinking = false;
                StopAllCoroutines();
                Destroy(gameObject);
            }
            var newSize = new Vector3(transform.localScale.x - 0.03f, transform.localScale.y - 0.03f, transform.localScale.z - 0.03f);
            boxCollider.size = newSize;
            transform.localScale = newSize;
            yield return new WaitForSeconds(.05f);
        }
    }
}