using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    public Transform target;
    public float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

	void Update () {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0f, 8f, 2f));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}
