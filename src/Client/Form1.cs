using System;
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

namespace Client
{
    public partial class ClientForm : Form
    {
        TcpClient client;
        NetworkStream netStream;
        public ClientForm()
        {
            InitializeComponent();
            client = new TcpClient();
            client.Connect("127.0.0.1", 3333);
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
