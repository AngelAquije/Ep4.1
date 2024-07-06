using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escenario : MonoBehaviour
{
    [SerializeField] private GameObject backG;
    [SerializeField] private GameObject backG1;
    [SerializeField] private GameObject backG2;
    [SerializeField] private GameObject Fondo;

    private void Start()
    {
        backG.SetActive(false);
        backG1.SetActive(false);
        backG2.SetActive(false);
        Fondo.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            backG.SetActive(true);
            backG1.SetActive(true);
            backG2.SetActive(true);
            Fondo.SetActive(false);
            Debug.Log("Listo");
        }
    }
}
