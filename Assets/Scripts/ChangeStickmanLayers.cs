using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStickmanLayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("StickMan1") == null)
        {
            this.gameObject.name = "StickMan1";
        }
        else if (GameObject.Find("StickMan2") == null)
        {
            this.gameObject.name = "StickMan2";
        }
        else if (GameObject.Find("StickMan3") == null)
        {
            this.gameObject.name = "StickMan3";
        }
        else if (GameObject.Find("StickMan4") == null)
        {
            this.gameObject.name = "StickMan4";
        }


        if (this.gameObject.name == "StickMan1")
        {
            SetLayerRecursively(this.gameObject, 9);
        }
        else if (this.gameObject.name == "StickMan2")
        {
            SetLayerRecursively(this.gameObject, 10);
        }
        else if (this.gameObject.name == "StickMan3")
        {
            SetLayerRecursively(this.gameObject, 11);
        }
        else if (this.gameObject.name == "StickMan4")
        {
            SetLayerRecursively(this.gameObject, 12);
        }
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
