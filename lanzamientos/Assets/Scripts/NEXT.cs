using UnityEngine;
using System.Collections;

public class NEXT : MonoBehaviour {

    public void next()
    {
        FindObjectOfType<objetivo>().cargarSiguienteNivel();
    }
}
