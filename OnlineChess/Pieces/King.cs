using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class King : ChessPiece{
        public King(String color) : base("King", color){

        }

        public override bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(base.IsValidMove(dx, dy, i1, i2, board) == false){
                return false;
            }

            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            //Check to make sure it isn't moving further than one space
            if(adx > 1 || ady > 1){
                return false;
            }

            if(CorrectDiagonal(adx, ady) == false){
                return false;
            }

            return true;
        }
    }
}
