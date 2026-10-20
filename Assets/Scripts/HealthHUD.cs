using UnityEngine;
using TMPro;

public class HealthHUD : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TMP_Text healthText;

    void Update()
    {
        healthText.text = "Health: " + playerHealth.health;
    }
}
