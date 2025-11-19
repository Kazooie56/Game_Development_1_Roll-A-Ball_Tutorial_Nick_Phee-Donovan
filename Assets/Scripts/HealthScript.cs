using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    // HEALTH SCRIPT WILL BE HANDLING ALL UI STUFF UNTIL THIS PRJECT IS OVER


    private RawImage[] healthIcons;
    private RawImage[] emptyHoneycombPieceIcons;

    [SerializeField] private GameObject healthIconsParent;
    [SerializeField] private GameObject emptyHoneycombPieceIconsParent;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject emptyHoneycombPieceUI;
    [SerializeField] private float uncollectedItemTransparency = 0.5f;              // 50% transparency
    [SerializeField] private float visibleDuration = 2f;                            // how long the screen stays visible when taking damage
    [SerializeField] private float honeycombVisibleDuration = 0.5f;                 // how long the honeycombui stays visible

    private float hideHealthTimer;
    private float hideEmptyHoneycombTimer;

    private void Awake()
    {
        healthIcons = healthIconsParent.GetComponentsInChildren<RawImage>(true);
        System.Array.Reverse(healthIcons);
    }


    void Start()
    {
        emptyHoneycombPieceIcons = emptyHoneycombPieceIconsParent.GetComponentsInChildren<RawImage>(true);

        healthBarUI.SetActive(true);
        emptyHoneycombPieceUI.SetActive(false);
        hideHealthTimer = Time.time + visibleDuration;  // this makes it visible at the beginning like it's supposed to
    }

    void Update()
    {
        // Hide health bar
        if (healthBarUI.activeSelf && Time.time > hideHealthTimer)        // Hide after timer runs out
            healthBarUI.SetActive(false);

        // Hide honeycomb UI
        if (emptyHoneycombPieceUI.activeSelf && Time.time > hideEmptyHoneycombTimer)
            emptyHoneycombPieceUI.SetActive(false);
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth) // this affects pictures only
    {
        healthBarUI.SetActive(true);
        hideHealthTimer = Time.time + visibleDuration; // this is when you take damage

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < maxHealth)
            {
                healthIcons[i].gameObject.SetActive(true); // only hide locked ones
                Color c = healthIcons[i].color;
                c.a = (i < currentHealth) ? 1f : uncollectedItemTransparency;
                healthIcons[i].color = c;
            }
            else
            {
                healthIcons[i].gameObject.SetActive(false); // completely invisible if locked
            }
        }
    }

    public void UpdateEmptyHoneycombUI(int emptyHoneycombPieceCount)
    {
        emptyHoneycombPieceUI.SetActive(true);
        hideEmptyHoneycombTimer = Time.time + honeycombVisibleDuration;

        for (int i = 0; i < emptyHoneycombPieceIcons.Length; i++)
        {
            Color c = emptyHoneycombPieceIcons[i].color;
            c.a = (i < emptyHoneycombPieceCount) ? 1f : uncollectedItemTransparency;
            emptyHoneycombPieceIcons[i].color = c;
        }
    }
}
// eventually make it so that it shows up for 3.5 seconds when hit, and forever when down to 2 health or less