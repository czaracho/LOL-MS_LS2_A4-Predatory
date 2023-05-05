using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehaviour : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 10f;
    public float verticalSpeed = 8.0f;

    private Vector3 initialPosition;
    private float elapsedTime = 0f;

    void Start() {
        initialPosition = transform.position;
        amplitude = Random.Range(amplitude, 1.5f);
        frequency = Random.Range(frequency, 15f);
        verticalSpeed = Random.Range(verticalSpeed, 5f);

    }

    void Update() {
        elapsedTime += Time.deltaTime;

        float offsetX = amplitude * Mathf.Cos(elapsedTime * frequency);
        float offsetY = amplitude * Mathf.Sin(elapsedTime * verticalSpeed);
        float offsetZ = amplitude * Mathf.Sin(elapsedTime * frequency);

        Vector3 newPosition = new Vector3(initialPosition.x + offsetX, initialPosition.y + offsetY, initialPosition.z + offsetZ);

        transform.position = newPosition;
    }
}
