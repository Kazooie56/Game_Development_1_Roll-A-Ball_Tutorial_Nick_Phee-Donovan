using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    // HEALTH SCRIPT WILL BE HANDLING ALL UI STUFF UNTIL THIS PRJECT IS OVER

    private RawImage[] healthIcons;
    public GameManager gameManager;

    [SerializeField] private GameObject healthIconsParent;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private float uncollectedItemTransparency = 0.5f;              // 50% transparency
    [SerializeField] private float visibleDuration = 2f;                            // how long the screen stays visible when taking damage

    private float hideHealthTimer;

    private void Awake()
    {
        healthIcons = healthIconsParent.GetComponentsInChildren<RawImage>(true);
        System.Array.Reverse(healthIcons);
    }


    void Start()
    {

        healthBarUI.SetActive(true);
        hideHealthTimer = Time.time + visibleDuration;  // this makes it visible at the beginning like it's supposed to
    }

    void Update()
    {
        if (gameManager.isPaused)
            return;   // skip hiding logic entirely when paused
        // Hide health bar
        if (healthBarUI.activeSelf && Time.time > hideHealthTimer)        // Hide after timer runs out
            healthBarUI.SetActive(false);
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
}
// eventually make it so that it shows up for 3.5 seconds when hit, and forever when down to 2 health or less