using UnityEngine;
using TMPro; // TMP_Text
 
// Shows the speed boost icon and seconds left while the boost is on.
public class PowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerPowerUps powerUps;
    [SerializeField] private GameObject speedIndicator; // icon + time
    [SerializeField] private TMP_Text speedTimeText;

    [SerializeField] private GameObject jumpIndicator;
    [SerializeField] private TMP_Text jumpTimeText;

    [SerializeField] private GameObject scoreIndicator;
    [SerializeField] private TMP_Text scoreTimeText;
 
    private void Update()
    {
        float timeLeft = powerUps.SpeedTimeLeft;
 
        // Visible for the whole boost, hidden the rest of the time
        speedIndicator.SetActive(timeLeft > 0f);
 
        // CeilToInt rounds 6.2 up to 7, so we never show 0s too early
        speedTimeText.text = "Speed Boost:" + Mathf.CeilToInt(timeLeft) + "s";

         // Jump boost
        float jumpTimeLeft = powerUps.JumpTimeLeft;

        jumpIndicator.SetActive(jumpTimeLeft > 0f);

        jumpTimeText.text = "Jump Boost:" + Mathf.CeilToInt(jumpTimeLeft) + "s";

        // Score boost
        float scoreTimeLeft = powerUps.ScoreTimeLeft;

        scoreIndicator.SetActive(scoreTimeLeft > 0f);

        scoreTimeText.text = "Score Boost:" + Mathf.CeilToInt(scoreTimeLeft) + "s";

    }
}
