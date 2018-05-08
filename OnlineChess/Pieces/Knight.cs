using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class Knight : ChessPiece{

        public Knight(String color) : base("Knight", color){

        }

        public override bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(base.IsValidMove(dx, dy, i1, i2, board) == false){
                return false;
            }

            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            //Check to make sure it is moving correctly
            if(!((adx == 2 && ady == 1) || (adx == 1 && ady == 2))){
                return false;
            }

            return true;
        }
    }
}
