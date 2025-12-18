using UnityEngine;
using UnityEngine.InputSystem;

public class StrikePanel : MonoBehaviour
{
    [SerializeField] private GameObject strikePanel;
    [SerializeField] private GameObject imageOne;
    [SerializeField] private GameObject imageTwo;
    [SerializeField] private GameObject imageThree;
    
    [SerializeField] private GameData gameData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameData.strikes > 0)
        {
            strikePanel.SetActive(true);
            ShowImages(gameData.strikes);
        }
        else 
        {
            strikePanel.SetActive(false);
            HideImages();   
        }
         
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowImages(int amount)
    {
        imageOne.SetActive(amount >= 1);
        imageTwo.SetActive(amount >= 2);
        imageThree.SetActive(amount >= 3);
    }

    void HideImages()
    {
        imageOne.SetActive(false);
        imageTwo.SetActive(false);
        imageThree.SetActive(false);
    }
}
