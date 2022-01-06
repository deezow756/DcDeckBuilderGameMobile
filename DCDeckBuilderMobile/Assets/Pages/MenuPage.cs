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
        GM.SwitchPage(this, GameManager.PageNames.StartPage);
    }
}
