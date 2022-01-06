using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPage : Page
{
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject ExitPanel;

    // Start is called before the first frame update
    public override void Start()
    {
        //StartCoroutine("ISetup");
    }

    IEnumerator ISetup()
    {
        //yield return new WaitUntil(() => gameManager.GotBluetoothPlugin == true);
        //Setup();
        return null;
    }

    private void Setup()
    {
        //CheckBluetoothCompatable();
        //PermissionChecks();
        //GetSettings();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ExitPanel.activeSelf == false)
                {
                    ExitPanel.SetActive(true);
                }
            }
        }
    }

    public void TapAnywhere()
    {
        GM.SwitchPage(this, GameManager.PageNames.MenuPage);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        ExitPanel.SetActive(false);
    }
}
