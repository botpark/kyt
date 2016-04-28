using System;
using System.Threading;
using ReaderB;
using SuperWebSocket;

namespace kyt
{
    class RFID
    {

        private static readonly Lazy<RFID> instance = new Lazy<RFID>(() => new RFID());
        private const string READER_IP = "192.168.1.250";
        private const string READER_PORT = "27011";
        private byte HANDLE_IP = Convert.ToByte("FF", 16);
        private int HANDLE_PORT = 27011;
        private bool READER_CONNECT = false;
        private bool READER_INIT = true;

        private ManualResetEvent EVENT = null;
        private Thread OBSERVER = null;

        private Parser parser = Parser.Instance;

        private RFID() { }

        public static RFID GetInstance
        {
            get
            {
                return instance.Value;
            }
        }

        public void Connect(WebSocketSession res)
        {
            if (!READER_CONNECT)
            {
                try
                {
                    Thread.Sleep(8000);
                    if (READER_INIT)
                    {
                        StaticClassReaderB.CloseNetPort(HANDLE_PORT);
                        READER_INIT = false;
                    }
                    StaticClassReaderB.OpenNetPort(Convert.ToInt32(READER_PORT), READER_IP, ref HANDLE_IP, ref HANDLE_PORT);
                    READER_CONNECT = true;
                    res.Send("Conectando Dispositivo...");
                }
                catch (Exception e)
                {
                    res.Send(e.Message);
                }
            }
            else
            {
                res.Send("Ya se encuentra conectado...");
            }
        }

        public void Disconnect(WebSocketSession res)
        {
            if (READER_CONNECT)
            {
                try
                {
                    Thread.Sleep(8000);
                    READER_CONNECT = false;
                    res.Send("Desconectando Dispositivo...");
                }
                catch (Exception e)
                {
                    res.Send(e.Message);
                }
            }
            else
            {
                res.Send("Ya se encuentra desconectado...");
            }
        }

