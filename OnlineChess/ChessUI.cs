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
namespace OnlineChess {
    public partial class ChessUI : Form{

        private NetworkStream stream;
        private ChessModel game;
        private Button[] buttons = new Button[64];

        public ChessUI(NetworkStream stream, String color){
            InitializeComponent();
            GenerateButtonArray();
            this.stream = stream;
            this.game = new ChessModel(color);
            if(color == "Black"){
                this.Text = "Chess - Host";
                this.InfoLabel.Text = "Your opponent goes first";
            }
            else{
                this.Text = "Chess - Client";
                this.InfoLabel.Text = "It's your turn first";
            }
            UpdateBoard();
            if(!game.IsTurn()){
                WaitForResponse();
            }
        }

        /*
         * Updates the GUI game board too reflect
         * what is stored in ChessModel
         */
        private void UpdateBoard(){
            List<ChessPiece> board = game.GetBoard();

            for(int i = 0; i < 64; i++){
                ChessPiece piece = board.ElementAt(i);

                if(piece.GetColor() == "Black"){
                    if(piece.GetName() == "Bishop"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.bb;
                    }
                    else if(piece.GetName() == "King"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.bk;
                    }
                    else if(piece.GetName() == "Knight"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.bn;
                    }
                    else if(piece.GetName() == "Pawn"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.bp;
                    }
                    else if(piece.GetName() == "Queen"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.bq;
                    }
                    else if(piece.GetName() == "Rook"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.br;
                    }
                }
                else if(piece.GetColor() == "White"){
                    if(piece.GetName() == "Bishop"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.wb;
                    }
                    else if(piece.GetName() == "King"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.wk;
                    }
                    else if(piece.GetName() == "Knight"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.wn;
                    }
                    else if(piece.GetName() == "Pawn"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.wp;
                    }
                    else if(piece.GetName() == "Queen"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.wq;
                    }
                    else if(piece.GetName() == "Rook"){
                        buttons[i].Image = global::OnlineChess.Properties.Resources.wr;
                    }
                }
                else {
                    buttons[i].Image = null;
                }
            }
        }

        /*
         * Updates the game based off data sent to the player
         * through the network stream. Updates the game if a move
         * was made or ends the game if the other player disconnected
         * or a cheat was detected.
         */
        public void UpdateGame(String data){
            String[] dataStrings = data.Split(',');
            if(dataStrings[0] == "Moved"){
                String opponentColor;
                int boardSize = game.GetBoard().Count() - 1;
                ChessPiece p1 = game.GetBoard().ElementAt(boardSize - Convert.ToInt32(dataStrings[1]));
                ChessPiece p2 = game.GetBoard().ElementAt(boardSize - Convert.ToInt32(dataStrings[2]));
                if(game.GetColor() == "Black"){
                    opponentColor = "White";
                }
                else {
                    opponentColor = "Black";
                }
                game.MovePieces(p1, p2, opponentColor);
                UpdateBoard();
                InfoLabel.Text = game.GetGameInfo();
                if(game.GetRecentMove() == "Invalid move!"){
                    InfoLabel.Text = "Opponent cheated. You won!";
                    WriteMessage("Cheated");
                    game.EndGame();
                    stream.Close();
                }
                else {
                    game.ChangeTurn();
                    UpdateBoard();
                    if(game.IsGameOver() == true){
                        stream.Close();
                    }
                }
            }
            else if(dataStrings[0] == "Disconnected"){
                InfoLabel.Text = "Opponent disconnected. You won!";
                game.EndGame();
                stream.Close();
            }
            else if(dataStrings[0] == "Cheated"){
                InfoLabel.Text = "You cheated. You lost the game!";
                game.EndGame();
                stream.Close();
            }

        }

        /*
         * Asyncronously waits for data to be received through
         * the network stream. Once data is received, it is
         * sent to UpdateGame() to update the game.
         */
        private async void WaitForResponse(){
            try{
                Byte[] bytes = new Byte[256];
                String data = null;
                Int32 i = await stream.ReadAsync(bytes, 0, bytes.Length);
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                UpdateGame(data);
            }
            catch(ObjectDisposedException err){
                InfoLabel.Text = "Opponent disconnected. You won!";
                game.EndGame();
                stream.Close();
            }
        }

        /*
         * Sends a string of data through the network stream
         */
        private void WriteMessage(String data){
            Byte[] bytes = new Byte[256];
            bytes = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void OnButtonClick(object sender, EventArgs e, int button){
            if(game.IsGameOver() == false){
                if(game.IsTurn() == true){
                    game.SelectPiece(button);
                    UpdateBoard();
                    InfoLabel.Text = game.GetGameInfo();
                    if(game.IsTurn() == false){
                        WriteMessage("Moved," + game.GetRecentMove());

                        if(game.IsGameOver() == false){
                            WaitForResponse();
                        }
                        else {
                            stream.Close();
                        }
                    }
                }
                else{
                    InfoLabel.Text = "It is not your turn";
                }
            }
        }

        private void BoardButton0_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 0);
        }

