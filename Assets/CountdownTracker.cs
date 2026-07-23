using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountdownTracker : MonoBehaviour
{
    [SerializeField]
    Text countdownText;
    float countdown = 10f;
    [SerializeField]
    Image loseScreen;
    public static CountdownTracker Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0) countdown = 0;
        countdownText.text = $"{Mathf.FloorToInt(countdown)}!";
        if (countdown <= 0f)
        {
            loseScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void TriggerLoss()
    {
        countdown = 0;
    }
    public void ResetCountdown()
    {
        countdown = 10f;
        PartnerRandomizer.instance.RandomizePartnerPlacement();
    }
}
