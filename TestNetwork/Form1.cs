using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;


namespace TestNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void btStart_Click(object sender, EventArgs e)
        {
            if (txtPort.Text =="")
            {
                MessageBox.Show("กรุณากรอกเลข Port ! ! !");
            }
            else
            {

                string outputSer;
                byte[] data = new byte[1024];
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Convert.ToInt32(txtPort.Text));
                //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
                Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {                    
                    int recv;
                    byte[] dataClient = new byte[1024];
                    newsock.Bind(ipep);
                    newsock.Listen(10);
                    outputSer = "Waiting for a client...";
                    listBoxServer.Items.Add(outputSer);
                    //Console.WriteLine("Waiting for a client...");
                    
                    Socket client = newsock.Accept();                    
                    IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;                    
                    outputSer = "Connected with :" + " " + clientep.Address.ToString() + " " + "at port" + " " + clientep.Port.ToString();
                    //Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);
                    listBoxServer.Items.Add(outputSer);
                    outputSer = "StartTime :" + " " + DateTime.Now.ToString(); 
                    listBoxServer.Items.Add(outputSer);
                    
                    
                    string welcome = "Welcome to my test server";
                    dataClient = Encoding.ASCII.GetBytes(welcome);
                    client.Send(dataClient, dataClient.Length, SocketFlags.None);


                    //Receive Client
                 

                       while (true)
                        {
                            dataClient = new byte[1024];
                            recv = client.Receive(dataClient);
                            if (recv == 0)
                                break;

                            //outputSer = "Receive :" + Encoding.ASCII.GetString(dataClient, 0, recv);
                            //listBoxServer.Items.Add(outputSer);
                            //Console.WriteLine(Encoding.ASCII.GetString(dataClient, 0, recv));
                            client.Send(dataClient, recv, SocketFlags.None);
                        }
                

                        outputSer = "Disconnected from :" + " " + clientep.Address.ToString();
                        listBoxServer.Items.Add(outputSer);
                        //Console.WriteLine("Disconnected from {0}", clientep.Address);
                        outputSer = "EndTime :" + " " + DateTime.Now.ToString();
                        listBoxServer.Items.Add(outputSer);
                        outputSer = "************************************************************************************";
                        listBoxServer.Items.Add(outputSer);
                        client.Close();
                        newsock.Close();                                           
                }
                catch (SocketException se)
                {

                }
           }
        }

        private void btProcess_Click(object sender, EventArgs e)
        {                       
            Process[] allProcs = Process.GetProcesses();
            foreach (Process thisProc in allProcs)
            {
                try
                {                    
                    string procName = thisProc.ProcessName;
                    DateTime started = thisProc.StartTime;
                    int procID = thisProc.Id;
                    int memory = thisProc.VirtualMemorySize;
                    int priMemory = thisProc.PrivateMemorySize;
                    int physMemory = thisProc.WorkingSet;
                    int priority = thisProc.BasePriority;
                    ProcessPriorityClass priClass = thisProc.PriorityClass;
                    TimeSpan cpuTime = thisProc.TotalProcessorTime;
                    int total = (memory + priMemory + physMemory) / 10000000;
                    string outputProc;

                    outputProc = "" + procName;
                    listBox1.Items.Add(outputProc);

                    outputProc = "" + procID;
                    listBox2.Items.Add(outputProc);

                    outputProc = "" + started;
                    listBox3.Items.Add(outputProc);

                    if (cpuTime.ToString() != "00:00:00")
                    {
                        String tim = "0.1 %";
                        outputProc = "" + tim;
                        listBox4.Items.Add(outputProc);
                    }

                    outputProc = "" + priClass.ToString() + " (" + priority + ")";
                    listBox5.Items.Add(outputProc);

                    outputProc = "" + total.ToString() + " KB";
                    listBox6.Items.Add(outputProc);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void buttonPingA_Click(object sender, EventArgs e)
        {
            
            if (textBoxPing.Text == "")
            {
                MessageBox.Show("กรุณากรอก IP Address หรือ Hostname ! ! !");
            }
            else
            {

                string output;
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;

                double Tmax = 0, Tmin = 0, Tsum = 0, Tavg = 0, p1 = 0;
                int ix = 0, iy = 0, iz = 0, b = 0;
                string v1 = "";

                for (int a = 1; a > b; a++)
                {
                    if (a == 2) break;
                    {
                        //listBoxPing.Items.Add(a);
                    }

                    try
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            PingReply reply = pingSender.Send(textBoxPing.Text, timeout, buffer, options);

                            ix = i + 1;
                            output = "";
                            String system = "";
                            if (reply.Status == IPStatus.Success)
                            {
                                if (i == 0)
                                {
                                    Tmin = reply.RoundtripTime;
                                    Tmax = reply.RoundtripTime;
                                }
                                v1 = reply.Address.ToString();
                                output = output + "Reply from : " + reply.Address.ToString() + " ";
                                output = output + "RoundTrip time : " + reply.RoundtripTime + " ";
                                output = output + "Time to live (TTL) : " + reply.Options.Ttl +" ";
                                output = output + "Don't fragment : " + reply.Options.DontFragment + " ";
                                output = output + "Buffer size (Bytes) : " + reply.Buffer.Length;
                                listBoxPing.Items.Add(output);
                                if (reply.Options.Ttl == 64)
                                {
                                    system = "ระบบปฏิบัติการ : Linux หรือ Router(ขนาดเล็ก)";
                                }
                                else if (reply.Options.Ttl == 128)
                                {
                                    system = "ระบบปฏิบัติการ : X68 (Microsoft Windows)";
                                }
                                else if (reply.Options.Ttl == 254)
                                {
                                    system = "Router ขนาดกลาง,ใหญ่";
                                }
                                else
                                {
                                    if (reply.Options.Ttl < 64)
                                    {
                                        int sum, ttl = 64, rou = reply.Options.Ttl;
                                        sum = ttl - rou;
                                        system = "จำนวน Router ที่ Packet วิ่งผ่าน : " + sum;
                                    }
                                    else if (reply.Options.Ttl < 128 & reply.Options.Ttl > 64)
                                    {
                                        int sum, ttl = 128, rou = reply.Options.Ttl;
                                        sum = ttl - rou;
                                        system = "จำนวน Router ที่ Packet วิ่งผ่าน : " + sum;
                                    }
                                    else if (reply.Options.Ttl < 254 & reply.Options.Ttl > 128)
                                    {
                                        int sum, ttl = 254, rou = reply.Options.Ttl;
                                        sum = ttl - rou;
                                        system = "จำนวน Router ที่ Packet วิ่งผ่าน : " + sum;
                                    }

                                }
                                listBoxPing.Items.Add(system);
                                iy = iy + 1;
                                Tmax = Math.Max(Tmax, reply.RoundtripTime);
                                Tmin = Math.Min(Tmin, reply.RoundtripTime);
                                Tsum = Tsum + reply.RoundtripTime;

                            }
                            else
                            {
                                iz = iz + 1;
                                output = output + reply.Status.ToString();
                                listBoxPing.Items.Add(output);
                            }
                        }

                        Tavg = Tsum / iy;
                        p1 = iz / ix;
                        output = "Ping statistics for : " + v1;
                        listBoxPing.Items.Add(output);

                        output = "Packets: Sent = " + ix.ToString() + " Received = " + iy + " Lost = " + iz + "(" + p1 + "% loss)";
                        listBoxPing.Items.Add(output);

                        output = "Apporximate Round Trip Time in milli-seconds";
                        listBoxPing.Items.Add(output);

                        output = "minimum = " + Tmin + " ms, Maximum = " + Tmax + " ms, Average = " + Tavg + " ms";
                        listBoxPing.Items.Add(output);

                        output = "************************************************************************************************************************************************************************";
                        listBoxPing.Items.Add(output);
                    }
                    catch (PingException ex)
                    {

                    }
                }
            }
        }

        private void btdomain_Click(object sender, EventArgs e)
        {
            String H = txtdomain.Text;
            String outputDo;
            try
            {
                IPHostEntry host;

                host = Dns.GetHostEntry(H);

                if (txtdomain.Text == "")
                {
                    MessageBox.Show("กรุณากรอกชื่อ Domain ! !");
                }
                else
                {

                    //Console.WriteLine("GetHostEntry({0}) returns:", H);
                    outputDo = "GetHostEntry returns : " + H;
                    listBoxdomain.Items.Add(outputDo);


                    foreach (IPAddress ip in host.AddressList)
                    {
                        outputDo = "IPAddress : " + ip.ToString();
                        listBoxdomain.Items.Add(outputDo);
                        //Console.WriteLine("  {0}", ip);
                    }
                }
            }
            catch (Exception ex)
            {
                outputDo = "error" + ex;
                listBoxdomain.Items.Add(outputDo);
               // Console.WriteLine("error", ex); 
            }
                

        }

        private void btnMachine_Click(object sender, EventArgs e)
        {
            string addr = txtMachine.Text;
            string buffer;

            Object state = new Object();
            IPHostEntry iphe = Dns.GetHostEntry(addr);

            if (txtMachine.Text == "")
            {
                MessageBox.Show("กรุณากรอกชื่อเครื่องคอมพิวเตอร์ ! ! !");
            }
            else
            {

                lstResult.Items.Clear();
                buffer = "Host name : " + iphe.HostName;
                lstResult.Items.Add(buffer);


                foreach (string alias in iphe.Aliases)
                {
                    buffer = "Alias : " + alias;
                    lstResult.Items.Add(buffer);
                }



                foreach (IPAddress addrs in iphe.AddressList)
                {
                    buffer = "";
                    string output1 = addrs.ToString();
                    int chk1 = output1.Length;
                    if (chk1 <= 16)
                    {
                        buffer = "Address (IPv4): " + addrs.ToString();
                    }
                    else
                    {
                        buffer = "Address (IPv6) : " + addrs.ToString();
                    }
                    lstResult.Items.Add(buffer);
                    //lstResult.Items.Add(chk1.ToString());
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}