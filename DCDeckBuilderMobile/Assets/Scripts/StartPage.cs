using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPage : Page
{
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject ExitPanel;

    public override void Start()
    {
        
    }

    private void Awake()
    {
        if (GameManager.BluetoothPlugin != null)
        {
            CheckBluetoothCompatable();
        }
        else
        {
            Invoke("CheckBluetoothCompatable", 0.2f);
            Invoke("PermissionChecks", 0.2f);
        }
    }

    void PermissionChecks()
    {
        GameManager.BluetoothPlugin.PermissionChecks();
    }
    void CheckBluetoothCompatable()
    {
        try
        {
            string btCheck = GameManager.BluetoothPlugin.BtCheck();
            if (btCheck != "0")
            {
                //ClickToContinueButtonObject.GetComponent<Button>().interactable = true;
                string btCheckIsOn = GameManager.BluetoothPlugin.BtCheckIsOn();
                if (btCheckIsOn == "1")
                {
                    //GameManager.BluetoothPlugin.ReceivePair();
                }
                else
                {
                    GameManager.BluetoothPlugin.TurnOnBt();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void TapAnywhere()
    {
        try
        {
            //Button btn = ClickToContinueButtonObject.GetComponent<Button>();
            //btn.interactable = false;
            string btCheckIsOn = GameManager.BluetoothPlugin.BtCheckIsOn();
            if (btCheckIsOn == "1")
            {
                //string IsReceiving = GameManager.BluetoothPlugin.GetIsReceiving();
                //if (IsReceiving == "0")
                //{
                //GameManager.BluetoothPlugin.ReceivePair();
                //}
                //btn.interactable = true;
                GM.SwitchPage(this, GameManager.PageNames.MenuPage);
                //Invoke("ButtonInteraction", 3);
            }
            else
            {
                Invoke("SecondCheck", 1);
            }
        }
        catch (Exception ex)
        {
        }
    }

    void SecondCheck()
    {
        try
        {
            //Button btn = ClickToContinueButtonObject.GetComponent<Button>();
            //btn.interactable = false;
            string btCheckIsOn = GameManager.BluetoothPlugin.BtCheckIsOn();
            if (btCheckIsOn == "1")
            {
                string IsReceiving = GameManager.BluetoothPlugin.GetIsReceiving();
                if (IsReceiving == "0")
                {
                    GameManager.BluetoothPlugin.ReceivePair();
                }
                //btn.interactable = true;
                GM.SwitchPage(this, GameManager.PageNames.MenuPage);
                //Invoke("ButtonInteraction", 3);
            }
            else
            {
                Invoke("StartGame", 1);
            }
        }
        catch (Exception ex)
        {
        }
    }

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

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        ExitPanel.SetActive(false);
    }
}
