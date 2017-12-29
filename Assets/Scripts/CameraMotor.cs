using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    public Transform lookAt;
    public Vector3 offset = new Vector3(0, 5.0f, -10f);
    public Vector3 rotation = new Vector3(35.0f, 0.0f, 0.0f);

    public bool IsMoving { set; get; }

    private void LateUpdate()
    {
        if (!IsMoving)
        {
            return;
        }
        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.1f);
    }

}
