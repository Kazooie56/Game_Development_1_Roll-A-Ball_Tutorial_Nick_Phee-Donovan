using UnityEngine;
public class NoteSpinner : MonoBehaviour
{
    public Transform cameraTransform; // assign main camera
    public float spinSpeed = 324f;    // degrees per second

    void Update()
    {
        if (cameraTransform == null) return;

        // 1. Align this object to the camera horizontally
        Vector3 lookEuler = transform.eulerAngles;
        lookEuler.y = cameraTransform.eulerAngles.y;
        transform.eulerAngles = lookEuler;

        // 2. Spin the child object around its local Y
        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}

