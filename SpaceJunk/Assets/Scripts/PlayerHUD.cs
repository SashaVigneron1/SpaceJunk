using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;
    private int score = 0;
    private Slider fuelSlider;
    [SerializeField] private int rechargeSpeed;
    private float currentFuel = 100;
    private float maxFuel = 100;

    private JunkCollector junkCollector = null;
    private PlayerMagnet playerMagnet = null;
    void Start()
    {
        gameObject.SetActive(false);

        if(fuelSlider == null)
            fuelSlider = GetComponentInChildren<Slider>();
        maxFuel = fuelSlider.maxValue;

        if (scoreText == null)
            scoreText = gameObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        //Charge Fuel
        if(currentFuel <= maxFuel)
        {
            currentFuel += Time.deltaTime * rechargeSpeed;
        }
        //update slider & text
        if (junkCollector != null)
            score = junkCollector.Score;
        if(scoreText != null)
            scoreText.text = score.ToString();
        if(fuelSlider != null)
            fuelSlider.value = playerMagnet.Charge;
    }
    public void SetColor(Color color)
    {
        Color newColor = new Color(color.r, color.g, color.b, 1);
        fuelSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = newColor;
        fuelSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = newColor;

        scoreText.color = newColor;
        gameObject.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().color = newColor;
    }
    public void SetScore(int s)
    {
        score = s;
    }

    public void SetPlayer(int playerIndex)
    {
        foreach (JunkCollector c in FindObjectsOfType<JunkCollector>())
        {
            if (c.playerCollectorIndex == playerIndex)
            {
                junkCollector = c;
                break;
            }
        }
        
        foreach (PlayerMagnet c in FindObjectsOfType<PlayerMagnet>())
        {
            if (c.playerIndex == playerIndex)
            {
                playerMagnet = c;
                break;
            }
        }
    }
}