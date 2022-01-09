using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : Page
{
    [SerializeField]
    GameManager GM;

    public override void Start()
    {
        
    }

    public void OnClickOpenRuleBooks()
    {
        StartCoroutine(IOpenRuleBooks());
    }

    IEnumerator IOpenRuleBooks()
    {
        yield return new WaitForSeconds(0.2f);
        GM.SwitchPage(this, GameManager.PageNames.GameRulesPage);
    }

    public void OnClickOpenCreateParty()
    {
        StartCoroutine(IOpenCreateParty());
    }

    IEnumerator IOpenCreateParty()
    {
        yield return new WaitForSeconds(0.2f);
        GM.SwitchPage(this, GameManager.PageNames.CreatePartyPage);
    }

    public override void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BtnBack();
            }
        }
    }

    public void BtnBack()
    {
        StartCoroutine(IBack());
    }

    IEnumerator IBack()
    {
        yield return new WaitForSeconds(0.2f);
        GM.SwitchPage(this, GameManager.PageNames.StartPage);
    }
}
