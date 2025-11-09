using UnityEngine;

public class PausarJuego : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject btnPausa;
    [SerializeField] private GameObject verticalLayout;
    [SerializeField] private bool juegoPausado = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }
    public void Reanudar()
    {
        menuPausa.SetActive(false);
        verticalLayout.SetActive(false);
        btnPausa.SetActive(true);
        Time.timeScale = 1;
        juegoPausado = false;
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        verticalLayout.SetActive(true);
        btnPausa.SetActive(false);
        Time.timeScale = 0;
        juegoPausado = true;
    }
}
