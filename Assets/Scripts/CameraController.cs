using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    [Header("Target")]          // This gives the Inspector a Header for a customizable section
    public Transform target;    // CameraPivot
    public Transform player;    // The rolling sphere
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
        if (Time.timeScale == 0f)
            return; // stop updating when paused

        bool CanSnapYaw(float delta) // check if we can rotate the camera and not have it blocked by a wall
        {
            Vector3 testDir = Quaternion.Euler(0, targetYaw + delta, 0) * Vector3.back;
            if (Physics.Raycast(target.position, testDir, distance, collisionMask))
                return false; // blocked by wall
            return true; // clear
        }

        // Update target pivot position (follow player but ignore rotation)
        target.position = player.position + Vector3.up * 1.5f;

        // Handle Q/E snapping input
        if (Input.GetKeyDown(KeyCode.Q) && CanSnapYaw(39f))
            StartCoroutine(SnapYaw(39f));

        if (Input.GetKeyDown(KeyCode.E) && CanSnapYaw(-39f))
            StartCoroutine(SnapYaw(-39f));
    }

    void LateUpdate()
    {
        // Smoothly interpolate yaw angle
        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * (180f / snapDuration)); // Yaw is a pilot term for how a plane can turn, a plane rotating left and right is Yaw, pitch is up and down, roll is doing starfox barrel rolls

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
            // Determine minimal rotation direction to avoid wall
            float leftAngle = FindClearAngle(targetPos, -1);
            float rightAngle = FindClearAngle(targetPos, 1);

            float rotateDir = leftAngle <= rightAngle ? -1f : 1f;

            // Rotate camera gradually toward the clear side
            targetYaw += rotateDir * Time.deltaTime * 60f; // adjust rotation speed as needed

            // Slide camera along the wall slightly
            Vector3 hitNormal = hit.normal;
            Vector3 slideDir = Vector3.ProjectOnPlane(dir.normalized, hitNormal);
            camPos = hit.point + slideDir * 0.5f;
        }

        return camPos;
    }

    // Finds how far (in degrees) the camera must rotate in a given direction to clear wall
    float FindClearAngle(Vector3 origin, float dirSign)
    {
        float testAngle = 0f;
        int maxSteps = 20;
        float step = 5f; // degrees per test

        for (int i = 0; i < maxSteps; i++)
        {
            testAngle += step * dirSign;
            Vector3 testDir = Quaternion.Euler(0, targetYaw + testAngle, 0) * Vector3.back;

            if (!Physics.Raycast(origin, testDir, distance, collisionMask))
                return Mathf.Abs(testAngle); // angle needed to clear
        }

        return 360f; // assume blocked if nothing clears within range
    }
}
