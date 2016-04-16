using UnityEngine;
using System.Collections;

public class iniciarPosAzar : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        transform.position = new Vector2(-3 + Random.value * 6, -3 + Random.value * 6);
    }
}