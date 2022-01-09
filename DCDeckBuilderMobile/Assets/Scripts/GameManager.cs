using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PageNames { StartPage, MenuPage, CreatePartyPage, GameRulesPage}
    public Dictionary<PageNames, Page> Pages = new Dictionary<PageNames, Page>();
    [SerializeField]
    private Page StartPage;
    [SerializeField]
    private Page MenuPage;
    [SerializeField]
    private Page CreatePartyPage;
    [SerializeField]
    private Page GameRulesPage;

    static BluetoothWrapper bluetoothPlugin;
    public static BluetoothWrapper BluetoothPlugin { get => bluetoothPlugin; set { bluetoothPlugin = value; } }


    void Start()
    {
        Pages.Add(PageNames.StartPage, StartPage);
        Pages.Add(PageNames.MenuPage, MenuPage);
        Pages.Add(PageNames.CreatePartyPage, CreatePartyPage);
        Pages.Add(PageNames.GameRulesPage, GameRulesPage);
        BluetoothPlugin = BluetoothWrapper.pluginWithGameObjectName(this.transform.name);
    }

    public void SwitchPage(Page prev, PageNames next)
    {
        prev.gameObject.SetActive(false);
        Pages[next].gameObject.SetActive(true);
    }

    public void NewDeviceFound(string device)
    {
        //connectionPage.GetComponent<ConnectionPage>().NewDeviceFound(device);
    }

    public void ConnectedToDeviceCallBack(string msg)
    {
        
    }
    public void ReceivedPairCallBack(string device)
    {
        
    }

    public void DisconnectedCallBack(string status)
    {
       
    }

    public void ReceivedDataCallBack(string msg)
    {
        
    }

    public void DataSentCallBack(string status)
    {

    }
}