        public void Detect(WebSocketSession res)
        {
            if (READER_CONNECT == true && READER_INIT == false)
            {
                EVENT = new ManualResetEvent(true);
                OBSERVER = new Thread(() =>
                {

                    while (OBSERVER.IsAlive)
                    {

                        EVENT.WaitOne();

                        dynamic data = parser.GetJSON(TAG());
                        bool state = Convert.ToBoolean(data.state);

                        if (state)
                        {
                            // res.Send(data.tag);
                            res.Send(@"
                                {
                                    'data': {
                                        tag: '" + data.tag + @"',
                                        ant: '" + data.ant + @"'
                                    }
                                }
                            ");
                        }
                    }
                });

                OBSERVER.SetApartmentState(ApartmentState.STA);
                OBSERVER.Start();
            }
            else
            {
                res.Send("Debe primero conectar el RFID");
            }
        }

        public void Pause()
        {
            if (READER_CONNECT == true && READER_INIT == false)
            {
                EVENT.Reset();
            }
        }

        public void Start()
        {
            if (READER_CONNECT == true && READER_INIT == false)
            {
                EVENT.Set();
            }
        }


        private string TAG()
        {
            // action, tag, antena
            byte[] EPC = new byte[5000];
            byte[] maskAdr = parser.GetBytes("0000");
            byte maskLen = Convert.ToByte("00");
            byte[] maskData = parser.GetBytes("00");
            byte Ant = 0;
            int CardNum = 0;
            int longitud = 0;

            int responseReader = StaticClassReaderB.Inventory_G2(ref HANDLE_IP, (byte)0, maskAdr, maskLen, maskData, (byte)0, (byte)0, (byte)0, (byte)0, EPC, ref Ant, ref longitud, ref CardNum, HANDLE_PORT);
            if ((responseReader == 1) | (responseReader == 2) | (responseReader == 3) | (responseReader == 4) | (responseReader == 0xFB))
            {

                byte[] daw = new byte[longitud];   //   EPC Extraido en longitud.
                Array.Copy(EPC, daw, longitud);    //   Copia la info del EPC al arreglo de extaracción

                string TAG_EPC = parser.GetString(daw); // Transforma el EPC extraido en string.

                if (CardNum == 0)
                {
                    // return "false,0,0";
                    return @"
                        {
                            'state': 'false',
                            'tag'  : 'null',
                            'ant'  : 'null'
                        }
                    ";
                }
                else
                {
                    int longitudEPC = daw[0] * 2;
                    string tags = TAG_EPC.Substring(2, longitudEPC);

                    if (tags.Length == longitudEPC && tags != "")
                    {
                        return @"
                            {
                                'state': 'true',
                                'tag'  : '" + tags + @"',
                                'ant'  : '" + Convert.ToString(Ant, 2).Length + @"'
                            }
                        ";
                    }
                    else
                    {
                        // return "false,0,0";
                        return @"
                            {
                                'state': 'false',
                                'tag'  : 'null',
                                'ant'  : 'null'
                            }
                        ";
                    }
                }
            }
            else
            {
                // return "false,0,0";
                return @"
                    {
                        'state': 'false',
                        'tag'  : 'null',
                        'ant'  : 'null'
                    }
                ";
            }

        }

        /*
        private static readonly Lazy<RFID> instance = new Lazy<RFID>(() => new RFID());

        private string _IP_READER = "";
        private string _PORT_READER = "";

        private byte _IP_HANDLE = Convert.ToByte("FF", 16);
        private int _PORT_HANDLE = 0;

        private int connect = -1;

        private Thread observer;


        private ManualResetEvent _event;

        private Parser parser = Parser.Instance;

        private RFID(string ip = "192.168.1.250", string port = "27011")
        {
            this._IP_READER = ip;
            this._PORT_READER = port;
        }

        public static RFID Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public void Run(WebSocketSession res)
        {
            bool con = RFIDConnect();

            if (con) {

                StartDetection(res, con);
                // res.Send("Lector RFID Conectado");
                res.Send(@"
                    {
                        'state'  : true,
                        'message': 'Lector RFID Conectado'
                    }
                ");

            }
            else {
                if (connect != 0) {
                    // res.Send("Cable UTP Desconectado");
                    res.Send(@"
                    {
                        'state'  : false,
                        'message': 'Cable UTP Desconectado'
                    }
                ");
                } 
            }
           
        }

        private void StartDetection(WebSocketSession res, bool conn)
        {
            if (conn)
            {
                try
                {
                    _event = new ManualResetEvent(true);
                    observer = new Thread(() =>
                    {
                       
                        while (observer.IsAlive)
                        {

                            _event.WaitOne();

                            dynamic data = parser.GetJSON(DetectLabel());
                            bool state = Convert.ToBoolean(data.state);

                            if (state)
                            {
                                // res.Send(data.tag);
                                res.Send(@"
                                    {
                                        'data': {
                                            tag: '" + data.tag + @"',
                                            ant: '" + data.ant + @"'
                                        }
                                    }
                                ");
                            }
                        }
                    });

                    observer.SetApartmentState(ApartmentState.STA);
                    observer.Start();
                    _event.Reset();

                }
                catch (Exception e)
                {

                   res.Send(e.Message);

                }
            }

        }

        public void Stop()
        {
            if (connect == 0) {
                _event.Reset();
            }
        }

        public void Start()
        {
            if (connect == 0) {
                _event.Set();
            }
        }

        public void DetectOne()
        {
            if (connect == 0) {
                Start();
                Thread.Sleep(100);
                Stop();
            } 
        }

        // Si es false es porque ya existe una conexión.
        private bool RFIDConnect()
        {
            if (connect != 0)
            {
                try {

                    StaticClassReaderB.OpenNetPort(Convert.ToInt32(_PORT_READER), _IP_READER, ref _IP_HANDLE, ref _PORT_HANDLE);
                    connect = 0;

                } catch(Exception e) {
                    throw e;
                }
                              
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RFIDDisconnect() {

            if (connect == 0) {
                if(StaticClassReaderB.CloseNetPort(_PORT_HANDLE) == 0)
                {
                    connect = -1;
                    observer.Abort();
                    Instance.observer = null;
                }
            }

        }

        private string DetectLabel()
        {
            // action, tag, antena
            byte[] EPC = new byte[5000];
            byte[] maskAdr = parser.GetBytes("0000");
            byte maskLen = Convert.ToByte("00");
            byte[] maskData = parser.GetBytes("00");
            byte Ant = 0;
            int CardNum = 0;
            int longitud = 0;

            int responseReader = StaticClassReaderB.Inventory_G2(ref _IP_HANDLE, (byte)0, maskAdr, maskLen, maskData, (byte)0, (byte)0, (byte)0, (byte)0, EPC, ref Ant, ref longitud, ref CardNum, _PORT_HANDLE);
            if ((responseReader == 1) | (responseReader == 2) | (responseReader == 3) | (responseReader == 4) | (responseReader == 0xFB))
            {

                byte[] daw = new byte[longitud];   //   EPC Extraido en longitud.
                Array.Copy(EPC, daw, longitud);    //   Copia la info del EPC al arreglo de extaracción

                string TAG_EPC = parser.GetString(daw); // Transforma el EPC extraido en string.

                if (CardNum == 0)
                {
                    // return "false,0,0";
                    return @"
                        {
                            'state': 'false',
                            'tag'  : 'null',
                            'ant'  : 'null'
                        }
                    ";
                }
                else
                {
                    int longitudEPC = daw[0] * 2;
                    string tags = TAG_EPC.Substring(2, longitudEPC);

                    if (tags.Length == longitudEPC && tags != "")
                    {
                        return @"
                            {
                                'state': 'true',
                                'tag'  : '" + tags + @"',
                                'ant'  : '" + Convert.ToString(Ant, 2).Length + @"'
                            }
                        ";
                    }
                    else
                    {
                        // return "false,0,0";
                        return @"
                            {
                                'state': 'false',
                                'tag'  : 'null',
                                'ant'  : 'null'
                            }
                        ";
                    }
                }
            }
            else
            {
                // return "false,0,0";
                return @"
                    {
                        'state': 'false',
                        'tag'  : 'null',
                        'ant'  : 'null'
                    }
                ";
            }

        }
        */


    }
}
