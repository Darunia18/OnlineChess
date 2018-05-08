using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class ChessModel{

        public const int DIMENSION = 8;

        private List<ChessPiece> board;
        private List<ChessPiece> selections;
        private String color;
        private String gameInfo;
        private String recentMove;
        private bool gameOver;
        private bool turn;

        public ChessModel(String color){
            this.board = new List<ChessPiece>();
            this.selections = new List<ChessPiece>();
            this.color = color;
            GenerateBoard();
            this.gameOver = false;
            if(color == "Black"){
                this.turn = false;
            }
            else{
                this.turn = true;
            }
        }

        /*
         * Returns a copy of the current game board
         */
        public List<ChessPiece> GetBoard(){
            List<ChessPiece> CopyBoard = new List<ChessPiece>();

            for(int i = 0; i < this.board.Count; i++){
                CopyBoard.Add(this.board.ElementAt(i));
            }

            return CopyBoard;
        }

        /*
         * Returns the color pieces the player controls
         */
        public String GetColor(){
            return this.color;
        }

        /*
         * Returns the information saved in GameInfo
         */
        public String GetGameInfo(){
            return this.gameInfo;
        }

        /*
         * Returns the most recent move performed
         */
        public String GetRecentMove(){
            return this.recentMove;
        }

        /*
         * Returns true if the game has ended,
         * false if it is still ongoing
         */
        public bool IsGameOver(){
            return this.gameOver;
        }

        /*
         * Returns true if it is the player's turn,
         * false otherwise if it is the opponent's turn
         */
        public bool IsTurn(){
            return this.turn;
        }

        /*
         * Changes the turn values to the opposite value
         */
        public void ChangeTurn(){
            this.turn = !this.turn;
        }

        /*
         * Ends the game
         */
        public void EndGame(){
            this.gameOver = true;
        }

        /*
         * Generates the game board for a new game
         */
        private void GenerateBoard(){
            if(this.color == "Black"){
                GeneratePieces("White");
                GeneratePawns("White");
                GenerateEmptyPieces();
                GeneratePawns("Black");
                GeneratePieces("Black");
            }
            else {
                GeneratePieces("Black");
                GeneratePawns("Black");
                GenerateEmptyPieces();
                GeneratePawns("White");
                GeneratePieces("White");
            }
        }

        /*
         * Helper function of GenerateBoard()
         * to generate the back row pieces
         */
        private void GeneratePieces(String color){
            this.board.Add(new Rook(color));
            this.board.Add(new Knight(color));
            this.board.Add(new Bishop(color));
            if(this.color == "Black"){
                this.board.Add(new King(color));
                this.board.Add(new Queen(color));
            }
            else {
                this.board.Add(new Queen(color));
                this.board.Add(new King(color));
            }
            this.board.Add(new Bishop(color));
            this.board.Add(new Knight(color));
            this.board.Add(new Rook(color));
        }

        /*
         * Helper function of GenerateBoard()
         * to generate the pawn rows
         */
        private void GeneratePawns(String color){
            for(int i = 0; i < 8; i++){
                this.board.Add(new Pawn(color));
            }
        }

        /*
         * Helper function of GenerateBoard()
         * to generate the empty spaces
         */
        private void GenerateEmptyPieces(){
            for(int i = 0; i < 32; i++){
                this.board.Add(new EmptyPiece());
            }
        }

        /*
         * Adds a piece to the selections list. If two pieces are selected,
         * it calls MovePieces() to make the move
         */
        public void SelectPiece(int n){
            if(selections.Count == 0){
                if(this.board.ElementAt(n).GetName() == "Empty"){
                    this.gameInfo = "You can't select an empty space!";
                }
                else if(this.board.ElementAt(n).GetColor() != this.color){
                    this.gameInfo = "You must select your own piece!";
                }
                else {
                    selections.Add(board.ElementAt(n));
                    this.gameInfo = "Choose where to move your piece";
                }
            }
            else if(selections.Count == 1){
                selections.Add(board.ElementAt(n));
                if(selections.Count == 2 && selections.ElementAt(0) == selections.ElementAt(1)){
                    selections.Clear();
                }
                else{
                    MovePieces(selections.ElementAt(0), selections.ElementAt(1), this.color);
                    selections.Clear();
                }
            }
        }

        /*
         * Moves the piece p1 to the location of p2, calling the validation
         * methods of p1 to verify that the move is legal. If the move is legal,
         * the move is made and the board is updated. If it is illegal, the
         * move will not be made and GameInfo will be updated
         */
        public void MovePieces(ChessPiece p1, ChessPiece p2, String color){
            List<ChessPiece> CopyBoard = this.CopyBoard();
            if(color != this.color){
                CopyBoard.Reverse();
            }
            int i1 = CopyBoard.IndexOf(p1);
            int i2 = CopyBoard.IndexOf(p2);
            int x1 = i1 % DIMENSION;
            int x2 = i2 % DIMENSION;
            int y1 = (int) Math.Floor((double) (i1 / DIMENSION));
            int y2 = (int) Math.Floor((double) (i2 / DIMENSION));
            int dx = x2 - x1;
            int dy = y2 - y1;

            if(p1.GetName() != "Empty" && p1.GetColor() == color){
                if(p1.IsValidMove(dx, dy, i1, i2, CopyBoard)){
                    CopyBoard.RemoveAt(i2);
                    CopyBoard.Insert(i2, p1);
                    CopyBoard.RemoveAt(i1);
                    CopyBoard.Insert(i1, new EmptyPiece());
                    if(color != this.color){
                        CopyBoard.Reverse();
                    }
                    this.board = CopyBoard;
                    if(p2.GetName() == "King"){
                        EndGame();

                        if(p2.GetColor() == this.color){
                            this.gameInfo = "You lost!";
                        }
                        else {
                            this.gameInfo = "You won!";
                        }
                    }
                    else {
                        this.gameInfo = p1.GetName() + " to (" + x2 + "," + y2 + ")";
                    }
                    this.recentMove = i1 + "," + i2;
                    this.turn = false;
                }
                else {
                    this.gameInfo = "Invalid move!";
                }
            }
            else {
                this.gameInfo = "Invalid color!";
            }
        }

        /*
         * Makes and returns a copy of the game board
         */
        private List<ChessPiece> CopyBoard(){
            List<ChessPiece> CopyBoard = new List<ChessPiece>();
            for(int i = 0; i < this.board.Count(); i++){
                CopyBoard.Add(this.board.ElementAt(i));
            }
            return CopyBoard;
        }

        /*
         * Turns the game board into a string of symbols
         */
        public String BoardToString(){
            String boardString = "";
            for(int i = 0; i < this.board.Count; i++){
                ChessPiece piece = this.board.ElementAt(i);
                if(piece.GetName() == "Bishop"){
                    if(piece.GetColor() == "Black"){
                        boardString += "B";
                    }
                    else {
                        boardString += "b";
                    }
                }
                else if(piece.GetName() == "King"){
                    if(piece.GetColor() == "Black"){
                        boardString += "K";
                    }
                    else {
                        boardString += "k";
                    }
                }
                else if(piece.GetName() == "Knight"){
                    if(piece.GetColor() == "Black"){
                        boardString += "N";
                    }
                    else {
                        boardString += "n";
                    }
                }
                else if(piece.GetName() == "Pawn"){
                    if(piece.GetColor() == "Black"){
                        boardString += "P";
                    }
                    else {
                        boardString += "p";
                    }
                }
                else if(piece.GetName() == "Queen"){
                    if(piece.GetColor() == "Black"){
                        boardString += "Q";
                    }
                    else {
                        boardString += "q";
                    }
                }
                else if(piece.GetName() == "Rook"){
                    if(piece.GetColor() == "Black"){
                        boardString += "R";
                    }
                    else {
                        boardString += "r";
                    }
                }
                else {
                    boardString += "-";
                }
            }
            return boardString;
        }
    }
}
