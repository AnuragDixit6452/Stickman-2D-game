using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool soundOn = true;
    public GameObject winTextObj;
    public GameObject waitTextObj;
    public static int noOfWinsP1;
    public Text noOfWinsP1Text;
    public static int noOfWinsP2;
    public Text noOfWinsP2Text;
    public static int matchesPlayed;
    public GameObject restartBtn;
    public GameObject quitBtn;
    public bool gameOver;

    Text winText;
    Text waitText;
    public GameObject stickMan1;
    public GameObject stickMan2;

    StickBasic stickBasicScript1;
    StickBasic2 stickBasicScript2;

    public GameObject gunPowerUpPrefabObj;

    bool alreadyWon;

    void Awake()
    {
        stickBasicScript1 = stickMan1.GetComponent<StickBasic>();
        stickBasicScript2 = stickMan2.GetComponent<StickBasic2>();
        winText = winTextObj.GetComponent<Text>();
        waitText = waitTextObj.GetComponent<Text>();
        winTextObj.SetActive(false);
        waitTextObj.SetActive(false);
        restartBtn.SetActive(false);
        quitBtn.SetActive(false);
        alreadyWon = false;
        gameOver = false;
    }

    void Start()
    {
        updateScores();
        if (SceneManager.GetActiveScene().buildIndex == 1)
            StartCoroutine(waitForSecondsAndLoadRandomScene(3));
        else if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartCoroutine(instantiatePowerUp(3));
        }
    }

    void Update()
    {
        if (!stickBasicScript1.getPlayerCanMoveBool() && !alreadyWon)
        {
            alreadyWon = true;
            stickBasicScript1.gunOn = false;
            matchController(false);
            StartCoroutine(waitForSecondsAndLoadRandomScene(2));
        }
        if (!stickBasicScript2.getPlayerCanMoveBool() && !alreadyWon)
        {
            alreadyWon = true;
            stickBasicScript2.gunOn = false;
            matchController(true);
            StartCoroutine(waitForSecondsAndLoadRandomScene(2));
        }
    }

    public void updateScores()
    {
        noOfWinsP1Text.text = "Red Player - Score: " + noOfWinsP1;
        noOfWinsP2Text.text = "Blue Player - Score: " + noOfWinsP2;
    }

    public void matchController(bool p1Won)
    {
        GameManager.matchesPlayed++;
        if (GameManager.matchesPlayed < 5)
        {
            if (p1Won)
                GameManager.noOfWinsP1++;
            else
                GameManager.noOfWinsP2++;
            updateScores();
        }
        else
        {

            if (p1Won)
                GameManager.noOfWinsP1++;
            else
                GameManager.noOfWinsP2++;
            updateScores();
            gameOver = true;
            if (GameManager.noOfWinsP1 > GameManager.noOfWinsP2)
            {
                winText.text = "GameOver\nRed Player Won!";
                winTextObj.SetActive(true);
                restartBtn.SetActive(true);
                quitBtn.SetActive(true);
            }
            else
            {
                winText.text = "GameOver\nBlue Player Won!";
                winTextObj.SetActive(true);
                restartBtn.SetActive(true);
                quitBtn.SetActive(true);
            }

        }
    }

    IEnumerator waitForSecondsAndLoadRandomScene(int time)
    {
        if (!gameOver)
        {
            waitTextObj.SetActive(true);
            for (int i = time; i >= 0; i--)
            {
                waitText.text = "Please Wait... " + i;
                yield return new WaitForSeconds(1);
            }
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int newIndex = currentIndex;
            while (newIndex == currentIndex)
                newIndex = Random.Range(2, 6);
            SceneManager.LoadScene(newIndex);
        }
    }

    IEnumerator instantiatePowerUp(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(gunPowerUpPrefabObj, new Vector3(0f, 5.5f, 0f), Quaternion.identity);
    }

    public void restartGame()
    {
        noOfWinsP1 = 0;
        noOfWinsP2 = 0;
        matchesPlayed = 0;
        SceneManager.LoadScene(0);
    }

    public void quitGame(){
        Application.Quit();
    }
}
