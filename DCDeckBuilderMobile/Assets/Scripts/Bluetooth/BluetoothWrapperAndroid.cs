using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluetoothWrapperAndroid : BluetoothWrapper
{
    public BluetoothWrapperAndroid(string gameObjectName) : base(gameObjectName)
    {
    }

    #region Native setup

    AndroidJavaClass _class;
    AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

    override protected void Setup()
    {
        // Start plugin `Fragment`.
        _class = new AndroidJavaClass("com.samsontech.bluetoothplugin.BluetoothManager");
        _class.CallStatic("start", gameObjectName);
    }

    #endregion

    #region Features

    override public void ShowAlertWithAttributes(string title, string message, string buttonTitle, string cancelButtonTitle)
    {
        //Debug.Log("EPPZ_Alert_Android.ShowAlertWithAttributes(`" + title + "`, `" + message + "`, `" + buttonTitle + "`, `" + cancelButtonTitle + "`, )");
        instance.Call("showAlertWithAttributes", title, message, buttonTitle, cancelButtonTitle);
    }

    public override void PermissionChecks()
    {
        instance.Call("PermissionChecks");
    }
    public override string BtCheck()
    {
        return instance.Call<string>("BtCheck");
    }

    public override string BtCheckIsOn()
    {
        return instance.Call<string>("BtCheckIsOn");
    }

    public override void TurnOnBt()
    {
        instance.Call("TurnOnBt");
    }

    public override string GetDeviceName()
    {
        return instance.Call<string>("GetDeviceName");
    }

    public override void EnableDiscoverable()
    {
        instance.Call("EnableDiscoverable");
    }

    public override string GetPairedDevices()
    {
        return instance.Call<string>("GetPairedDevices");
    }

    public override void DiscoverDevices()
    {
        instance.Call("DiscoverDevices");
    }

    public override void CancelDiscovery()
    {
        instance.Call("CancelDiscovery");
    }

    public override void ConnectToDevice(string device)
    {
        instance.Call("ConnectToDevice", device);
    }

    public override void Disconnect()
    {
        instance.Call("Disconnect");
    }

    public override void ReceivePair()
    {
        instance.Call("ReceivePair");
    }

    public override string GetIsReceiving()
    {
        return instance.Call<string>("GetIsReceiving");
    }

    public override void ReceiveData()
    {
        instance.Call("ReceiveData");
    }

    public override void SendData(string msg)
    {
        instance.Call("SendData", msg);
    }

    public override void Toast(string msg)
    {
        instance.Call("ShowToast", msg);
    }

    public override void Reset()
    {
        instance.Call("Reset");
    }

    public override void RestrictReceive(bool val)
    {
        string temp = "0";
        if (val)
        {
            temp = "1";
        }
        instance.Call("RestrictReceive", temp);
    }

    #endregion
}
