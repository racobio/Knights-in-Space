using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public float curExp;
    public float maxExp;
    public int curLevel;
    public Image ExpImage;
    public Text LevelText;
    public GameObject Helper;

    //reference
    PlayerCombat playerCom;
    Player playerHea;

    Renderer rend;

    private void Start()
    {
        playerCom = FindObjectOfType<PlayerCombat>();
        playerHea = FindObjectOfType<Player>();
        rend =GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        curExp = 0;
        curLevel = 1;
        Helper.SetActive(false);
    }

    private void Update()
    {
        if(curExp >= maxExp)
        {
            curExp -= maxExp;
            curLevel +=1;
            StartCoroutine("OnHelper");
            Bonus();
            ExpImage.fillAmount = Mathf.Lerp(ExpImage.fillAmount, curExp/maxExp, 10*Time.deltaTime);
        }
        else
        {
            ExpImage.fillAmount = Mathf.Lerp(ExpImage.fillAmount, curExp/maxExp, 10*Time.deltaTime);
        }

        LevelText.text = "Level " + curLevel.ToString();
    }
    //Generate helper 7 seconds
    IEnumerator OnHelper()
    {
        Helper.SetActive(true);
        yield return new WaitForSeconds(7f);
        Helper.SetActive(false);
    }
    //Increases Max health by 20. Restore all health losses. Added laser bullets and shotgun bullets
    void Bonus()
    {
        playerHea.MaxHealth += 20;
        playerHea.Health = playerHea.MaxHealth;
        playerCom.LaserBullet += 10;
        playerCom.ShotgunBullet += 12;
    }
}