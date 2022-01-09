using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePartyPage : Page
{
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private Text PlayerCountText;
    [SerializeField]
    private GameObject ListContent;
    [SerializeField]
    private GameObject listButtonPrefab;
    [SerializeField]
    private GameObject InvitePlayerPanel;
    [SerializeField]
    private GameObject InvitePlayerListContent;
    [SerializeField]
    private GameObject InvitePlayerListPrefab;
    [SerializeField]
    private GameObject RefreshIcon;
    [SerializeField]
    private Button BtnRefresh;

    private string DeviceName;

    private List<GameObject> ListPlayers = new List<GameObject>();
    private List<GameObject> InviteablePlayers = new List<GameObject>();

    public override void OnEnable()
    {
        DeviceName = GameManager.BluetoothPlugin.GetDeviceName();

        PlayerCountText.text = "1/4";

        GameObject button = Instantiate(listButtonPrefab, ListContent.transform);
        button.transform.name = DeviceName;
        button.GetComponentInChildren<Text>().text = "Player 1: " + DeviceName;
        ListPlayers.Add(button);
    }

    public void OnClickStartGame()
    {

    }

    public void OnClickLeaveParty()
    {
        GM.SwitchPage(this, GameManager.PageNames.MenuPage);
    }

    public override void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickLeaveParty();
            }
        }
    }

    public void OnClickInvitePlayer()
    {
        if(!InvitePlayerPanel.activeSelf)
        {
            InvitePlayerPanel.SetActive(true);
            GetNewDevices();
        }
    }

    public void OnClickClosePanel()
    {
        if (InvitePlayerPanel.activeSelf)
        {
            InvitePlayerPanel.SetActive(false);
            CancelDiscovery();
            DestoryInviteablePlayers();
        }
    }

    public void NewDeviceFound(string device)
    {
        try
        {
            if (InvitePlayerPanel.activeSelf)
            {
                GameObject button = Instantiate(InvitePlayerListPrefab, InvitePlayerListContent.transform);
                button.GetComponent<ListButton>().SetUpButton(this, device);
                button.transform.name = device;
                button.GetComponentInChildren<Text>().text = device.Split('.')[0];
                button.GetComponent<Button>().onClick.AddListener(button.GetComponent<ListButton>().Clicked);
                InviteablePlayers.Add(button);
            }
        }
        catch (Exception ex)
        {
            //errorText.SetActive(true);
            //errorText.GetComponent<Text>().text = "Error New Device Found";
        }
    }

    public override void ButtonCallBack(string msg)
    {        
        OnClickClosePanel();
    }

    public void GetNewDevices()
    {
        try
        {
            DestoryInviteablePlayers();
            InviteablePlayers.Clear();
            BtnRefresh.interactable = false;
            RefreshIcon.GetComponent<Animator>().SetBool("Loading", true);
            GameManager.BluetoothPlugin.DiscoverDevices();
            Invoke("CancelDiscovery", 10);
        }
        catch (Exception ex)
        {
            //errorText.SetActive(true);
            //errorText.GetComponent<Text>().text = "Error Getting New Devices";
        }
    }

    void CancelDiscovery()
    {
        try
        {
            GameManager.BluetoothPlugin.CancelDiscovery();
            BtnRefresh.interactable = true;
            RefreshIcon.GetComponent<Animator>().SetBool("Loading", false);
        }
        catch (Exception ex)
        {
            //errorText.SetActive(true);
            //errorText.GetComponent<Text>().text = "Error Canceling Discovery";
        }
    }

    public void DestoryInviteablePlayers()
    {
        try
        {
            if (InviteablePlayers.Count > 0)
            {
                for (int i = 0; i < InviteablePlayers.Count; i++)
                {
                    Destroy(InviteablePlayers[i]);
                }
            }
        }
        catch (Exception ex)
        {
            //errorText.SetActive(true);
            //errorText.GetComponent<Text>().text = "Error Destoring New Devices";
        }
    }

}
