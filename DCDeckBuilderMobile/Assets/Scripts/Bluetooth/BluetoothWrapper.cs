using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluetoothWrapper : MonoBehaviour
{
    #region Native setup

    protected string gameObjectName;

    public static BluetoothWrapper pluginWithGameObjectName(string gameObjectName)
    {
        BluetoothWrapper plugin;

        // Get plugin class for the actual platform.
#if UNITY_IPHONE
			//plugin = (BluetoothWrapper)new BluetoothWrapperIOS(gameObjectName);
#elif UNITY_ANDROID
        plugin = (BluetoothWrapper)new BluetoothWrapperAndroid(gameObjectName);
#endif

        return plugin;
    }

    public BluetoothWrapper(string gameObjectName)
    {
        this.gameObjectName = gameObjectName;
        Setup();
    }

    virtual protected void Setup() { }

    #endregion


    #region Features

    virtual public void ShowAlertWithAttributes(string title, string message, string buttonTitle, string cancelButtonTitle) { }

    virtual public void PermissionChecks() { }
    virtual public string BtCheck() { return null; }

    virtual public string BtCheckIsOn() { return null; }

    virtual public void TurnOnBt() {  }

    virtual public string GetDeviceName() { return null; }

    virtual public void EnableDiscoverable() { }

    virtual public string GetPairedDevices() { return null; }

    virtual public void DiscoverDevices() { }

    virtual public void CancelDiscovery() { }

    virtual public void ConnectToDevice(string device) { }

    virtual public void Disconnect() { }

    virtual public void ReceivePair() { }

    virtual public string GetIsReceiving() { return null; }

    virtual public void ReconnectReceive() { }

    virtual public void ReconnectSend() { }

    virtual public void ReconnectCancel() { }

    virtual public void ReceiveData() { }

    virtual public void SendData(string msg) { }

    virtual public void Toast(string msg) { }

    virtual public void Reset() { }

    virtual public void RestrictReceive(bool val) { }

    // ...
    #endregion
}
