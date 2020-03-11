using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {
    private Vector3 desiredPosition;
    private float movementSpeed;
    public bool IsMoving { get; private set; }

    public void MoveToPosition(Vector3 position, float speed = 1.0f) {
        desiredPosition = position;
        movementSpeed = speed;
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
}