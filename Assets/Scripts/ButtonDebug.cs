using UnityEngine;
using UnityEngine.UI;

public class ButtonDebug : MonoBehaviour
{
    void Start()
    {
        Button[] allButtons = FindObjectsOfType<Button>();
        Debug.Log("Checking buttons in scene...");

        foreach (Button btn in allButtons)
        {
            string name = btn.name;
            bool interactable = btn.interactable;
            bool blockedByCanvasGroup = false;

            // Check parent CanvasGroups
            CanvasGroup cg = btn.GetComponentInParent<CanvasGroup>();
            if (cg != null && (!cg.interactable || cg.blocksRaycasts))
            {
                blockedByCanvasGroup = true;
            }

            string reason = interactable ? "Interactable" : "Not interactable";
            if (blockedByCanvasGroup) reason += " (Blocked by CanvasGroup)";

            Debug.Log($"Button '{name}': {reason}");
        }

        Debug.Log($"Time.timeScale = {Time.timeScale}");
    }
}
