using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // CameraPivot
    public Transform player; // The rolling sphere
    public float distance = 8f;
    public float height = 3f;

    [Header("Smoothing")]
    public float followSmooth = 10f;
    public float snapDuration = 0.5f; // how long a 40° turn takes

    [Header("Wall Collision")]
    public float wallCheckRadius = 0.5f;
    public LayerMask collisionMask;

    private float targetYaw = 0f;
    private float currentYaw = 0f;
    private Vector3 currentVelocity;

    void Update()
    {
        // Update target pivot position (follow player but ignore rotation)
        target.position = player.position + Vector3.up * 1.5f;

        // Handle Q/E snapping input
        if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(SnapYaw(-40f));
        if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(SnapYaw(40f));
    }

    void LateUpdate()
    {
        // Smoothly interpolate yaw angle
        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * (180f / snapDuration));

        // Compute desired camera position (pivot around player)
        Vector3 offset = Quaternion.Euler(0, currentYaw, 0) * Vector3.back * distance + Vector3.up * height;
        Vector3 desiredPos = target.position + offset;

        // Handle wall sliding
        desiredPos = AdjustForWalls(target.position, desiredPos);

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followSmooth);

        // Always look at player
        transform.LookAt(target.position);
    }

    IEnumerator SnapYaw(float delta)
    {
        targetYaw += delta;
        yield return null; // optional pause for smoothness, handled by Lerp
    }

    Vector3 AdjustForWalls(Vector3 targetPos, Vector3 camPos)
    {
        Vector3 dir = camPos - targetPos;
        float dist = dir.magnitude;

        if (Physics.SphereCast(targetPos, wallCheckRadius, dir.normalized, out RaycastHit hit, dist, collisionMask))
        {
            // Slide camera along wall surface instead of zooming in
            Vector3 hitNormal = hit.normal;
            Vector3 slideDir = Vector3.ProjectOnPlane(dir.normalized, hitNormal);
            camPos = hit.point + slideDir * 0.5f;
        }

        return camPos;
    }
}





//public class CameraController : MonoBehaviour
//{
//    public GameObject player;
//    private Vector3 offset;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        offset = transform.position - player.transform.position;
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {
//        transform.position = player.transform.position + offset;
//    }
//}
