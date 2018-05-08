using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class Bishop : ChessPiece{

        public Bishop(String color) : base("Bishop", color){
            
        }

        public override bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(base.IsValidMove(dx, dy, i1, i2, board) == false){
                return false;
            }

            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            //Check if moving diagonally
            if(!(adx == ady)){
                return false;
            }

            if(CheckUpRight(dx, dy, i1, i2, board) == false){
                return false;
            }

            if(CheckDownRight(dx, dy, i1, i2, board) == false){
                return false;
            }

            if(CheckUpLeft(dx, dy, i1, i2, board) == false){
                return false;
            }

            if(CheckDownLeft(dx, dy, i1, i2, board) == false){
                return false;
            }

            return true;
        }
    }
}
