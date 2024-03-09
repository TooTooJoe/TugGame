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
        } // �}�ҷs�P��
        if (isRoundDataDefault)
        {
            isRoundDataDefault = false;
            OnRoundDataDefault?.Invoke();
        }
        if (isStartCountDown)
        {
            StartCountDown();
        }//Result���O
        OnObjectsMove?.Invoke();
        CompleteGame();
    }

    public void StartNewGame()
    {
        isNewGame = true;
        isRoundDataDefault = true;
    }

    /// <summary>
    /// Resut�˼ƪ��A�X��
    /// </summary>
    public void ChangeCountDownState()
    {
        isStartCountDown = true;
    } //Change State to Start Count Down.

    /// <summary>
    /// ���G�T�{��2s ����Result ���O
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
    }//�˼ƭp��


    /// <summary>
    /// ����Exit Game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// �̫�@�^�X����
    /// </summary>
    void CompleteGame()
    {
         if (roundData.winningRounds + roundData.losingRounds == 10)
        {
            OnEndGame?.Invoke();
        }
    }

    /// <summary>
    /// �䤤�@�Ӫ��a�Ӯt�W�L6�^�X
    /// </summary>
    public void CompleteGameEarly()
    {
        OnEndGame?.Invoke();
    }
}
