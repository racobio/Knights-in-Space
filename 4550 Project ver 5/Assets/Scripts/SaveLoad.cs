using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoad : MonoBehaviour
{    
    //Player returns the gameobject named "Player"
    GameObject Player()
    {
        GameObject player = GameObject.Find("Player");
        return player;
    }
    //SavePlayer will call PlayerPrefs.Set for every var we want to save
    //then calls save to write PlayerPrefs to disk
    //in this version it's only health
    public void SavePlayer()
    {
        GameObject player = Player();
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
        PlayerPrefs.SetFloat("Health", player.GetComponent<Player>().Health);
        PlayerPrefs.Save();
        Debug.Log("Saved");
    }
    //LoadPlayer will call PlayerPrefs.Get for every var saved in SavePlayer
    public void LoadPlayer()
    {
        GameObject player = Player();
        player.GetComponent<Player>().Health = PlayerPrefs.GetFloat("Health", 100);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX", 5), 
                                                PlayerPrefs.GetFloat("PlayerY", 5), 
                                                PlayerPrefs.GetFloat("PlayerZ", 0));
        Debug.Log("Loaded");
    }
}
