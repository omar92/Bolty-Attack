/// <summary>
/// 2D Space Shooter Example
/// By Bug Games www.Bug-Games.net
/// Programmer: Danar Kayfi - Twitter: @DanarKayfi
/// Special Thanks to Kenney for the CC0 Graphic Assets: www.kenney.nl
/// 
/// This is the SharedValues Script:
/// - Shared Value Script between all other scripts
/// - In-Game & GameOver GUI
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using Photon;

public class GameManager : PunBehaviour
{
    public SmoothFollow cameraControl;

    //Public Var
    public GUIText scoreText; 				//GUI Score
	public GUIText GameOverText; 			//GUI GameOver
	public GUIText FinalScoreText; 			//GUI Final Score
	public GUIText ReplayText;              //GUI Replay
    public Transform[] spawnPoints;

    //Public Shared Var
    public static int score = 0; 			//Total in-game Score
	public static bool gameover = false; 	//GameOver Trigger

	// Use this for initialization
	void Start () 
	{
		gameover = false; 					//return the Gameover trigger to its initial state when the game restart
		score = 0;        //return the Score to its initial state when the game restart

   //     photonView.RPC("ConfirmLoad", PhotonTargets.All);
        CreateCar();

    }
    // called when a player computer finishes loading this scene...
    //[PunRPC]
    //public void ConfirmLoad()
    //{
    //    loadedPlayers++;
    //}

    // Fixed Update is called one per specific time
    void FixedUpdate ()
	{
		//Excute when the GameOver Trigger is True
		if (gameover == true )
		{
            
			GameOverText.text = "GAME OVER"; 			//Show GUI GameOver
			FinalScoreText.text = "" + score;           //Show GUI FinalScore
            gameover = false;                                         //ReplayText.text = "PRESS R TO REPLAY"; 		//Show GUI Replay
           
        }
	}

    private void CreateCar()
    {
        // gets spawn Transform based on player join order (spawn property)
        int pos = (int)PhotonNetwork.player.CustomProperties["spawn"];
        Debug.Log("pos " + pos);

        int shipNumber = (int)PhotonNetwork.player.CustomProperties["Fish"];
        Debug.Log("shipNumber " + shipNumber);
        Transform spawn = spawnPoints[pos];

        // instantiate car at Spawn Transform
        // ship prefabs are numbered in the same order as the car sprites that the player chose from
        GameObject ship = PhotonNetwork.Instantiate("Fish" + shipNumber, spawn.position, spawn.rotation, 0);
        if (photonView.isMine)
        {
            ship.tag = "Player";
        }
        else
        {
            ship.tag = "Enemy";

        }
        cameraControl.target = ship.transform;

    }
}