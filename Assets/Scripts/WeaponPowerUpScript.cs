using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerUpScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 10)
            Destroy(this.gameObject);
    }
}
