using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoad : MonoBehaviour
{
    private string slot = "1";
    //SlotSelect is called by save or load menu buttons to alter slot, which modifies playerprefs keys for saving/loading
    //SlotSelect must be attached to the buttons in these menus with a corresponding parameter
    public void SlotSelect(int choose)
    {
        switch(choose)
        {
            case 1: slot = "1"; break;
            case 2: slot = "2"; break;
            case 3: slot = "3"; break;
            default: slot = "1"; break;
        }
        return;
    }
    //Player returns the gameobject named "Player"
    GameObject Player()
    {
        GameObject player = GameObject.Find("Player");
        return player;
    }
    //SavePlayer will call PlayerPrefs.Set for every var we want to save
    //then calls save to write PlayerPrefs to disk
    public void SavePlayer()
    {
        GameObject player = Player();
        PlayerPrefs.SetFloat(slot + "PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat(slot + "PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat(slot + "PlayerZ", player.transform.position.z);
        PlayerPrefs.SetFloat(slot + "Health", player.GetComponent<Player>().Health);
        PlayerPrefs.Save();
        Debug.Log("Saved");
    }
    //LoadPlayer will call PlayerPrefs.Get for every var saved in SavePlayer
    public void LoadPlayer()
    {
        GameObject player = Player();
        player.GetComponent<Player>().Health = PlayerPrefs.GetFloat(slot + "Health", 100);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat(slot + "PlayerX", 5), 
                                                PlayerPrefs.GetFloat(slot + "PlayerY", 5), 
                                                PlayerPrefs.GetFloat(slot + "PlayerZ", 0));
        Debug.Log("Loaded");
    }
}
