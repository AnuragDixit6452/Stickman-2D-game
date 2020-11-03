using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCollision : MonoBehaviour
{
    List<Collider2D> col;
    void Start()
    {
        col = new List<Collider2D>();
        Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();
        col.AddRange(colliders);

        for (int i = 0; i < col.Count; i++)
        {
            for (int k = 0; k < col.Count; k++)
            {
                Physics2D.IgnoreCollision(col[i], col[k]);
            }
        }
    }
}
