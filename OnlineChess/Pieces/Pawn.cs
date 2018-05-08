using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class Pawn : ChessPiece{

        private bool moved;

        public Pawn(String color) : base("Pawn", color){
            this.moved = false;
        }

        public override bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(base.IsValidMove(dx, dy, i1, i2, board) == false){
                return false;
            }

            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            
            //Check if it can move forward 2 spaces
            if(this.moved == false && dy == -2){
                if(board.ElementAt(i2 + 8).GetName() != "Empty"){
                    moved = true;
                    return false;
                }
                if(adx != 0){
                    moved = true;
                    return false;
                }
            }
            else{
                //Check to make sure it is only moving forward
                if(dy != -1){
                    moved = true;
                    return false;
                }
                //Check to make sure it is only moving diagonally if taking another piece
                if(board.ElementAt(i2).GetName() != "Empty"){
                    if(adx != 1){
                        moved = true;
                        return false;
                    }
                }
                //Check to make sure it is only moving forward if not
                else{
                    if(adx != 0){
                        moved = true;
                        return false;
                    }
                }
            }

            moved = true;
            return true;
        }
    }
}
