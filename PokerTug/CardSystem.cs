using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region 花色大小 enum
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
    public int suits; //花色
    public int number;
}

public class CardSystem : MonoBehaviour
{
    [SerializeField] MainSystem mainSystem;
    [SerializeField] UISystem uISystem;
    [SerializeField] RoundSystem roundSystem;


    /// <summary>
    /// 1回合勝利
    /// </summary>
    public event Action OnPlayerWinARound;

    /// <summary>
    /// 1回合失敗
    /// </summary> 
    public event Action OnPlayerLoseARound;

    //桌面
    [SerializeField] public Card playerTable;
    [SerializeField] public Card opponentTable;

    // 牌池 CardPool
    Card[] cardPool; //  a variable has to  reference an object;
    List<Card> playerHands;
    List<Card> opponentHands;
   
    /// <summary>
    /// 取得玩家手牌
    /// </summary>
    /// <returns></returns>
    public List<Card> GetPlayerHands()
    {
        return playerHands;
    }
    /// <summary>
    /// 取得對手手牌
    /// </summary>
    /// <returns></returns>
    public List<Card> GetOpponentHands()
    {
        return opponentHands;
    }

    /// <summary>
    /// 初始化 Deck
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
    /// Card Pool 洗牌
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
    /// 指派2位玩家手牌
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

    // 比大小區域(結果計算)
    /// <summary>
    /// 選定後翻牌比大小
    /// </summary>
    public void FlopCard()
    {
        if (playerTable.number > opponentTable.number)
        {
            //玩家階級大
            OnPlayerWinARound?.Invoke();
        }
        else if (playerTable.number < opponentTable.number)
        {
            //玩家階級小
            OnPlayerLoseARound?.Invoke();
        }
        else if (playerTable.number == opponentTable.number)
        {
            //相同階級比花色
            if (playerTable.suits > opponentTable.suits)
            {
                //玩家花色大
                OnPlayerWinARound?.Invoke();
            }
            else if (playerTable.suits < opponentTable.suits)
            {
                //玩家花色小
                OnPlayerLoseARound?.Invoke();
            }
            else Debug.LogError("FlopCardError");
        }
    }

    /// <summary>
    /// 開啟新牌局方法
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
    // 最終比分

    // Start is called before the first frame update
    void Start()
    {
        if (uISystem == null) { Debug.LogError("UISystem is null"); }
        if (mainSystem == null) { Debug.LogError("mainSystem is null"); }
        mainSystem.OnStartNewGame += StartNewRound;
        uISystem.OnComfirmResult += FlopCard;// UISystem 選定後通知大小判定
        uISystem.OnComfirmResult += ClearTable;// UISystem 判斷大小後清空桌面
    }
}
