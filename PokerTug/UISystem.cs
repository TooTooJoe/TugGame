using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystem : MonoBehaviour
{
    //public event Action OnCardSlected;
    public event Action OnComfirmResult;
    public event Action OnFinalResult;
    [SerializeField] RoundSystem roundSystem;
    [SerializeField] CardSystem cardSystem;
    [SerializeField] MainSystem mainSystem;
    [SerializeField] PlayerSystem playerSystem;
    [SerializeField] List<TextMeshProUGUI> playerHandsUI;
    [SerializeField] List<GameObject> playerHandsObjects;
    [SerializeField] List<TextMeshProUGUI> opponentHandsUI;
    [SerializeField] List<GameObject> opponentObjects;
    [SerializeField] GameObject checkingPannel;
    [SerializeField] GameObject showResultPannel;
    [SerializeField] GameObject EndGamePannel;
    [SerializeField] GameObject playTableObj;
    [SerializeField] GameObject opponentTableObj;
    [SerializeField] TextMeshProUGUI playerTableUI;
    [SerializeField] TextMeshProUGUI opponentTableUI;
    [SerializeField] TextMeshProUGUI showResultUI;
    [SerializeField] TextMeshProUGUI currentRoundUI;
    [SerializeField] TextMeshProUGUI winRoundUI;
    [SerializeField] TextMeshProUGUI loseRoundUI;
    [SerializeField] TextMeshProUGUI winGameUI;
    [SerializeField] TextMeshProUGUI endGameUI;
    [SerializeField] TextMeshProUGUI playerNameUI;
    [SerializeField] TextMeshProUGUI opponentNameUI;
    [SerializeField] TextMeshProUGUI historyPlayers;
    List<Card> playerHands;
    List<Card> opponentHands;
    RoundData roundData;
    

    public void ShowPlayersHistory(PlayerSystem.GameData[] arrayRanking)
    {
        string content= "name " +"    "+ "winner" +"    "+"endinground "+"\r\n";
        for(int i =0; i <arrayRanking.Length; i++)
        {
            content = content + arrayRanking[i].name +"    " +arrayRanking[i].winner +"    "+ arrayRanking[i].endinground + "\r\n";
        }
        historyPlayers.text = content;
    }
    





    /// <summary>
    /// 收回出牌
    /// </summary>
    public void RetreatTableCard()
    {
        if (cardSystem.playerTable != null)
        {
            playerHands.Add(cardSystem.playerTable);
            cardSystem.playerTable = null;
            opponentHands.Add(cardSystem.opponentTable);
            cardSystem.opponentTable = null;
            checkingPannel.SetActive(false);
            SetUpCards();
        }
    }

    private void WritePlayerToUI()
    {
        playerNameUI.text = playerSystem.gameData.name;
    }


    /// <summary>
    /// 結束遊戲面板
    /// </summary>
    void EndGamePannelOpen()
    {
        endGameUI = EndGamePannel.GetComponentInChildren<TextMeshProUGUI>();
        switch (roundData.winningRounds - roundData.losingRounds)
        {
            case 0:
                {
                    playerSystem.gameData.winner = "Draw";
                    //playerSystem.gameData.endinground = roundData.endRound;
                    //endGameUI.text = "Game Over." + "Winner is" + playerSystem.gameData.winner + ".";
                    EndGamePannel.SetActive(true);
                    break;
                }
            case > 0:
                {
                    playerSystem.gameData.winner = "Human";
                    //playerSystem.gameData.endinground = roundData.endRound;
                    //endGameUI.text = "Game Over." + "Winner is" + playerSystem.gameData.winner + ".";
                    EndGamePannel.SetActive(true);
                    break;
                }
            case < 0 :
                {
                    playerSystem.gameData.winner = "Compute";
                    //playerSystem.gameData.endinground = roundData.endRound;
                    //endGameUI.text = "Game Over." + "Winner is" + playerSystem.gameData.winner + ".";
                    EndGamePannel.SetActive(true);
                    break;
                }
        }
        playerSystem.gameData.endinground = roundData.endRound;
        OnFinalResult?.Invoke();
        endGameUI.text = "Game Over." + "Winner is  " + playerSystem.gameData.winner + ".";
    }
    void EndGamePannelClose()
    {
        EndGamePannel.SetActive(false);
    }

    /// <summary>
    /// UI顯示手牌
    /// </summary>
    public void SetUpCards()
    {
        playerHands = cardSystem.GetPlayerHands();
        opponentHands = cardSystem.GetOpponentHands();
        for(int i = 0; i < 10; i++)//Hands Text set empty
        {
            playerHandsUI[i].text = "";
            opponentHandsUI[i].text = "";
        }
        playerTableUI.text = ""; //Table Text set empty
        opponentTableUI.text = ""; //Table Text set empty

        if (cardSystem.playerTable != null) // Table Text Refresh
        { 
            playerTableUI.text = ((Suits)cardSystem.playerTable.suits).ToString() + "\r\n" + cardSystem.playerTable.number.ToString(); 
            opponentTableUI.text = ((Suits)cardSystem.opponentTable.suits).ToString() + "\r\n" + cardSystem.opponentTable.number.ToString();
        }
        for (int i =0; i < playerHands.Count; i++) // Hands refresh
        {
            playerHandsObjects[i].GetComponent<Button>().interactable = true;
            if (playerHands.Count < 10)
            {
                playerHandsObjects[playerHands.Count].GetComponent<Button>().interactable = false;
            }
            //else if (playerHands.Count >0) { playerHandsObjects[playerHands.Count-1].GetComponent<Button>().interactable = true; }

            playerHandsUI[i].text = ((Suits)playerHands[i].suits).ToString() + "\r\n" + playerHands[i].number.ToString();
        }
        for (int i = 0; i < opponentHands.Count; i++)// Hands refresh
        {
            opponentObjects[i].GetComponent<Button>().interactable = true;
            if (opponentHands.Count < 10)
            {
                opponentObjects[opponentHands.Count].GetComponent<Button>().interactable = false;
            }
            //else if (opponentHands.Count>0){ opponentObjects[playerHands.Count-1].GetComponent<Button>().interactable = true; }

            opponentHandsUI[i].text = ((Suits)opponentHands[i].suits).ToString() + "\r\n" + opponentHands[i].number.ToString();
        }
    }

    /// <summary>
    /// 玩家選擇出牌
    /// </summary>
    /// <param name="select"></param>
    public void SlectCard(int select)
    {
        cardSystem.playerTable = playerHands[select];
        playerHands.Remove(playerHands[select]);
        checkingPannel.SetActive(true);
    }

    /// <summary>
    /// 隨機對手選擇出牌>UnityEventSystem CallBack
    /// </summary>
    public void SlectCounterCardRandom()
    {
        System.Random random = new System.Random();
        int randomNum = random.Next(0, opponentHands.Count);
        cardSystem.opponentTable = opponentHands[randomNum];
        opponentHands.Remove(opponentHands[randomNum]);
        SetUpCards();
    }

    /// <summary>
    /// 玩家確認結果
    /// </summary>
    public void ComfirmResult()
    {
        checkingPannel.SetActive(false);
        OnComfirmResult?.Invoke();
    }

    /// <summary>
    /// 顯示勝利結果
    /// </summary>
    public void ShowWinResult()
    {
        showResultPannel.SetActive(true);
        showResultUI.text = "Win a round!";
    }
    /// <summary>
    /// 顯示失敗結果
    /// </summary>
    public void ShowLoseResult()
    {
        showResultPannel.SetActive(true);
        showResultUI.text = "Lose a round!";
    } 

    /// <summary>
    /// 關閉結果面板
    /// </summary>
    public void CloseResultPannel()
    {
        if(showResultPannel.activeSelf == true)
        {
            showResultPannel.SetActive(false);
        }
    }

    /// <summary>
    /// 刷新UI回合數字
    /// </summary>
    void RefreshRoundUINum()
    {        //更新回合資料然後顯示
        roundData = roundSystem.GetRoundData();
        currentRoundUI.text = "Current Round:"+"\r\n"+roundData.currentRound.ToString();
        winRoundUI.text = "Win Round:"+"\r\n"+roundData.winningRounds.ToString();
        loseRoundUI.text = "Lose Round:"+"\r\n"+roundData.losingRounds.ToString();
        winGameUI.text = "Win Game:"+"\r\n"+roundData.winGames.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        mainSystem.OnStartNewGame += RefreshRoundUINum; //註冊新回合UI更新
        mainSystem.OnStartNewGame += EndGamePannelClose; 
        mainSystem.OnStartNewGame += WritePlayerToUI; 
        mainSystem.OnAllCardsRefresh += SetUpCards;
        roundSystem.OnRefreshRoundNumber += RefreshRoundUINum;//註冊每回合UI更新
        cardSystem.OnPlayerWinARound += ShowWinResult;
        cardSystem.OnPlayerLoseARound+= ShowLoseResult;
        mainSystem.OnEndGame += EndGamePannelOpen;
    }
}
