using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://stackoverflow.com/questions/42658013/slowly-rotating-towards-angle-in-unity

public class Popup : MonoBehaviour {

    Quaternion startRotation, endRotation;
    float rotationProgress = -1;
    bool finishDelay, run;
    float startTime;

    public float delay; // Delay in milliseconds

    void Start() {
        startTime = Time.time;
    }

    void Update() {
        if (Time.time - startTime >= delay) {
            finishDelay = true;
        }

        if (finishDelay && !run) {
            startRotation = transform.rotation;
            endRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            rotationProgress = 0;
            run = true;
        }

        if (rotationProgress < 1 && rotationProgress >= 0) {
            rotationProgress += Time.deltaTime * 2f;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
    }
}
