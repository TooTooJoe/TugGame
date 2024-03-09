using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsControlSystem : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject opponent;
    [SerializeField] GameObject linkObject;
    [SerializeField] GameObject playerNameUI;
    [SerializeField] GameObject opponentNameUI;
    [SerializeField] MainSystem mainSystem;
    [SerializeField] RoundSystem roundSystem;
    RoundData roundData;
    public Vector3 playerDefPosition = new Vector3(-3, 0, 10);
    public Vector3 opponentDefPosition = new Vector3(3, 0, 10);
    public Vector3 linkDefPosition = new Vector3(0, 0, 10);
    public Vector3 playerNameDefPosition = new Vector3(-3, 0, 10);
    public Vector3 opponentNameDefPosition = new Vector3(3, 0, 10);

    void MoveObjects()
    {
        roundData = roundSystem.GetRoundData();
        Vector3 distance = new Vector3((roundData.winningRounds*-0.5f)+(roundData.losingRounds*0.5f),0,0);
        player.transform.position = Vector3.Lerp(player.transform.position, playerDefPosition + distance,0.1f);
        opponent.transform.position = Vector3.Lerp(opponent.transform.position, opponentDefPosition +distance,0.1f);
        linkObject.transform.position = Vector3.Lerp(linkObject.transform.position,linkDefPosition + distance,0.1f);
        playerNameUI.transform.position = Vector3.Lerp(playerNameUI.transform.position,playerNameDefPosition + distance,0.1f);
        opponentNameUI.transform.position = Vector3.Lerp(opponentNameUI.transform.position,opponentNameDefPosition + distance,0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //roundSystem.OnRefreshRoundNumber += MoveObjects;
        mainSystem.OnObjectsMove += MoveObjects;
    }
}
