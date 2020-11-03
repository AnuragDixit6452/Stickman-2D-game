using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller Aim is managed is StickBasic2.
public class GunScriptController : MonoBehaviour
{
    public SpriteRenderer weapongunGameObject;
    public GameObject projectile;
    public Transform shotPointObj;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public float offsetAfterFlipping;
    public bool flippedBool;
    public bool shootBool = false;

    void Update()
    {
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
        shootProjectile();
    }

    public void shootProjectile()
    {
        if (timeBtwShots <= 0 && shootBool)
        {
            shootBool = false;
            if (flippedBool)
                Instantiate(projectile, new Vector3(shotPointObj.position.x, shotPointObj.position.y + offsetAfterFlipping, shotPointObj.position.z), transform.rotation);
            else
                Instantiate(projectile, shotPointObj.position, transform.rotation);

            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            shootBool = false;
            timeBtwShots -= Time.deltaTime;
        }
    }
}
