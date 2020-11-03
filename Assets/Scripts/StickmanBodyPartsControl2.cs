using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StickmanBodyPartsControl2 : MonoBehaviour
{

    StickBasic2 stickBasicScriptObj2;

    public GameObject healthSquareObj;
    HealthBar healthBar;

    void Start()
    {
        GameObject temp = gameObject.transform.parent.gameObject;
        stickBasicScriptObj2 = temp.GetComponent<StickBasic2>();
        healthBar = healthSquareObj.GetComponent<HealthBar>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            stickBasicScriptObj2.resetJumpVal();
            stickBasicScriptObj2.setInAirToFalse();
        }
        if (other.gameObject.tag == "HealthDown")
        {
            healthBar.reduceCurrentHealth(5f);
            healthBar.reduceHealthBool = true;
        }
        if (other.gameObject.tag == "Respawn")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (other.gameObject.tag == "InstantDeath")
        {
            stickBasicScriptObj2.setPlayerCanMoveBool(false);
            stickBasicScriptObj2.straight = false;
        }
        if (other.gameObject.tag == "Bullet")
        {
            healthBar.reduceCurrentHealth(2f);
            healthBar.reduceHealthBool = true;
        }
        if (other.gameObject.tag == "WeaponPowerUp")
        {
            stickBasicScriptObj2.gunOn = true;
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "WeaponSword")
        {
            healthBar.reduceCurrentHealth(1f);
            healthBar.reduceHealthBool = true;
        }
        if (other.gameObject.tag == "InstantDeath")
        {
            stickBasicScriptObj2.setPlayerCanMoveBool(false);
            stickBasicScriptObj2.straight = false;
        }
    }

}
