using UnityEngine;

public class StrikePanel : MonoBehaviour
{
    [SerializeField] private GameObject strikePanel;
    [SerializeField] private GameObject imageOne;
    [SerializeField] private GameObject imageTwo;
    [SerializeField] private GameObject imageThree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        strikePanel.SetActive(false);
        HideImages();   
    }

    // Update is called once per frame
    void Update()
    {
        if (!imageOne.activeSelf && !imageTwo.activeSelf && !imageThree.activeSelf)
        {
            strikePanel.SetActive(false);
        }
        else
        {
            strikePanel.SetActive(true);
        }
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
