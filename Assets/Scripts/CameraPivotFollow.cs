using UnityEngine;

public class CameraPivotFollow : MonoBehaviour
{
    public Transform player;
    void LateUpdate()
    {
        transform.position = player.position + Vector3.up * 1.5f;
        // no rotation = no spin from the rolling sphere
    }
}