        private void BoardButton1_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 1);
        }

        private void BoardButton2_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 2);
        }

        private void BoardButton3_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 3);
        }

        private void BoardButton4_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 4);
        }

        private void BoardButton5_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 5);
        }

        private void BoardButton6_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 6);
        }

        private void BoardButton7_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 7);
        }

        private void BoardButton8_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 8);
        }

        private void BoardButton9_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 9);
        }

        private void BoardButton10_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 10);
        }

        private void BoardButton11_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 11);
        }

        private void BoardButton12_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 12);
        }

        private void BoardButton13_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 13);
        }

        private void BoardButton14_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 14);
        }

        private void BoardButton15_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 15);
        }

        private void BoardButton16_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 16);
        }

        private void BoardButton17_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 17);
        }

        private void BoardButton18_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 18);
        }

        private void BoardButton19_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 19);
        }

        private void BoardButton20_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 20);
        }

        private void BoardButton21_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 21);
        }

        private void BoardButton22_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 22);
        }

        private void BoardButton23_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 23);
        }

        private void BoardButton24_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 24);
        }

        private void BoardButton25_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 25);
        }

        private void BoardButton26_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 26);
        }

        private void BoardButton27_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 27);
        }

        private void BoardButton28_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 28);
        }

        private void BoardButton29_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 29);
        }

        private void BoardButton30_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 30);
        }

        private void BoardButton31_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 31);
        }

        private void BoardButton32_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 32);
        }

        private void BoardButton33_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 33);
        }

        private void BoardButton34_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 34);
        }

        private void BoardButton35_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 35);
        }

        private void BoardButton36_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 36);
        }

        private void BoardButton37_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 37);
        }

        private void BoardButton38_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 38);
        }

        private void BoardButton39_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 39);
        }

        private void BoardButton40_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 40);
        }

        private void BoardButton41_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 41);
        }

        private void BoardButton42_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 42);
        }

        private void BoardButton43_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 43);
        }

        private void BoardButton44_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 44);
        }

        private void BoardButton45_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 45);
        }

        private void BoardButton46_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 46);
        }

        private void BoardButton47_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 47);
        }

        private void BoardButton48_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 48);
        }

        private void BoardButton49_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 49);
        }

        private void BoardButton50_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 50);
        }

        private void BoardButton51_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 51);
        }

        private void BoardButton52_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 52);
        }

        private void BoardButton53_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 53);
        }

        private void BoardButton54_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 54);
        }

        private void BoardButton55_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 55);
        }

        private void BoardButton56_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 56);
        }

        private void BoardButton57_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 57);
        }

        private void BoardButton58_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 58);
        }

        private void BoardButton59_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 59);
        }

        private void BoardButton60_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 60);
        }

        private void BoardButton61_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 61);
        }

        private void BoardButton62_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 62);
        }

        private void BoardButton63_Click(object sender, EventArgs e){
            OnButtonClick(sender, e, 63);
        }

        private void GenerateButtonArray(){
            this.buttons[0] = BoardButton0;
            this.buttons[1] = BoardButton1;
            this.buttons[2] = BoardButton2;
            this.buttons[3] = BoardButton3;
            this.buttons[4] = BoardButton4;
            this.buttons[5] = BoardButton5;
            this.buttons[6] = BoardButton6;
            this.buttons[7] = BoardButton7;
            this.buttons[8] = BoardButton8;
            this.buttons[9] = BoardButton9;
            this.buttons[10] = BoardButton10;
            this.buttons[11] = BoardButton11;
            this.buttons[12] = BoardButton12;
            this.buttons[13] = BoardButton13;
            this.buttons[14] = BoardButton14;
            this.buttons[15] = BoardButton15;
            this.buttons[16] = BoardButton16;
            this.buttons[17] = BoardButton17;
            this.buttons[18] = BoardButton18;
            this.buttons[19] = BoardButton19;
            this.buttons[20] = BoardButton20;
            this.buttons[21] = BoardButton21;
            this.buttons[22] = BoardButton22;
            this.buttons[23] = BoardButton23;
            this.buttons[24] = BoardButton24;
            this.buttons[25] = BoardButton25;
            this.buttons[26] = BoardButton26;
            this.buttons[27] = BoardButton27;
            this.buttons[28] = BoardButton28;
            this.buttons[29] = BoardButton29;
            this.buttons[30] = BoardButton30;
            this.buttons[31] = BoardButton31;
            this.buttons[32] = BoardButton32;
            this.buttons[33] = BoardButton33;
            this.buttons[34] = BoardButton34;
            this.buttons[35] = BoardButton35;
            this.buttons[36] = BoardButton36;
            this.buttons[37] = BoardButton37;
            this.buttons[38] = BoardButton38;
            this.buttons[39] = BoardButton39;
            this.buttons[40] = BoardButton40;
            this.buttons[41] = BoardButton41;
            this.buttons[42] = BoardButton42;
            this.buttons[43] = BoardButton43;
            this.buttons[44] = BoardButton44;
            this.buttons[45] = BoardButton45;
            this.buttons[46] = BoardButton46;
            this.buttons[47] = BoardButton47;
            this.buttons[48] = BoardButton48;
            this.buttons[49] = BoardButton49;
            this.buttons[50] = BoardButton50;
            this.buttons[51] = BoardButton51;
            this.buttons[52] = BoardButton52;
            this.buttons[53] = BoardButton53;
            this.buttons[54] = BoardButton54;
            this.buttons[55] = BoardButton55;
            this.buttons[56] = BoardButton56;
            this.buttons[57] = BoardButton57;
            this.buttons[58] = BoardButton58;
            this.buttons[59] = BoardButton59;
            this.buttons[60] = BoardButton60;
            this.buttons[61] = BoardButton61;
            this.buttons[62] = BoardButton62;
            this.buttons[63] = BoardButton63;
        }
    }
}
