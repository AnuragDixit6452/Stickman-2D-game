using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject playsound;

    public float offset = 0;
    public SpriteRenderer weapongunGameObject;
    public GameObject projectile;
    public Transform shotPointObj;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public float offsetAfterFlipping;
    public bool flippedBool;

    void Awake()
    {
        flippedBool = false;
    }


    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        var tempZ = transform.eulerAngles.z;
        if (tempZ > 180)
        {
            weapongunGameObject.flipY = true;
            flippedBool = true;
        }
        else
        {
            weapongunGameObject.flipY = false;
            flippedBool = false;
        }

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {   
                Destroy(Instantiate(playsound, new Vector3(0f, 0f, 0f), Quaternion.identity), 0.5f);
                if (flippedBool)
                    Instantiate(projectile, new Vector3(shotPointObj.position.x, shotPointObj.position.y + offsetAfterFlipping, shotPointObj.position.z), transform.rotation);
                else
                    Instantiate(projectile, shotPointObj.position, transform.rotation);

                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
