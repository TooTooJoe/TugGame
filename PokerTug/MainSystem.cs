using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainSystem : MonoBehaviour
{
    public bool isNewGame = false;
    public bool isRoundDataDefault = false;
    bool isStartCountDown = false;
    float TimeCountDown = 2;
    RoundData roundData;
    public event Action OnStartNewGame;
    public event Action OnAllCardsRefresh;
    public event Action OnRoundDataDefault;
    public event Action OnEndGame;
    public event Action OnObjectsMove;

    [SerializeField] UISystem uIsystem;
    [SerializeField] RoundSystem roundSystem;
    // Start is called before the first frame update
    void Start()
    {
        uIsystem.OnComfirmResult += ChangeCountDownState;
    }


    // Update is called once per frame
    void Update()
    {
        roundData = roundSystem.GetRoundData();
        if (isNewGame)
        {
            isNewGame = false;
            OnStartNewGame?.Invoke();
            OnAllCardsRefresh?.Invoke();
        } // 開啟新牌局
        if (isRoundDataDefault)
        {
            isRoundDataDefault = false;
            OnRoundDataDefault?.Invoke();
        }
        if (isStartCountDown)
        {
            StartCountDown();
        }//Result面板
        OnObjectsMove?.Invoke();
        CompleteGame();
    }

    public void StartNewGame()
    {
        isNewGame = true;
        isRoundDataDefault = true;
    }

    /// <summary>
    /// Resut倒數狀態旗標
    /// </summary>
    public void ChangeCountDownState()
    {
        isStartCountDown = true;
    } //Change State to Start Count Down.

    /// <summary>
    /// 結果確認後2s 關閉Result 面板
    /// </summary>
    void StartCountDown()
    {
        TimeCountDown -= Time.deltaTime;
        if (TimeCountDown <= 0)
        {
            TimeCountDown = 2;
            isStartCountDown = false;
            uIsystem.CloseResultPannel();
            uIsystem.SetUpCards();
        } // start countdown method.
    }//倒數計時


    /// <summary>
    /// 關閉Exit Game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 最後一回合結束
    /// </summary>
    void CompleteGame()
    {
         if (roundData.winningRounds + roundData.losingRounds == 10)
        {
            OnEndGame?.Invoke();
        }
    }

    /// <summary>
    /// 其中一個玩家勝差超過6回合
    /// </summary>
    public void CompleteGameEarly()
    {
        OnEndGame?.Invoke();
    }
}
