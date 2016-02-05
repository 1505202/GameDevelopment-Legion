using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LegionNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(spawnPrefabs[(short)numPlayers], new Vector3(0,0,0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
