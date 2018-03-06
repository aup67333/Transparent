using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;


namespace  CameraObjectROI

{
    class HandleServerEye
    {
        public delegate void ShowMsgCallback(string msg);
        public delegate void EyeTrackingCallback(int pNum, double[] ePos);

        #region private variable
        //please refer package formate
        private static int ADDR_HEADER = 0;
        private static int ADDR_SUB = 1;
        private static int ADDR_SIZE_LOW1 = 2;
        private static int ADDR_SIZE_LOW2 = 3;
        private static int ADDR_SIZE_HIGH1 = 4;
        private static int ADDR_SIZE_HIGH2 = 5;
        private static int ADDR_CMD = 6;
        private static int ADDR_TAIL_SIZE = 3;


        TcpClient clientSocket = new TcpClient();
        ShowMsgCallback m_showMsgCallback = null;
        EyeTrackingCallback m_eyeTrackingCallback = null;
        string msg = "";
        Thread ctThread = null;

        //CRC16 TABLE
        private static ushort[] crc16tab = new ushort[] {
          0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
          0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
          0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
          0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
          0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
          0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
          0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
          0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
          0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
          0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
          0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
          0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
          0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
          0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
          0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
          0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
          0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
          0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
          0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
          0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
          0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
          0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
          0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
          0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
          0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
          0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
          0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
          0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
          0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
          0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
          0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
          0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0
        };
        #endregion
        #region private method
        private static ushort CRCode(byte[] buf, int len)
        {
            int counter;
            ushort crc = 0;
            for (counter = 0; counter < len; counter++)
            {
                byte index = (byte)(((crc >> 8) ^ buf[counter]) & 0x00FF);
                crc = (ushort)((crc << 8) ^ crc16tab[index]);
            }
            return crc;

        }
        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>   
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>   
        /// <returns> Returns an array of bytes. </returns>   
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>   
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>   
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>   
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }
        private static byte[] GetBytesDouble(double argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        private static double BAToDouble(byte[] bytes, int index)
        {
            double value = BitConverter.ToDouble(bytes, index);
            return value;
        }

        private void SendCommand(byte[] data)
        {
            if (clientSocket.Connected)
            {

                //取得網路串流物件，取得來自 socket client 的訊息
                NetworkStream serverStream = clientSocket.GetStream();


                //-------------------------EnCode--------------------------------    
                int length = data.Length;
                int len = ADDR_CMD + length + ADDR_TAIL_SIZE;
                byte[] sendBytes = new byte[len];

                sendBytes[ADDR_HEADER] = 0x02;
                sendBytes[ADDR_SUB] = 0x00;
                sendBytes[ADDR_SIZE_LOW1] = (byte)(0xff & length);
                sendBytes[ADDR_SIZE_LOW2] = (byte)(0xff & (length >> 8));
                sendBytes[ADDR_SIZE_HIGH1] = (byte)(0xff & (length >> 16));
                sendBytes[ADDR_SIZE_HIGH2] = (byte)(0xff & (length >> 24));
                for (int i = 0; i < length; i++)
                    sendBytes[ADDR_CMD + i] = data[i];
                ushort CRCValue = CRCode(data, length);
                sendBytes[ADDR_CMD + length] = (byte)(0xff & CRCValue);//for debug
                sendBytes[ADDR_CMD + length + 1] = (byte)(0xff & (CRCValue >> 8));
                sendBytes[ADDR_CMD + length + 2] = 0x03;

                byteArySubstitute(ref sendBytes);

                serverStream.Write(sendBytes, 0, sendBytes.Length);
                serverStream.Flush();

                //string sendstr = System.Text.Encoding.ASCII.GetString(data);
                string sendstr = ByteArrayToHexString(sendBytes);
                //msg(sendstr);
            }
        }
        
        private void byteArySubstitute(ref byte[] sendBytes)
        {
            ArrayList idxAry = new ArrayList();
            for (int i = 1; i < sendBytes.Length; i++)
            {
                if (sendBytes[i] == sendBytes[0])
                {
                    idxAry.Add(i);
                }
            }
            if (idxAry.Count == 0)
                return;

            //Find Substitute Value
            int[] valCountAry = new int[256];
            for (int ii = 0; ii < sendBytes.Length; ii++)
            {
                valCountAry[(int)(sendBytes[ii])]++;
            }
            byte subVal = 0x00;
            for (int i = 1; i < valCountAry.Length; i++)
            {
                if (valCountAry[i] == 0)
                {
                    subVal = (byte)i;
                    i = valCountAry.Length;
                }
            }
            for (int i = 0; i < idxAry.Count; i++)
            {
                sendBytes[(int)(idxAry[i])] = subVal;
            }
            sendBytes[ADDR_SUB] = subVal;

        }
        private void cmdProcess(byte[] readBytes)
        {
            byte cmd = readBytes[0];
            switch (cmd)
            {
                case 0x91:
                    //Start Output Gaze Vector
                    
                    break;
                case 0x92:
                    //Stop Output Gaze Vector
                   
                    break;
                case 0x93:
                    //Output one Gaze Vector
                    //Decode Command Parameter
                    int pNum = Convert.ToInt32(readBytes[1]);
                    double[] wPosAry = new double[pNum * 3];
                    for (int k = 0; k < pNum; k++)
                    {
                        byte[] tmpAry = new byte[8];
                        for (int i = 0; i < 8; i++)
                        {
                            tmpAry[i] = readBytes[3 + k * 24 + i];
                        }
                        wPosAry[k * 3] = BAToDouble(tmpAry, 0);
                        for (int i = 0; i < 8; i++)
                        {
                            tmpAry[i] = readBytes[3 + k * 24 + 8 + i];
                        }
                        wPosAry[k * 3 + 1] = BAToDouble(tmpAry, 0);
                        for (int i = 0; i < 8; i++)
                        {
                            tmpAry[i] = readBytes[3 + k * 24 + 16 + i];
                        }
                        wPosAry[k * 3 + 2] = BAToDouble(tmpAry, 0);
                    }
                    /*
                    string eyeResult = "pNum:" + pNum.ToString() + "\n";
                    for(int i=0;i<pNum;i++){
                        eyeResult += ( "(" + (wPosAry[i*3].ToString("#0.00")) + ","+ (wPosAry[i*3+1].ToString("#0.00")) + ","+ (wPosAry[i*3+2].ToString("#0.00")) + ")\n");
                    }
                    m_showMsgCallback(eyeResult);
                    */ 
                    m_eyeTrackingCallback(pNum, wPosAry);
                    break;

            }

        }
        private void DecodePackage(byte[] bytesFrom, int requestCount)
        {

            for (int i = 0; i < bytesFrom.Length; i++)
            {
                if (bytesFrom[i] == 0x02)//Find Head
                {
                    int rcvlength = 0;
                    int data_len = 0;

                    //--------------------------------------------------------------------------------------
                    //Decode
                    //--------------------------------------------------------------------------------------
                    if (bytesFrom[i + ADDR_SUB] != 0x00)
                    {
                        if (bytesFrom[i + ADDR_SIZE_LOW1] == bytesFrom[i + ADDR_SUB])
                            bytesFrom[i + ADDR_SIZE_LOW1] = 0x02;
                        if (bytesFrom[i + ADDR_SIZE_LOW2] == bytesFrom[i + ADDR_SUB])
                            bytesFrom[i + ADDR_SIZE_LOW2] = 0x02;
                        if (bytesFrom[i + ADDR_SIZE_HIGH1] == bytesFrom[i + ADDR_SUB])
                            bytesFrom[i + ADDR_SIZE_HIGH1] = 0x02;
                        if (bytesFrom[i + ADDR_SIZE_HIGH2] == bytesFrom[i + ADDR_SUB])
                            bytesFrom[i + ADDR_SIZE_HIGH2] = 0x02;
                    }
                    data_len = (int)((0xff & bytesFrom[i + ADDR_SIZE_LOW1]) + (0xffff & ((int)bytesFrom[i + ADDR_SIZE_LOW2] << 8)) + (0xffffff & ((int)bytesFrom[i + ADDR_SIZE_HIGH1] << 16)) + (0xffffffff & ((int)bytesFrom[i + ADDR_SIZE_HIGH2] << 24)));
                    rcvlength = data_len + 9;

                    if (bytesFrom[i + 1] != 0x00)
                    {
                        for (int j = 6; j < rcvlength; j++)
                        {
                            if (bytesFrom[i + j] == bytesFrom[i + ADDR_SUB])
                            {
                                bytesFrom[i + j] = 0x02;
                            }
                        }
                    }


                    byte[] readBytes = new byte[data_len];
                    for (int j = 0; j < data_len; j++)
                        readBytes[j] = (byte)bytesFrom[i + ADDR_CMD + j];

                    ushort CRCValue = CRCode(readBytes, data_len);
                    ushort readCRCValue = (ushort)(bytesFrom[i + ADDR_CMD + data_len] + ((ushort)bytesFrom[i + ADDR_CMD + data_len + 1] << 8));


                    if (readCRCValue != CRCValue)
                    {
                        //Console.WriteLine(" >> " + "From client(" + this.clNo + ") => {" + requestCount.ToString() + "}: CRC Error");
                        msg = "From server(Eye) => {" + requestCount.ToString() + "}: CRC Error";
                        m_showMsgCallback(msg);
                    }
                    //if(buffer[ADDR_CMD + length + 2] != 0x03)
                    //--------------------------------------------------------------------------------------


                    cmdProcess(readBytes);
                    //string rcvstr = ByteArrayToHexString(readBytes);
                    //Console.WriteLine(" >> " + "From client(" + this.clNo + ") => {" + requestCount.ToString() + "}: " + rcvstr);
                    //msg = "From server(Eye) => {" + requestCount.ToString() + "}: " + rcvstr;
                    //m_showMsgCallback(msg);

                    i += rcvlength;


                }
            }
            
            
        }
        private void doChat()
        {
            int requestCount = 0;
            int len = clientSocket.ReceiveBufferSize;
            byte[] bytesFrom = new byte[len];
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);


                    //Process Receive Buffer Data
                    byte[] bytesFromCopy = new byte[bytesFrom.Length];
                    bytesFrom.CopyTo(bytesFromCopy, 0);
                    DecodePackage(bytesFromCopy, requestCount);

                    for (int i = 0; i < bytesFrom.Length; i++)
                    {
                        bytesFrom[i] = 0;
                    }

                }
                catch (Exception ex)
                {
                    clientSocket.Close();
                    //Console.WriteLine(" >> " + "Server to clinet(" + clNo + ") closed...");
                    msg = "Server(Eye) to clinet closed...";
                    m_showMsgCallback(msg);
                    break;
                }
            }
        }
        #endregion

        public HandleServerEye(string ip, int port)
        {
            
            clientSocket.Connect(ip, port);

            ctThread = new Thread(doChat);
            ctThread.Start();
        }

        ~HandleServerEye()
        {
            if (ctThread.IsAlive)
            {
                ctThread.Abort();
            }
            
        }

        public void GetOneEyeReport()
        {
            byte[] cmd = new byte[1];
            cmd[0] = 0x13;
            SendCommand(cmd);
        }
        public void StartEyeReport()
        {
            byte[] cmd = new byte[1];
            cmd[0] = 0x11;
            SendCommand(cmd);
        }
        public void StopEyeReport()
        {
            byte[] cmd = new byte[1];
            cmd[0] = 0x12;
            SendCommand(cmd);
        }
        public void registerCallbackFcn(EyeTrackingCallback eyeTrackingCallback, ShowMsgCallback showMsgCallback)
        {
            m_eyeTrackingCallback = eyeTrackingCallback;
            m_showMsgCallback = showMsgCallback;
        }
        public void Close()
        {
            
           
            try
            {
                if (ctThread.IsAlive)
                {
                    ctThread.Abort();
                }

                clientSocket.GetStream().Close();
                clientSocket.Close();
            }
            catch
            {

            }

        }
    }
}
