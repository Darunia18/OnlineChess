using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Online Chess
// author: Brady Sklenar
// Networking Concepts and Admininstration
namespace OnlineChess {
    public abstract class ChessPiece{
        private String name;
        private String color;

        public ChessPiece(String name, String color){
            this.name = name;
            this.color = color;
        }

        public String GetName(){
            return this.name;
        }

        public String GetColor(){
            return this.color;
        }

        public virtual bool IsValidMove(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(board.ElementAt(i1).GetColor() == board.ElementAt(i2).GetColor()){
                return false;
            }
            return true;
        }


        public bool CheckUp(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dy < 0){
                for(int i = i1 - 8; i > i2; i -= 8){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckDown(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dy > 0){
                for(int i = i1 + 8; i < i2; i += 8){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckLeft(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dx < 0){
                for(int i = i1 - 1; i > i2; i--){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckRight(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dx > 0){
                for(int i = i1 + 1; i < i2; i++){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckUpRight(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dx > 0 && dy < 0){
                for(int i = i1 - 7; i > i2; i -= 7){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckDownRight(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dx > 0 && dy > 0){
                for(int i = i1 + 9; i < i2; i += 9){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckUpLeft(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dx < 0 && dy < 0){
                for(int i = i1 - 9; i > i2; i -= 9){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckDownLeft(int dx, int dy, int i1, int i2, List<ChessPiece> board){
            if(dx < 0 && dy > 0){
                for(int i = i1 + 7; i < i2; i += 7){
                    if(board.ElementAt(i).GetName() != "Empty"){
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CorrectDiagonal(int adx, int ady){
            if(adx > 0 && ady > 0){
                if(adx != ady){
                    return false;
                }
            }
            return true;
        }

    }
}
