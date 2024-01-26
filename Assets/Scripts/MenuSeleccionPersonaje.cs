using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSeleccionPersonaje : MonoBehaviour
{
    
    public GameObject[] playerObjects;
    public int selectedCharacter = 0;

    private string selectedCharacterDataName = "SelectedCharacter";

    void Start()
    {

        HideAllCharacters();

        selectedCharacter = PlayerPrefs.GetInt(selectedCharacterDataName, 0);

        playerObjects[selectedCharacter].SetActive(true);
   
    }

    //Ocultar personajes
    private void HideAllCharacters()
    {
        foreach (GameObject g in playerObjects)
        {
            g.SetActive(false);
        }
    }

    //Menu
    public void Next()
{
    playerObjects[selectedCharacter].SetActive(false);
    selectedCharacter++;
    if (selectedCharacter >= playerObjects.Length)
    {
        selectedCharacter = 0;
    }
    playerObjects[selectedCharacter].SetActive(true);
}

public void Previous()
{
    playerObjects[selectedCharacter].SetActive(false);
    selectedCharacter--;
    if (selectedCharacter < 0)
    {
        selectedCharacter = playerObjects.Length - 1;
    }
    playerObjects[selectedCharacter].SetActive(true);
}

    public void Play()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, selectedCharacter);
        SceneManager.LoadScene("SC Demo Scene - Village Props");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");

    }    
    
    public void Salir()
    {
        SceneManager.LoadScene("Credits");
    }

}