using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera myCamera;
    public float smoothSpeed = 0.125f;
    public Transform target;
    public Vector3 offset;
    public Vector3 dialog_offset;
    
    private const float DIALOGUE_FOV = 40f;
    private const float FOV_SMOOTH_SPEED = 3;


    private void Start()
    {
        myCamera = this.gameObject.GetComponent<Camera>();

    }

    private void FixedUpdate()
    {
        if (GameManagerScript.instance.isNormalLevel)
        {
            Vector3 _desiredPosition = target.position + offset;
            Vector3 _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = _smoothedPosition;
        }
        else {
            Vector3 _desiredPosition = target.position + dialog_offset;
            Vector3 _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = _smoothedPosition;

            if (GameManagerScript.instance.playerIsTalking) {
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, DIALOGUE_FOV, FOV_SMOOTH_SPEED * Time.deltaTime);
            }

        }

    }
}
