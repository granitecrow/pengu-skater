using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;

    public static GameManager Instance { set; get; }

    private bool isGameStarted = false;
    private PlayerMotor motor;
    public bool isDead { set; get; }

    // UI and UI fields
    public Text scoreText;
    public Text coinText;
    public Text modifierText;
    private float score;
    private float coinScore;
    private float modifierScore = 1.0f;
    private int lastScore;

    private void Awake()
    {
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        Instance = this;
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");

    }
    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
        }

        if (isGameStarted && !isDead)
        {
            // increase score
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");

    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

}
