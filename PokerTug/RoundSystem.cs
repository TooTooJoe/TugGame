using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoundData
{
    public int currentRound;
    public int endRound;
    public int winningRounds;
    public int losingRounds;
    public int winGames;
}
public class RoundSystem : MonoBehaviour
{
    [SerializeField] MainSystem mainSystem;
    [SerializeField] UISystem uISystem;
    [SerializeField] CardSystem cardSystem;
    public event Action OnRefreshRoundNumber;
    RoundData roundData = new RoundData();

    int currentRound;
    int endRound;
    int winningRounds;
    int losingRounds;


    public void SetRoundDataToDefault()
    {
        currentRound = 1;
        endRound = 10;
        winningRounds = 0;
        losingRounds = 0;
        roundData.currentRound = currentRound;
        roundData.endRound = endRound;
        roundData.winningRounds = winningRounds;
        roundData.losingRounds = losingRounds;
        OnRefreshRoundNumber?.Invoke();
    }

    /// <summary>
    /// Get 回合數Data
    /// </summary>
    /// <returns></returns>
    public RoundData GetRoundData()
    {
        return roundData;
    }
    /// <summary>
    /// 玩家確認結果後刷新回合數字
    /// </summary>
    void RefreshRoundNumber()
    {
        if(currentRound > endRound) { currentRound = endRound;}
        roundData.currentRound = currentRound;
        roundData.winningRounds = winningRounds;
        roundData.losingRounds = losingRounds;
        OnRefreshRoundNumber?.Invoke();
    }
    void PlayerWinARound()
    {
        currentRound++;
        winningRounds++;
    }
    void PlayerLoseARound()
    {
        currentRound++;
        losingRounds++;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (uISystem == null) { Debug.LogError("uiSystem is null"); }
        mainSystem.OnRoundDataDefault += SetRoundDataToDefault;
        uISystem.OnComfirmResult += RefreshRoundNumber;
        cardSystem.OnPlayerWinARound += PlayerWinARound;
        cardSystem.OnPlayerWinARound += RefreshRoundNumber;
        cardSystem.OnPlayerLoseARound += PlayerLoseARound;
        cardSystem.OnPlayerLoseARound += RefreshRoundNumber;
    }
}
