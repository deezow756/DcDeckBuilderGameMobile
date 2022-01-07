package com.samsontech.bluetoothplugin;

import android.Manifest;
import android.app.Fragment;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothClass;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothServerSocket;
import android.bluetooth.BluetoothSocket;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.ServerSocket;
import java.util.ArrayList;
import java.util.Set;
import java.util.UUID;

import static android.Manifest.*;

public class BluetoothManager extends Fragment {
    // Constants.
    public static final String TAG = "Bluetooth_Manager_Fragment";
    private static final String CALLBACK_METHOD_DEVICEFOUND = "NewDeviceFound";

    // Unity context.
    String gameObjectName;

    public static BluetoothManager instance;

    public static BluetoothManager getInstance() {
        return instance;
    }

    public BluetoothAdapter btAdapter;

    //region Setup
    public static void start(String gameObjectName) {
        // Instantiate and add to Unity Player Activity.
        instance = new BluetoothManager();
        instance.gameObjectName = gameObjectName; // Store `GameObject` reference
        final int commit = UnityPlayer.currentActivity.getFragmentManager().beginTransaction().add(instance, BluetoothManager.TAG).commit();
    }

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setRetainInstance(true); // Retain between configuration changes (like device rotation)
    }

    public BluetoothManager() {
        btAdapter = BluetoothAdapter.getDefaultAdapter();
        PairedDeviceList = new ArrayList<>();
        NewDeviceList = new ArrayList<BluetoothDevice>();
    }
//endregion

    //region Utils
    public void ShowToast(String messaage)
    {
        try {
            Toast.makeText(getActivity(), messaage, Toast.LENGTH_SHORT).show();
        }
        catch (Exception ex)
        {
            Log.d("ShowToast", "Error Showing Message");
        }
    }

    void SendUnityMessage(String methodName, String parameter)
    {
        try {
            UnityPlayer.UnitySendMessage(gameObjectName, methodName, parameter);
        }
        catch (Exception ex)
        {
            ShowToast("Error Sending Message To Unity" + methodName + parameter);
        }
    }

    public void Reset()
    {
        //try{
            //if(Socket != null && Socket.isConnected())
            //{
                //Socket.close();
                //serverSocket.close();
            //}
        //}
        //catch (Exception ex)
        //{
            //ShowToast("Error Disconnecting");
        //}

        //inputStream = null;
        //outputStream = null;
        //serverSocket = null;
        //Socket = null;
        //CurDevice = null;
        //ReceivePair();
    }
//endregion

    //region Permissions
    public void PermissionChecks() {
        try {
            if (Build.VERSION.SDK_INT >= 23) {

                requestPermissions(new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION}, 1);
                requestPermissions(new String[]{Manifest.permission.READ_EXTERNAL_STORAGE, Manifest.permission.WRITE_EXTERNAL_STORAGE}, 2);
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Getting Permissions");
        }
    }
