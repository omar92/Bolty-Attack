using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Connects to Photon Cloud, then enables GUI to create/join game rooms,
/// which loads the actual Race scene.
/// Inherits from PUNBehavior, overriding standard Photon's Event methods.
/// </summary>
public class PUNMenu : PunBehaviour {

	// References to GUI game objects, so they can be enabled/disabled and
	// used to show useful messages to player.
	public Transform nickPanel;

	//public Sprite[] carTextures;
	//public Sprite noCar;
	public RoomJoiner[] roomButtons;
	public Transform[] playerMenus;
	public InputField edtNickname;
	public Text messages;
	public Sprite[] FishsTextures;
	public Transform[] trackButtons;
	public Image FishSprite;

//	private int carIndex = 0;
	private int creatureIndex = 0;
    private int endscore;

    const string ROOM_NAME= "game";
    // Connects to photon
    void Start () {
		// in case weŕe getting back from a race, already connected to lobby
		if (!PhotonNetwork.connected) {
            PhotonNetwork.ConnectUsingSettings("v1.0");
            messages.text = "connecting...";

        }
        else
        {
            messages.text = "connected";
        }
       
	}

	// For each listed room, sets a join button from the available button/slots.
	//void OnGUI () {
	//	if (!racesPanel.gameObject.GetActive ())
	//		return;
	//	foreach (RoomJoiner bt in roomButtons) {
	//		bt.gameObject.SetActive(false);
	//	}
	//	int index = 0;
	//	foreach (RoomInfo game in PhotonNetwork.GetRoomList())
	//	{
	//		if (index >= roomButtons.Length || !game.IsOpen)
	//			break;
	//		RoomJoiner button = roomButtons[index++];
	//		button.gameObject.SetActive(true);
	//		button.RoomName = game.Name;
	//		string info = game.Name.Trim() + " (" + game.PlayerCount + "/" + game.MaxPlayers + ")";
	//		button.GetComponentInChildren<Text>().text = info;
	//	}
	//}

	// Called when finished editing nickname (which will also serve as 
	// room name - if player creates one)
	public void EnteredNickname() {
		PhotonNetwork.player.NickName = edtNickname.text;
        Debug.Log("id "+PhotonNetwork.player.ID);

        if (PhotonNetwork.GetRoomList().Length < 1)
        {
            CreateGame();
        }
        else
        {
                     
            PhotonNetwork.JoinRoom(ROOM_NAME);

        }
    }

    // When connected to Photon, enable nickname editing (too)
    public override void OnConnectedToMaster()
    {
		PhotonNetwork.JoinLobby ();
		messages.text = "Entering lobby...";
        
    }

	// When connected to Photon Lobby, disable nickname editing and messages text, enables room list
	public override void OnJoinedLobby () {
      
        messages.text = "";
    }

    // Called from UI
    public void CreateGame () {
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 20;      
		PhotonNetwork.CreateRoom(ROOM_NAME, options, TypedLobby.Default);
	}

	// if we join (or create) a room, no need for the create button anymore;
	public override void OnJoinedRoom () {


        SetCustomProperties(PhotonNetwork.player, creatureIndex, PhotonNetwork.playerList.Length - 1);
        LoadMap();
    }

    // (masterClient only) enables start race button
    public override void OnCreatedRoom () {
        PhotonNetwork.SetMasterClient(PhotonNetwork.player);
	}

	// If master client, for every newly connected player, sets the custom properties for him
	// car = 0, position = last (size of player list)
	public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer) {
		if (PhotonNetwork.isMasterClient) {
		//	SetCustomProperties (newPlayer, 0, PhotonNetwork.playerList.Length - 1);
		//	photonView.RPC("UpdateTrack", PhotonTargets.All, trackIndex);
		}
	}

	// when a player disconnects from the room, update the spawn/position order for all
	public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {
		if (PhotonNetwork.isMasterClient) {
			int playerIndex = 0;
			foreach (PhotonPlayer p in PhotonNetwork.playerList) {
				SetCustomProperties(p, (int) p.CustomProperties["Fish"], playerIndex++);
			}
		}
	}

	//public override void OnPhotonPlayerPropertiesChanged (object[] playerAndUpdatedProps) {
 //       UpdatePlayerList();
	//}

 //   // self-explainable
 //   public void UpdatePlayerList()
 //   {
 //       Debug.Log("updating");
 //       ClearPlayersGUI();
 //       int playerIndex = 0;
 //       foreach (PhotonPlayer p in PhotonNetwork.playerList)
 //       {
 //           Debug.Log("nickname "+p.NickName);
 //           Transform playerMenu = playerMenus[playerIndex++];
 //           //if (p == PhotonNetwork.player)
 //           //{
 //           //    playerMenu.FindChild("arrow-left").gameObject.SetActive(true);
 //           //    playerMenu.FindChild("arrow-right").gameObject.SetActive(true);
 //           //}
 //           if (p.CustomProperties.ContainsKey("Fish"))
 //           {
 //               //   playerMenu.FindChild("Image").GetComponent<Image>().sprite = carTextures[(int)p.customProperties["car"]];

 //               playerMenu.FindChild("Text").GetComponent<Text>().text = p.NickName.Trim();
 //           }
 //       }
 //   }

 //   private void ClearPlayersGUI()
 //   {
 //       foreach (Transform t in playerMenus)
 //       {
 //         //  t.FindChild("Image").GetComponent<Image>().sprite = noCar;
 //           t.FindChild("Text").GetComponent<Text>().text = "";
 //         //  t.FindChild("arrow-left").gameObject.SetActive(false);
 //         //  t.FindChild("arrow-right").gameObject.SetActive(false);
 //       }
 //   }

    public void NextCreature() {
		creatureIndex = (creatureIndex + 1) % FishsTextures.Length;
        FishSprite.sprite=FishsTextures[creatureIndex];
        SetCustomProperties(PhotonNetwork.player, creatureIndex, PhotonNetwork.playerList.Length-1);
	}

	public void previousCreature() {

        creatureIndex--;
		if (creatureIndex < 0)
            creatureIndex = FishsTextures.Length - 1;
        FishSprite.sprite = FishsTextures[creatureIndex];
        Debug.Log("FishIndex " + creatureIndex);
        SetCustomProperties(PhotonNetwork.player, creatureIndex, (int) PhotonNetwork.player.CustomProperties ["spawn"]);
	}

	public void addENDScore() {
		// = (trackIndex + 1) % trackTextures.Length;
	//	photonView.RPC("UpdateTrack", PhotonTargets.All, trackIndex);
	}

	//[PunRPC]
	//public void UpdateTrack(int index) {
	//	endscore = index;
	//	//trackSprite.sprite = FishsTextures [FishIndex];
	//}

	// masterClient only. Calls an RPC to start the race on all clients. Called from GUI
	public void CallLoadRace() {
		PhotonNetwork.room.IsOpen = false;
		photonView.RPC("LoadRace", PhotonTargets.All);
	}

	// Loads race level (called once from masterClient)
	// Use LoadLevel from Photon, otherwise it messes up the GOs created in
	// between level changes
	// The level loaded is related to the track chosen by the Master Client (updated via RPC).
//	[PunRPC]
	public void LoadMap () {
		PhotonNetwork.LoadLevel("SpaceArena");
	}

	// sets and syncs custom properties on a network player (including masterClient)
	private void SetCustomProperties(PhotonPlayer player, int Fish, int position) {
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position }, { "Fish", Fish} };
		player.SetCustomProperties(customProperties);
	}

	// Use this to go back to the menu, without leaving the lobby
	public void ResetToMenu () {
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.LoadLevel ("Menu");
	}

}
