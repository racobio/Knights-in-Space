using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator scene_transition;
    public float transition_time = 1f;
    /* Don't need update for this
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }*/
    //this function triggers when the player collides with the sprite this script is a component of
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Gate Colliding");
        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.name == "Player")
        {
            //Debug.Log("Gate Colliding if statement");
            SceneManager.LoadScene(0);
            //StartCoroutine(LoadLevel(0));
        }

    }

    //this function specifically loads the next level in the index
    public void LoadNextLevel()
    {
        //loads the next scene by grabbing the current scene index and adding 1
        //it's a coroutine because we want to wait for some time during this call
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    //this is the function that loads levels based on index
    IEnumerator LoadLevel(int levelIndex)
    {
        //Debug.Log("load level");
        //uses trigger "start" to begin animation
        scene_transition.SetTrigger("Start");
        //starts a coroutine to wait for 'transition_time' seconds, default is 1
        yield return new WaitForSeconds(transition_time);
        //loads scene by index
        SceneManager.LoadScene(levelIndex);
    }


}
