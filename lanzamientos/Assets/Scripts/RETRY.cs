using UnityEngine;
using System.Collections;

public class RETRY : MonoBehaviour {

    public void retry()
    {
        FindObjectOfType<objetivo>().cargarEsteNivel();
    }
}
