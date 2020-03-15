using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {
    private Vector3 desiredPosition;
    private float movementSpeed;
    public bool IsMoving { get; private set; }
    private bool IsShrinking;

    public void MoveToPosition(Vector3 position, float speed = 1.0f) {
        desiredPosition = position;
        movementSpeed = speed;
    }

    public void ExpodeAndShrink()
    {
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.AddExplosionForce(5f, rb.gameObject.transform.position, 5f, 5f);
        IsShrinking = true;
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
            transform.localScale = new Vector3(transform.localScale.x - 0.03f, transform.localScale.y - 0.03f, transform.localScale.z - 0.03f);
            yield return new WaitForSeconds(.05f);
            if (transform.localScale.x <= 0 || transform.localScale.y <= 0 || transform.localScale.z <= 0)
            {
                IsShrinking = false;
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }
}