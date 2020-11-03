using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StickmanBodyPartsControl : MonoBehaviour
{

    StickBasic stickBasicScriptObj1;

    public GameObject healthSquareObj;
    HealthBar healthBar;

    void Start()
    {
        GameObject temp = gameObject.transform.parent.gameObject;
        stickBasicScriptObj1 = temp.GetComponent<StickBasic>();
        healthBar = healthSquareObj.GetComponent<HealthBar>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            stickBasicScriptObj1.resetJumpVal();
            stickBasicScriptObj1.setInAirToFalse();
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
            stickBasicScriptObj1.setPlayerCanMoveBool(false);
            stickBasicScriptObj1.straight = false;
        }
        if (other.gameObject.tag == "Bullet")
        {
            healthBar.reduceCurrentHealth(2f);
            healthBar.reduceHealthBool = true;
        }
        if (other.gameObject.tag == "WeaponPowerUp")
        {
            stickBasicScriptObj1.gunOn = true;
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
            stickBasicScriptObj1.setPlayerCanMoveBool(false);
            stickBasicScriptObj1.straight = false;
        }
    }
}
