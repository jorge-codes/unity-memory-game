using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    [Header("Visual Settings")]
    public GameObject cardPrefab;
    public Transform deckParent;
    public CardIcons cardIcons;
    public GameObject feedbackObj;
    [Range(0.5f, 5f)] public float flipDelayTime;
    [Range(0.1f, 1f)] public float flipTime;
    [Space(20)]
    public DealerController dealer;

    private int cardsFlipped;
    private int[] deck;
    private CardController[] cards;
    private Collider[] cardColliders;
    private bool[] cardColliderStates;
    private int cardA;
    private int cardB;
    private int cardsLeft;
    private EventSystem _eventSystem;

    private void Start()
    {
        _eventSystem = EventSystem.current;
        
        int i;
        cardsFlipped = 0;
        int cardTypesNum = cardIcons.icons.Length;
        cardsLeft = cardTypesNum;
        int wholeDeckNum = cardIcons.icons.Length * 2;
        deck = new int[wholeDeckNum];
        cards = new CardController[wholeDeckNum];
        cardColliders = new Collider[wholeDeckNum];
        cardColliderStates = new bool[wholeDeckNum];
        for (i = 0; i < wholeDeckNum; i++)
        {
            deck[i] = i % cardTypesNum;
            GameObject cardObj = Instantiate(cardPrefab, deckParent);
            cardObj.name = "Card " + i;
            cardColliders[i] = cardObj.GetComponent<Collider>();
            cardColliderStates[i] = cardColliders[i].enabled;
            CardController card = cardObj.GetComponent<CardController>();
            cards[i] = card;
        }
        dealer.PlaceCards(cards);
        
        
        InitGame();   
    }

    public void InitGame()
    {
        deck = Shuffle(deck);
        for (int i = 0; i < deck.Length; i++)
        {
            CardController card = cards[i];
            int cardType = deck[i];
            card.Setup(cardIcons.icons[cardType], i, this);
        }
        if (feedbackObj != null)
        {
            feedbackObj.SetActive(false);            
        }
    }

    public int[] Shuffle(int[] deck)
    {
        List<int> oldDeck = new List<int>(deck);
        List<int> newDeck = new List<int>(deck.Length);
        while (oldDeck.Count != 0)
        {
            int randomPosition = Random.Range(0, oldDeck.Count - 1);
            newDeck.Add(oldDeck[randomPosition]);
            oldDeck.RemoveAt(randomPosition);
        }
        return newDeck.ToArray();
    }

    public void FlipCard(int number)
    {
        cardsFlipped++;
        if (cardsFlipped == 1)
        {
            cardA = number;
            return;
        }
        
        cardB = number;
        if (deck[cardA] == deck[cardB])
        {
            cards[cardA].Disable();
            cards[cardB].Disable();
            cardsLeft--;
            if (cardsLeft == 0)
            {
                // CELEBRATION
                if (feedbackObj != null)
                {
                    feedbackObj.SetActive(true);                    
                }
            }
        }
        else
        {
            StartCoroutine(FlipBack(cardA, cardB, flipDelayTime));
        }
        cardsFlipped = 0;
        cardA = cardB = -1;
    }

    private void InputEnable(bool val)
    {
        int i;
        if (val == false)
        {
            SaveColliderStates();
            for (i = 0; i < cardColliders.Length; i++)
            {
                cardColliders[i].enabled = val;
            }
        }
        else
        {
            for (i = 0; i < cardColliders.Length; i++)
            {
                cardColliders[i].enabled = cardColliderStates[i];
            }
        }
    }

    private void SaveColliderStates()
    {
        int i;
        for (i = 0; i < cardColliders.Length; i++)
        {
            cardColliderStates[i] = cardColliders[i].enabled;
        }
    }

    private IEnumerator FlipBack(int cardA, int cardB, float delay)
    {
        InputEnable(false);
        yield return new WaitForSeconds(delay);
        cards[cardA].Flip(false);
        cards[cardB].Flip(false);
        InputEnable(true);
    }

}
