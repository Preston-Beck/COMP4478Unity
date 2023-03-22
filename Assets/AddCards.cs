using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddCards : MonoBehaviour
{
    [SerializeField]
    private GameObject CardBtn;

    [SerializeField]
    private GameObject GameOverScreen;

    [SerializeField]
    private Sprite backImage;

    [SerializeField]
    private Sprite frontImage;

    [SerializeField]
    private List<Sprite> frontImages = new List<Sprite>();

    private List<int> assignImages = new List<int>();

    private List<int> listOfInts = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};

    private List<Button> cards = new List<Button>();


    private bool firstGuess, secondGuess;
    private string cardName1, cardName2;
    private int countGuesses, matches = 0;
    private int gameOver = 8;
    private int index1, index2;

    public Text guessText;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 16; i++)
        {
            GameObject card = Instantiate(CardBtn);
            card.name = "" + i;
            card.transform.SetParent(transform, false);
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("CardButton");

        for (int i = 0; i < items.Length; i++)
        {
            cards.Add(items[i].GetComponent<Button>());
            cards[i].image.sprite = backImage;
            cards[i].onClick.AddListener(() => ClickCard());
        }

        GenerateRandomCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickCard()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;
            index1 = int.Parse(name) - 1;
            cards[index1].image.sprite = frontImages[assignImages[index1]];
            cardName1 = frontImages[assignImages[index1]].name;
            cards[index1].interactable = false;
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            index2 = int.Parse(name) - 1;
            cards[index2].image.sprite = frontImages[assignImages[index2]];
            cardName2 = frontImages[assignImages[index2]].name;
            countGuesses++;
            StartCoroutine(WaitCoroutine());
        }
        
        
    }

    public void GenerateRandomCards()
    {
        for (int i = 0; i < 16; i++)
        {
            int random = Random.Range(0, listOfInts.Count);
            assignImages.Add(listOfInts[random]);
            listOfInts.RemoveAt(random);
        }
    }

    public void IsGameOver()
    {
        if(gameOver == matches)
        {
            EndScreen(countGuesses);
        }
    }

    IEnumerator WaitCoroutine()
    {
        //yield on a new YieldInstruction that waits for 1.5 seconds.
        yield return new WaitForSeconds(1.5f);
        if (cardName1 == cardName2)
        {
            cards[index1].interactable = false;
            cards[index2].interactable = false;
            matches++;
            IsGameOver();
        }
        else
        {
            cards[index1].interactable = true;
            cards[index1].image.sprite = backImage;
            cards[index2].image.sprite = backImage;
        }
        firstGuess = false;
        secondGuess = false;
    }

    public void EndScreen(int guesses)
    {
        GameOverScreen.SetActive(true);
        guessText.text = "Total Guesses: " + guesses.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
