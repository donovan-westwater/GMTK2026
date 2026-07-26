using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountdownTracker : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI countdownText;
    float countdown = 10f;
    [SerializeField]
    Image loseScreen;
    [SerializeField]
    Image introScreen;
    [SerializeField]
    Image winScreen;
    public static CountdownTracker Instance;
    public AudioSource playerSource;
    private AudioClip mainSong;
    [HideInInspector]
    public bool beganGame = false;
    private bool loseGame = false;
    public bool winGame = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        mainSong = Resources.Load<AudioClip>("HallOfTheMountianKing-Trimmed");
        playerSource.clip = mainSong;
        playerSource.loop = true;
        Time.timeScale = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (!beganGame)
        {
            if (Input.GetKeyDown(KeyCode.E)) BeginGame();
            return;
        }
        if (!loseGame)
        {
            countdown -= Time.deltaTime;
            if (countdown < 0) countdown = 0;
            countdownText.text = $"{Mathf.FloorToInt(countdown)}!";
        }
        if (countdown <= 0f)
        {
            countdownText.transform.parent.gameObject.SetActive(false);
            loseScreen.gameObject.SetActive(true);
            playerSource.gameObject.GetComponent<FirstPersonController>().enabled= false;
            Cursor.lockState = CursorLockMode.None;
        }

    }
    public void Reload()
    {
        TagTarget.ClearActiveTargets();
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
    public void BeginGame()
    {
        beganGame = true;
        playerSource.Play();
        Time.timeScale = 1f;
        countdownText.transform.parent.gameObject.SetActive(true);
        playerSource.gameObject.GetComponent<FirstPersonController>().lockCursor = true;
        introScreen.gameObject.SetActive(false);
    }
    public void EndGame()
    {
        playerSource.Stop();
        countdownText.transform.parent.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
        playerSource.gameObject.GetComponent<FirstPersonController>().lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
