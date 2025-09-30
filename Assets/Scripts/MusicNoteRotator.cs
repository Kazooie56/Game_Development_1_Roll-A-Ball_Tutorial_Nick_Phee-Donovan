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


//public Transform MainCamera;
//public class MusicNoteRotater : MonoBehaviour
//{
//    void Update()
//    {
//        transform.Rotate(new Vector3(0, 324, 0) * Time.deltaTime); // banjo Kazooie runs at 20 fps, might be 30 or 60, it has 18 frames in the spinning animation, so just check and see what looks right
//    }
//}

