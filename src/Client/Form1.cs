﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Client
{
    public partial class ClientForm : Form
    {
        private delegate void SafeCallDelegate(string text);
        TcpClient client, timerClient;
        NetworkStream netStream, timerStream;
        public ClientForm()
        {
            InitializeComponent();
            client = new TcpClient();
            client.Connect("127.0.0.1", 3333);
            Thread clientThread = new Thread(new ThreadStart(GetTime));
            clientThread.Start();
        }
        public void GetTime()
        {
            byte[] timerData = new byte[256];
            timerClient = new TcpClient();
            timerClient.Connect("127.0.0.1", 3334);
            timerStream = timerClient.GetStream();
            while (true)
            {
                timerStream.Read(timerData, 0, timerData.Length);
                WriteTextSafe(System.Text.Encoding.UTF8.GetString(timerData));
            }
        }
        private void WriteTextSafe(string text)
        {
            if (label4.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                Invoke(d, new object[] { text });
            }
            else
            {
                label4.Text = text;
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            netStream = client.GetStream();
            Server.RequestFile request = new Server.RequestFile(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(netStream, request);
            //netStream.Write(requestData, 0, requestData.Length);

            using (FileStream fileStream = new FileStream(@"C:\Files\Client\"+request.FileName, FileMode.Create))
            {
                int numberOfBytesRead = 0;
                byte[] data = new byte[256];
                do
                {
                    int bytes = netStream.Read(data, 0, data.Length);
                    fileStream.Write(data, numberOfBytesRead, bytes);
                    numberOfBytesRead += bytes;
                }
                while (netStream.DataAvailable);
            }
        }
    }
}
