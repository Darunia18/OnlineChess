using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    class EmptyPiece : ChessPiece{

        public EmptyPiece() : base("Empty", "Empty"){

        }

        public override bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            return false;
        }
    }
}
