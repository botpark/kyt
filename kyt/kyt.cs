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
            switch (message) {
                case "connect":
                    RFID.GetInstance.Connect(session);
                    break;
                case "disconnect":
                    RFID.GetInstance.Disconnect(session);
                    break;
                case "reset":
                    RFID.GetInstance.Detect(session);
                    break;
                case "start":
                    RFID.GetInstance.Start();
                    break;
                case "pause":
                    RFID.GetInstance.Pause();
                    break;
                case "single":
                    RFID.GetInstance.DetectOne();
                    break;
                default: session.Send("Evento No Registrado.");
                    break;
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

        protected override void OnStop() {
            RFID.GetInstance.Disconnect(null);
        }
    }
}
