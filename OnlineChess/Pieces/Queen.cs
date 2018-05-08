using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class Queen : ChessPiece{

        public Queen(String color) : base("Queen", color){

        }

        public override bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(base.IsValidMove(dx, dy, i1, i2, board) == false){
                return false;
            }

            int adx = Math.Abs(dx);
            int ady = Math.Abs(dy);

            if(CorrectDiagonal(adx, ady) == false){
                return false;
            }

            if(CheckUp(dx, dy, i1, i2, board) == false){
                return false;
            }

            if(CheckDown(dx, dy, i1, i2, board) == false){
                return false;
            }

            if(CheckLeft(dx, dy, i1, i2, board) == false){
                return false;
            }

            if(CheckRight(dx, dy, i1, i2, board) == false){
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
