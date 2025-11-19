using UnityEngine;
using UnityEngine.UI;

public class EmptyHoneycombScript : MonoBehaviour
{
    private RawImage[] emptyHoneycombPieceIcons;
    public PlayerController playerController;
    public HealthScript healthScript;

    [SerializeField] private GameObject emptyHoneycombIconsParent;
    [SerializeField] private GameObject emptyHoneycombUI;
    [SerializeField] private float uncollectedItemTransparency = 0.5f;              // 50% transparency
    [SerializeField] private float emptyHoneycombVisibleDuration = 0.5f;                 // how long the honeycombui stays visible

    private float hideEmptyHoneycombTimer;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emptyHoneycombPieceIcons = emptyHoneycombIconsParent.GetComponentsInChildren<RawImage>(true);
        emptyHoneycombUI.SetActive(false);
        hideEmptyHoneycombTimer = Time.time + emptyHoneycombVisibleDuration;  // this makes it visible at the beginning like it's supposed to

    }

    // Update is called once per frame
    void Update()
    {
        // Hide honeycomb UI
        if (emptyHoneycombUI.activeSelf && Time.time > hideEmptyHoneycombTimer)
            emptyHoneycombUI.SetActive(false);
    }

    public void UpdateEmptyHoneycombUI(int emptyHoneycombPieceCount)
    {
        emptyHoneycombUI.SetActive(true);
        hideEmptyHoneycombTimer = Time.time + emptyHoneycombVisibleDuration;

        for (int i = 0; i < emptyHoneycombPieceIcons.Length; i++)
        {
            Color c = emptyHoneycombPieceIcons[i].color;
            c.a = (i < emptyHoneycombPieceCount) ? 1f : uncollectedItemTransparency;
            emptyHoneycombPieceIcons[i].color = c;
        }

        if (emptyHoneycombPieceCount >= 6)
        {
            playerController.maxHealth = 6;
            playerController.currentHealth = 6;
            healthScript.UpdateHealthBar(playerController.currentHealth, playerController.maxHealth);
        }
    }
}
