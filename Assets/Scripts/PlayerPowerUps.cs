using UnityEngine;
 
// Gives the player a speed boost for a limited time.
public class PlayerPowerUps : MonoBehaviour
{
    [SerializeField] private float boostMultiplier = 1.5f; // 50% faster
 
    // Seconds of boost left. The HUD reads it; only this script changes it.
    public float SpeedTimeLeft { get; private set; }
    public float JumpTimeLeft { get; private set; }
    public float ScoreTimeLeft { get; private set; }
 
    private PlayerMovement movement;
 
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
 
    // Called by the pickup. Collecting another boost restarts the timer.
    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        movement.SpeedMultiplier = boostMultiplier;
    }
    public void ActivateJumpBoost(float duration)
    {
        JumpTimeLeft = duration;
        movement.canDoubleJump = true;
    }
    public void ActivateScoreBoost(float duration)
    {
        ScoreTimeLeft = duration;
        ScoreManager.Instance.multiplier = 2;
    }

    private void Update()
    {
        //because many power ups can't do return alone orelse the other power ups wont work.
        // simple logic if greater then 0 time goes down, and if <=0 we reset that specific powerup so the whole system continues to work
        if (SpeedTimeLeft > 0f)//if the power up not over
        {
            SpeedTimeLeft -= Time.deltaTime;//power up time go down

            if (SpeedTimeLeft <= 0f) // power up over
            {
                SpeedTimeLeft = 0f;// no more time
                movement.SpeedMultiplier = 1f;// reset speed
            }
        }

        if (JumpTimeLeft > 0f)// same logic as other but double jump goes away
        {
            JumpTimeLeft -= Time.deltaTime;

            if (JumpTimeLeft <= 0f)
            {
                JumpTimeLeft = 0f;
                movement.canDoubleJump = false;
            }
        }
        
        if (ScoreTimeLeft > 0f)// same logic as other but multiplier resets to original value of 1
        {
            ScoreTimeLeft -= Time.deltaTime;

            if (ScoreTimeLeft <= 0f)
            {
                ScoreTimeLeft = 0f;
                ScoreManager.Instance.multiplier = 1;
            }
        }
    }
}
