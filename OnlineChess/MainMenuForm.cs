using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess{
    public partial class MainMenuForm : Form{

        private NetworkStream stream;
        public String version = "1.0";

        public MainMenuForm(){
            InitializeComponent();
        }

        /*
         * Reads message sent through the network stream
         * and returns the data as a String
         */
        private String ReadMessage(){
            Byte[] bytes = new Byte[256];
            String data = null;
            Int32 i = stream.Read(bytes, 0, bytes.Length);
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            return data;
        }

        /*
         * Takes a string and sends the message
         * through the network stream
         */
        private void WriteMessage(String data){
            Byte[] bytes = new Byte[256];
            bytes = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
        }

        /*
         * Sends a message through the network stream to notify the
         * other player when they disconnect
         */
        private void SendDisconnectMessage(){
            if(stream.CanRead){
                WriteMessage("Disconnected");
                stream.Close();
            }
        }

        private void HostButton_Click(object sender, EventArgs e){
            TcpListener server = null;
            try{
                Int32 port = Convert.ToInt32(PortTextBox.Text);
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, port);
                server.Start();

                InfoLabel.Text = "Waiting for connection...";

                TcpClient client = server.AcceptTcpClient();
                InfoLabel.Text = "Connected!";

                // Get a stream object for reading and writing
                this.stream = client.GetStream();

                String data = ReadMessage();

                if(data == this.version){
                    WriteMessage("Connected");

                    this.Hide();
                    ChessUI chess = new ChessUI(stream, "Black");
                    chess.ShowDialog();

                    SendDisconnectMessage();
                }
                else{
                    WriteMessage("Invalid version");
                }
                
                this.Show();
            }
            catch(SocketException err){
                InfoLabel.Text = "SocketException: " + err;
            }
            finally{
                // Stop listening for new clients
                server.Stop();
            }
        }

        private void JoinButton_Click(object sender, EventArgs e){
            try{
                // Create a TcpClient
                string ip = IPTextBox.Text;
                Int32 port = Convert.ToInt32(PortTextBox.Text);
                TcpClient client = new TcpClient(ip, port);

                InfoLabel.Text = "Connecting to host...";

                // Get a client stream for reading and writing.
                this.stream = client.GetStream();

                WriteMessage("1.0");

                String data = ReadMessage();

                if(data == "Connected"){
                    InfoLabel.Text = "Connected!";
                    this.Hide();
                    ChessUI chess = new ChessUI(stream, "White");
                    chess.ShowDialog();
                }
                else{
                    if(data == "Invalid version"){
                        InfoLabel.Text = "Wrong version!";
                    }
                    else {
                        InfoLabel.Text = data;
                    }
                    stream.Close();
                }
                
                this.Show();

                SendDisconnectMessage();

                // Close everything
                client.Close();
            }
            catch(ArgumentNullException err){
                InfoLabel.Text = "ArgumentNullException: " + err;
            }
            catch(SocketException err){
                InfoLabel.Text = "Could not connect to host";
            }
        }
    }
}
