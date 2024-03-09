using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region ���j�p enum
public enum Suits
{
    Club = 100,
    Diamond = 101,
    Heart = 102,
    Spade = 103,
}
#endregion
public class Card
{
    public int order;
    public int suits; //���
    public int number;
}

public class CardSystem : MonoBehaviour
{
    [SerializeField] MainSystem mainSystem;
    [SerializeField] UISystem uISystem;
    [SerializeField] RoundSystem roundSystem;


    /// <summary>
    /// 1�^�X�ӧQ
    /// </summary>
    public event Action OnPlayerWinARound;

    /// <summary>
    /// 1�^�X����
    /// </summary> 
    public event Action OnPlayerLoseARound;

    //�ୱ
    [SerializeField] public Card playerTable;
    [SerializeField] public Card opponentTable;

    // �P�� CardPool
    Card[] cardPool; //  a variable has to  reference an object;
    List<Card> playerHands;
    List<Card> opponentHands;
   
    /// <summary>
    /// ���o���a��P
    /// </summary>
    /// <returns></returns>
    public List<Card> GetPlayerHands()
    {
        return playerHands;
    }
    /// <summary>
    /// ���o����P
    /// </summary>
    /// <returns></returns>
    public List<Card> GetOpponentHands()
    {
        return opponentHands;
    }

    /// <summary>
    /// ��l�� Deck
    /// </summary>
    public void CardPoolInitialized()
    {
        int cardPoolInDex = 0;
        if (cardPool == null)
        {
            cardPool = new Card[52];
            for (int s =(int)Suits.Club ; s < (int)Suits.Spade+1 ; s++)
            {
                for (int n =2; n <=14; n++)
                {
                    cardPool[cardPoolInDex] = new Card();
                    cardPool[cardPoolInDex].suits = s;
                    cardPool[cardPoolInDex].number = n;
                    cardPoolInDex++;
                }
            }
        }
    }

    /// <summary>
    /// Card Pool �~�P
    /// </summary>
    public void CardPoolRandom()
    {
        if(cardPool  == null) { Debug.LogError("The Card Pool is null !"); }
        for(int i  = cardPool.Length-1; i >= 0; i--)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, i);
            Card temp = cardPool[i];
            cardPool[i] = cardPool[randomIndex];
            cardPool[randomIndex] = temp;
        }
    }  

    /// <summary>
    /// ����2�쪱�a��P
    /// </summary>
    public void AssignCards()
    {
        if (playerHands == null)
        {
            playerHands = new List<Card>();
            //Debug.LogError("playerHands are null");
        }
        if(opponentHands == null)
        {
            opponentHands = new List<Card>();
            //Debug.LogError("opponentHands are null");
        }
        CardPoolRandom();
        playerHands.Clear();
        opponentHands.Clear();
        for (int i = 0; i < 10; i++)
        {
            playerHands.Add(cardPool[i]);
            playerHands[i].order = i;
            opponentHands.Add(cardPool[i+10]);
            opponentHands[i].order = i;
        }
    }

    // ��j�p�ϰ�(���G�p��)
    /// <summary>
    /// ��w��½�P��j�p
    /// </summary>
    public void FlopCard()
    {
        if (playerTable.number > opponentTable.number)
        {
            //���a���Ťj
            OnPlayerWinARound?.Invoke();
        }
        else if (playerTable.number < opponentTable.number)
        {
            //���a���Ťp
            OnPlayerLoseARound?.Invoke();
        }
        else if (playerTable.number == opponentTable.number)
        {
            //�ۦP���Ť���
            if (playerTable.suits > opponentTable.suits)
            {
                //���a���j
                OnPlayerWinARound?.Invoke();
            }
            else if (playerTable.suits < opponentTable.suits)
            {
                //���a���p
                OnPlayerLoseARound?.Invoke();
            }
            else Debug.LogError("FlopCardError");
        }
    }

    /// <summary>
    /// �}�ҷs�P����k
    /// </summary>
    public void StartNewRound()
    {
        CardPoolInitialized();
        CardPoolRandom();
        AssignCards();
        playerTable = null;
        opponentTable = null;
    }

    public void ClearTable()
    {
        playerTable = null;
        opponentTable = null;
    }
    // �̲פ��

    // Start is called before the first frame update
    void Start()
    {
        if (uISystem == null) { Debug.LogError("UISystem is null"); }
        if (mainSystem == null) { Debug.LogError("mainSystem is null"); }
        mainSystem.OnStartNewGame += StartNewRound;
        uISystem.OnComfirmResult += FlopCard;// UISystem ��w��q���j�p�P�w
        uISystem.OnComfirmResult += ClearTable;// UISystem �P�_�j�p��M�Ůୱ
    }
}