//endregion

    //region Bluetooth Checks
    public String BtCheck()
    {
        try {
            if (btAdapter == null) {
                return "0";
            } else {
                return "1";
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Checking Bluetooth Compatable");
            return null;
        }
    }

    public String BtCheckIsOn()
    {
        try {
            if (btAdapter.isEnabled()) {
                return "1";
            } else {
                return "0";
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Checking Bluetooth Is On");
            return  null;
        }
    }
    public void TurnOnBt()
    {
        try {
            btAdapter.enable();
            ShowToast("Turning On Bluetooth");
        }
        catch (Exception ex)
        {
            ShowToast("Error Turning On Bluetooth");
        }
    }

    public String GetDeviceName()
    {
        try {
            if (btAdapter.isEnabled()) {
                return btAdapter.getName();
            } else {
                return null;
            }
        }
        catch(Exception e)
        {
            ShowToast(e.getMessage());
            return null;
        }
    }
    //endregion

    //region Bluetooth Pairing

    ArrayList<BluetoothDevice> PairedDeviceList;
    ArrayList<BluetoothDevice> NewDeviceList;
    ArrayList<BluetoothDevice> DevicesFound;

    public void EnableDiscoverable()
    {
        try {
            Intent dIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_DISCOVERABLE);
            dIntent.putExtra(BluetoothAdapter.EXTRA_DISCOVERABLE_DURATION, 300);
            startActivity(dIntent);
        }
        catch (Exception ex)
        {
            ShowToast("Error Enabling Discoverable");
        }
    }

    public String GetPairedDevices()
    {
        try {
            int index = 0;
            Set<BluetoothDevice> btDevices = btAdapter.getBondedDevices();

            if (btDevices.size() > 0) {
                String str = "";

                for (BluetoothDevice device : btDevices) {
                    BluetoothClass bClass = device.getBluetoothClass();
                    if(bClass.getDeviceClass() == BluetoothClass.Device.PHONE_SMART) {
                        PairedDeviceList.add((device));
                        if (index == btDevices.size()) {
                            str += device.getName() + "." + device.getAddress();
                        } else {
                            str += device.getName() + "." + device.getAddress() + ",";
                        }
                    }
                }
                return str;
            } else {
                return "no devices";
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Getting Paired Devices");
            return null;
        }
    }

    public void DiscoverDevices()
    {
        try {
            NewDeviceList.clear();
            DevicesFound = new ArrayList<>();
            try {
                getActivity().unregisterReceiver(statusReceiver);
                IntentFilter i = new IntentFilter();
                i.addAction(BluetoothDevice.ACTION_FOUND);
                getActivity().registerReceiver(receiver, i);
                btAdapter.startDiscovery();
            }
            catch (Exception ex)
            {
                IntentFilter i = new IntentFilter();
                i.addAction(BluetoothDevice.ACTION_FOUND);
                getActivity().registerReceiver(receiver, i);
                btAdapter.startDiscovery();
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Start Discovery");
        }
    }

    public void CancelDiscovery() {
        try {
            if (btAdapter.isDiscovering()) {
                getActivity().unregisterReceiver(receiver);
                btAdapter.cancelDiscovery();
            }
        } catch (Exception ex) {
            ShowToast("Error Cancel Discovery");
        }
    }

    BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (BluetoothDevice.ACTION_FOUND.equals(action)) {
                BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);

                if(!DevicesFound.contains(device)) {
                    DevicesFound.add(device);
                    BluetoothClass bClass = device.getBluetoothClass();
                    if(bClass.getDeviceClass() == BluetoothClass.Device.PHONE_SMART) {
                        NewDeviceList.add(device);
                        //ShowToast(device.getName());
                        SendUnityMessage(CALLBACK_METHOD_DEVICEFOUND, device.getName() + "." + device.getAddress());
                    }
                }
            }
        }
    };

    public void Disconnect()
    {
        Reset();
    }
    //endregion

    //region Bluetooth Connecting Devices

    UUID myUUID = UUID.fromString("862cd111-7592-4453-a1b9-3a388e18a5f5");
    public BluetoothDevice CurDevice;
    public BluetoothSocket Socket;
    public BluetoothServerSocket serverSocket;

    public boolean isReceiving = false;
    public boolean receiveStricted = false;

    public String GetIsReceiving()
    {
        if (isReceiving)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }

    public void ReceivePair()
    {
        try {
            if (btAdapter.isEnabled()) {
                isReceiving = true;
                Thread Receive = new Thread() {
                    @Override
                    public void run() {
                        try {
                            serverSocket = btAdapter.listenUsingRfcommWithServiceRecord("BattleShots", myUUID);
                        } catch (IOException e) {
                            getActivity().runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    ShowToast("Failed To Create Server Socket");
                                }
                            });
                            return;
                        }

                        Socket = null;
                        isReceiving = true;

                        while (Socket == null && isReceiving && !receiveStricted) {
                            try {
                                Socket = serverSocket.accept();
                            } catch (IOException e) {
                            }

                            if (isReceiving == false) {
                                break;
                            }

                            if (Socket != null) {
                                getActivity().runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        CurDevice = Socket.getRemoteDevice();
                                        ShowToast("Connected To: " + CurDevice.getName());
                                        CancelDiscovery();
                                        try {
                                            getActivity().unregisterReceiver(receiver);
                                            IntentFilter i = new IntentFilter();
                                            i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECTED);
                                            i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECT_REQUESTED);
                                            getActivity().registerReceiver(statusReceiver, i);
                                            ReceivedPairCallBack(CurDevice.getName() + "." + CurDevice.getAddress());
                                            SetupInputOutputStreams();
                                        }
                                        catch (Exception ex)
                                        {
                                            IntentFilter i = new IntentFilter();
                                            i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECTED);
                                            i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECT_REQUESTED);
                                            getActivity().registerReceiver(statusReceiver, i);
                                            ReceivedPairCallBack(CurDevice.getName() + "." + CurDevice.getAddress());
                                            SetupInputOutputStreams();
                                        }
                                    }
                                });
                                break;
                            }
                        }
                    }
                };
                Receive.start();
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Starting Receive Bluetooth Connection");
        }
    }

    public void ReceivedPairCallBack(String _device)
    {
        SendUnityMessage("ReceivedPairCallBack", _device);
    }

    public void ConnectToDevice(final String _device)
    {
        try {
            if (btAdapter.isEnabled()) {
                boolean skip = false;
                isReceiving = false;
                CancelDiscovery();
                for (BluetoothDevice device : PairedDeviceList) {
                    String s = device.getName() + "." + device.getAddress();

                    if (s.equals(_device)) {
                        CurDevice = device;
                        skip = true;
                        break;
                    }
                }

                if (!skip) {
                    for (BluetoothDevice device : NewDeviceList) {
                        String s = device.getName() + "." + device.getAddress();
                        if (s.equals(_device)) {
                            CurDevice = device;
                            break;
                        }
                    }
                }
                Thread Connect = new Thread() {
                    @Override
                    public void run() {
                        try {
                            Socket = CurDevice.createRfcommSocketToServiceRecord(myUUID);
                        } catch (Exception e) {
                            getActivity().runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    ShowToast("Failed To Create Socket");
                                    PairedWithDeviceCallBack("0");
                                    ReceivePair();
                                }
                            });
                        }
                        try {
                            Socket.connect();
                            getActivity().runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    ShowToast("Connected To: " + CurDevice.getName());
                                    try {
                                        getActivity().unregisterReceiver(receiver);
                                        IntentFilter i = new IntentFilter();
                                        i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECTED);
                                        i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECT_REQUESTED);
                                        getActivity().registerReceiver(statusReceiver, i);
                                        PairedWithDeviceCallBack("1");
                                        SetupInputOutputStreams();
                                    }
                                    catch (Exception ex)
                                    {
                                        IntentFilter i = new IntentFilter();
                                        i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECTED);
                                        i.addAction(BluetoothDevice.ACTION_ACL_DISCONNECT_REQUESTED);
                                        getActivity().registerReceiver(statusReceiver, i);
                                        PairedWithDeviceCallBack("1");
                                        SetupInputOutputStreams();
                                    }
                                }
                            });
                        } catch (Exception e) {
                            getActivity().runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    PairedWithDeviceCallBack("0");
                                    ShowToast("Failed To Connect");
                                    ReceivePair();
                                }
                            });
                        }
                    }
                };
                Connect.start();
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Connection To Device");
        }
    }

    public void PairedWithDeviceCallBack(String status)
    {
        SendUnityMessage("ConnectedToDeviceCallBack", status);
    }

    BroadcastReceiver statusReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (BluetoothDevice.ACTION_ACL_DISCONNECTED.equals(action)) {
                try {
                    ShowToast("Disconnected From " + CurDevice.getName());
                    DisconnectCallBack();
                    getActivity().unregisterReceiver(statusReceiver);

                    Socket.close();
                    Socket = null;
                    CurDevice = null;
                    serverSocket.close();
                    serverSocket = null;
                    ReceivePair();
                }
                catch(Exception e)
                {
                    ShowToast("failed to disconnect");
                }
            }
            else if (BluetoothDevice.ACTION_ACL_DISCONNECT_REQUESTED.equals(action)) {
                try {
                    ShowToast(CurDevice.getName() + " Is Exiting Game");
                }
                catch(Exception e)
                {
                    ShowToast("failed to disconnect");
                }
            }
        }
    };

    public void DisconnectCallBack()
    {
        SendUnityMessage("DisconnectedCallBack", "1");
    }

    public void RestrictReceive(String val)
    {
        if(val == "0")
        {
            receiveStricted = false;
        }
        else
        {
            receiveStricted = true;
        }
    }

    //endregion

    //region Bluetooth Send/Receive
    protected InputStream inputStream;
    public OutputStream outputStream;

    public void SetupInputOutputStreams() {
        try {
            if (Socket != null) {
                try {
                    inputStream = Socket.getInputStream();
                    outputStream = Socket.getOutputStream();
                    ReceiveData();
                } catch (IOException e) {
                    ShowToast("Failed To Setup Streams");
                }
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Setting Up Input Output Stream");
        }
    }

    public void ReceivedDataCallBack(String data)
    {
        SendUnityMessage("ReceivedDataCallBack", data);
    }

    public String Msg;
    public void ReceiveData()
    {
        try {
            if (Socket.isConnected()) {
                if (inputStream != null && outputStream != null) {
                    Thread _ReceiveData = new Thread() {
                        @Override
                        public void run() {
                            while (Socket.isConnected()) {
                                final byte[] buffer = new byte[1024];
                                int bytes = 0;

                                try {
                                    bytes = inputStream.read(buffer);

                                } catch (IOException e) {
                                    getActivity().runOnUiThread(new Runnable() {
                                        @Override
                                        public void run() {
                                            //ShowToast("Error Receiving Data");
                                        }
                                    });
                                }

                                if (bytes > 0) {
                                    try {
                                        Msg = new String(buffer, "UTF-8");
                                    } catch (IOException e) {
                                        getActivity().runOnUiThread(new Runnable() {
                                            @Override
                                            public void run() {
                                                //ShowToast("Failed To Convert Buffer To String");
                                            }
                                        });
                                    }
                                    getActivity().runOnUiThread(new Runnable() {
                                        @Override
                                        public void run() {
                                            //ShowToast("Received Data: " + Msg);
                                            ReceivedDataCallBack(Msg);
                                        }
                                    });
                                }
                            }
                        }
                    };
                    _ReceiveData.start();
                }
            }
        }
        catch (Exception ex)
        {
            //ShowToast("Error Receiving Data");
        }
    }

    public void DataSentCallBack(String status)
    {
        SendUnityMessage("DataSentCallBack", status);
    }

    public String DataToSend;
    public void SendData(String data)
    {
        try {
            if (Socket.isConnected()) {
                if (inputStream != null && outputStream != null) {
                    DataToSend = data;
                    Thread _SendData = new Thread() {
                        @Override
                        public void run() {
                            try {
                                outputStream.write(DataToSend.getBytes());
                                getActivity().runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        //ShowToast("Message Sent: " + DataToSend);
                                        DataSentCallBack("1");
                                    }
                                });
                            } catch (IOException e) {
                                getActivity().runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        ShowToast("Failed To Send Message");
                                        DataSentCallBack("0");
                                    }
                                });
                            }
                        }
                    };
                    _SendData.start();
                }
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error Sending Data");
        }
    }

    //endregion
}
