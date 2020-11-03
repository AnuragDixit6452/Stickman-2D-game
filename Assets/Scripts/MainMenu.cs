using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject playsound;
    // Start is called before the first frame update
    public void PlayGame()
   {
//if(GameManager.soundOn){
	Destroy(Instantiate(playsound, new Vector3(0f, 0f, 0f), Quaternion.identity), 0.5f);
//}
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
