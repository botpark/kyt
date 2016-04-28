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
                RFID.Instance.Run(session);
            }

            if (message == "disconnect")
            {
                RFID.Instance.RFIDDisconnect();
            }

            if (message == "pause") {
                RFID.Instance.Stop();
            }

            if (message == "start")
            {
                RFID.Instance.Start();
            }

            if (message == "detect")
            {
                RFID.Instance.DetectOne();
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
