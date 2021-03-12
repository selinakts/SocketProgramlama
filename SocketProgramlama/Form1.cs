using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Threading;



namespace SocketProgramlama
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        int c_tıklama = 0;
        int s_tıklama = 0;
        Thread thread;
        TcpListener listener;
        Socket socket;
        NetworkStream NetworkStream;
        IPAddress IPAddress;
        StreamReader streamReader;
        StreamWriter streamWriter;
        TcpClient tcpClient;
        UdpClient client;




        public void serverokumayabasla()
        {
            socket = listener.AcceptSocket();
            if (socket.Connected)
            {

                while (true)
                {
                    try
                    {
                        NetworkStream = new NetworkStream(socket);
                        streamReader = new StreamReader(NetworkStream);
                        string yazi = streamReader.ReadLine();

                        serverekranabas(yazi);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }
        public void serverekranabas(string s)
        {
            s = "" + s;
            richTextBox1.AppendText(s + "\n");
        }
        private void dinle()
        {
            try
            {
                if (textBox1.Text.Length == 0 || textBox6.Text.Length == 0)
                {
                    MessageBox.Show("Server Port ve Server Nıc alanları boş olamaz. \nLütfen değer giriniz!");

                }
                if (textBox1.Text != null)
                {
                    IPAddress = IPAddress.Parse(textBox1.Text);
                    listener = new TcpListener(IPAddress, Convert.ToInt16(textBox6.Text));
                    listener.Start();
                    thread = new Thread(new ThreadStart(serverokumayabasla));
                    thread.Start();
                    int lengthPre = richTextBox1.Text.Length;
                    richTextBox1.AppendText(DateTime.Now.ToString() + " Dinleme Başlatıldı...\n");
                    richTextBox1.Select(lengthPre, richTextBox1.Text.Length);

                    richTextBox1.SelectionColor = Color.Green;
                    button1.Text = "Close";

                }
                else if (textBox1.Text.Length == 0)
                {
                    MessageBox.Show("Hata Server Port alanı boş olamaz");
                    return;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Girdiğiniz dizinleri kontrol edin. \nHata detayı : " + ex.Message, " Hata",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);

            }
        }
        private void button1_Click(object sender, EventArgs e)//listen button
        {
            s_tıklama++;
            if (s_tıklama % 2 == 1)
            {
                dinle();

            }
            if (s_tıklama % 2 == 0)
            {
                if (NetworkStream != null)
                {
                    NetworkStream.Close();// kapattık
                }



                int lengthPre = richTextBox1.Text.Length;
                richTextBox1.AppendText(DateTime.Now.ToString() + " Dinleme Kapatıldı... \n");
                button1.Text = "Listen";
                richTextBox1.Select(lengthPre, richTextBox1.Text.Length);
                richTextBox1.SelectionColor = Color.Red;
            }

        }

        private void button3_Click(object sender, EventArgs e)//server send button
        {
            if (textBox4.Text == "")

                //Burda bos alan göndermeyi önlüyoruz...
                return;
            else
            {
                streamWriter = new StreamWriter(NetworkStream);
                streamWriter.WriteLine(textBox4.Text);
                streamWriter.Flush();
                richTextBox4.AppendText("SERVER:" + textBox4.Text + "\n");
                richTextBox1.AppendText("SERVER:" + textBox4.Text + "\n");
                textBox4.Text = " ";
            }
        }
        private void clientoku_basla()
        {
            NetworkStream = tcpClient.GetStream();
            streamReader = new StreamReader(NetworkStream);
            while (true)
            {
                try
                {
                    string yazi = streamReader.ReadLine();
                    clientekrana_bas(yazi);
                }
                catch
                {
                    return;
                }
            }
        }
        public void clientekrana_bas(string s)
        {
            s = "" + s;
            richTextBox2.AppendText(s + "\n");
        }
        public void baglanti_kur()
        {
            try
            {
                if (textBox6.Text == textBox3.Text && textBox1.Text == textBox2.Text)
                {
                    String s = textBox2.Text;
                    Int16 i = Convert.ToInt16(textBox3.Text);

                    tcpClient = new TcpClient(s, i);
                    thread = new Thread(new ThreadStart(clientoku_basla));
                    thread.Start();
                    int lengthpre = richTextBox3.Text.Length;
                    richTextBox3.AppendText(DateTime.Now.ToString() + " Baglanti Kuruldu...\n");
                    richTextBox3.Select(lengthpre, richTextBox3.Text.Length);
                    richTextBox3.SelectionColor = Color.Green;
                    button2.Text = "Close";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Girdiğiniz dizinleri kontrol edin \n Baglantı kurulamadı. \nHata detayı : " + ex.Message, " Hata",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);


            }
        }
        private void button2_Click(object sender, EventArgs e)//connect button
        {
            if (textBox6.Text == textBox3.Text && textBox1.Text == textBox2.Text)
            {
                c_tıklama++;
                if (c_tıklama % 2 == 1)
                {
                    baglanti_kur();

                }
                if (c_tıklama % 2 == 0)
                {
                    tcpClient.Close();
                    int lengtpre = richTextBox3.Text.Length;

                    richTextBox3.AppendText(DateTime.Now.ToString() + " Bağlantı Kapatıldı... \n");
                    richTextBox3.Select(lengtpre, richTextBox3.Text.Length);
                    richTextBox3.SelectionColor = Color.Red;
                    button2.Text = "Connect";
                }
            }
            else
            {
                c_tıklama = 0;
                MessageBox.Show("Girdiğiniz dizinleri kontrol edin \n Baglantı kurulamadı");
                return;
            }

        }

        private void button4_Click(object sender, EventArgs e)//client send button
        {
            if (textBox5.Text == "")
                //Burda bos alan göndermeyi önlüyoruz...
                return;
            else
            {
                streamWriter = new StreamWriter(NetworkStream);
                streamWriter.WriteLine(textBox5.Text);
                streamWriter.Flush();
                richTextBox2.AppendText("CLIENT:" + textBox5.Text + "\n");
                richTextBox3.AppendText("CLIENT:" + textBox5.Text + "\n");
                textBox5.Text = "";
            }
        }

        private void sENDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;//unicast sender
            groupBox3.Visible = false;//multicast sender
            groupBox2.Visible = false;//unicast receıver
            groupBox4.Visible = false;//multicast receıver
            groupBox5.Visible = false;//broadcast sender
            groupBox6.Visible = false;//broadcast receıver
        }

        private void rECEIVERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;//unıcast receıver
            groupBox1.Visible = false;//unıcast sender
            groupBox3.Visible = false;//multicast sender
            groupBox4.Visible = false;//multıcast receıver
            groupBox5.Visible = false;//broadcast sender
            groupBox6.Visible = false;//broadcast receıver
        }

        private void sENDERToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;//multıcast sender
            groupBox2.Visible = false;//unıcast receıver
            groupBox1.Visible = false;//unıcast saender
            groupBox4.Visible = false;//multıcast receıver
            groupBox5.Visible = false;//broadcast sender
            groupBox6.Visible = false;//broadcast receıver


        }

        private void sENDERToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = true;//broadcast sender
            groupBox1.Visible = false;//unıcast sender
            groupBox2.Visible = false;//unıcast receıver
            groupBox3.Visible = false;//multıcast sender
            groupBox4.Visible = false;//multıcast receıver
            groupBox6.Visible = false;//broadcast receıver

        }

        private void rECEIVERToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groupBox6.Visible = true;//broadcast receıver
            groupBox1.Visible = false;//unıcast sender
            groupBox2.Visible = false;//unıcast receıver
            groupBox3.Visible = false;//multıcast sender
            groupBox4.Visible = false;//multıcast receıver
            groupBox5.Visible = false;//broadcast sender
        }

        private void rECEIVERToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = true;//multıcast receıver
            groupBox1.Visible = false;//unıcast sender
            groupBox2.Visible = false;//unıcast receıver
            groupBox3.Visible = false;//multıcast sender
            groupBox5.Visible = false;//broadcast sender
            groupBox6.Visible = false;//broadcast receıver
        }

        private void rECEIVERToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            groupBox5.Visible = false;//broadcast sender
            groupBox1.Visible = false;//unıcast sender
            groupBox2.Visible = false;//unıcast receıver
            groupBox3.Visible = false;//multıcast sender
            groupBox4.Visible = true;//multıcast receıver
            groupBox6.Visible = false;//broadcast receıver

        }

    }
    
}
