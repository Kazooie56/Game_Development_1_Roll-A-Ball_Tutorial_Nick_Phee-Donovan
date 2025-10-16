using UnityEngine;
using TMPro;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text versionText;

    void Start()
    {
        versionText.text = $"Version {Application.version}";
    }
}
