using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    private RawImage[] healthIcons;

    [SerializeField] private GameObject healthIconsParent;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private float fadedAlpha = 0.5f;                // 50% transparency
    [SerializeField] private float visibleDuration = 2f;            // how long the screen stays visible when taking damage
    private float hideTimer;

    void Start()
    {
        healthIcons = healthIconsParent.GetComponentsInChildren<RawImage>(true);
        System.Array.Reverse(healthIcons);

        healthBarUI.SetActive(true);
        hideTimer = Time.time + visibleDuration;
    }

    void Update()
    {
        if (healthBarUI.activeSelf && Time.time > hideTimer)        // Hide after timer runs out
            healthBarUI.SetActive(false);
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth) // this affects pictures only
    {
        healthBarUI.SetActive(true);
        hideTimer = Time.time + visibleDuration;

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < maxHealth)
            {
                healthIcons[i].gameObject.SetActive(true); // only hide locked ones
                Color c = healthIcons[i].color;
                c.a = (i < currentHealth) ? 1f : fadedAlpha;
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