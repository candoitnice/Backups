using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

public enum ConnectState
{
    Connecting,
    Error,
    Connected,
    DisConnect,
    Close,
    Closing,
    Open
}
namespace SportClient
{

    public interface IClient
    {
      
        void OnDisconnect();

        void OnRecivedData(Parameter pararme);
      
        void OnConnectStateChange(ConnectState state);

    }
}
