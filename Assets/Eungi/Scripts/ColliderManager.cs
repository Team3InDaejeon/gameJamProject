using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public bool isCheckPlayer = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isCheckPlayer = true;
        }
    }
}
