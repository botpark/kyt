using System;
using System.ServiceProcess;
using SuperWebSocket;
using SuperSocket.SocketBase;

namespace kyt
{
    public partial class kyt : ServiceBase
    {

        private WebSocketServer appServer;
        private const string _PORT_SOCKET = "2020";

        public kyt()
        {
            InitializeComponent();
        }

        private bool RunServer()
        {
            appServer = new WebSocketServer();
            appServer.Setup(Convert.ToInt32(_PORT_SOCKET));
            appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(Request);
            return appServer.Start();
        }

        private void Request(WebSocketSession session, string message)
        {
            if (message == "connect") {
                RFID.GetInstance.Connect(session);
            }

            if (message == "disconnect")
            {
                RFID.GetInstance.Disconnect(session);
            }
            
            if (message == "detect") {
                RFID.GetInstance.Detect(session);
            }

            if (message == "start") {
                RFID.GetInstance.Start(); ;
            }

            if (message == "pause") {
                RFID.GetInstance.Pause();
            }

        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            RunServer();
        }

        protected override void OnStop() {}
    }
}
