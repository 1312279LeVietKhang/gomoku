using Gomoku_1312279.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gomoku_1312279.ViewModels
{
    public class GomokuViewModel
    {
        Board chessBoard;
        int gameSize = 12;
        Cell currPlayer;

        public GomokuViewModel()
        {
            chessBoard = new Board(gameSize);
            currPlayer = Cell.Red;   //đặt active player1 là đỏ (chơi trước hoặc là người nếu là người chơi với máy) 
        }

        public void Paint_ChessBoard(Canvas board)
        {
            double x = board.ActualWidth / (double)gameSize;
            double y = board.ActualHeight / (double)gameSize;
            double setTop = 0;
            
            bool chessType = true;

            for (int i = 0; i < gameSize; i++)
            {
                double setLeft = 0;
                for (int j = 0; j < gameSize; j++)
                {
                    if (chessType)
                    {
                        Grid whiteRec = new Grid()
                        {
                            Height = y,
                            Width = x,
                            Background = Brushes.White,

                        };
                        Canvas.SetLeft(whiteRec, setLeft);
                        Canvas.SetTop(whiteRec, setTop);
                        board.Children.Add(whiteRec);
                        chessType = false;

                    }
                    else
                    {
                        Grid grayRec = new Grid()
                        {
                            Height = y,
                            Width = x,
                            Background = Brushes.Gray,
                        };
                        Canvas.SetTop(grayRec, setTop);
                        Canvas.SetLeft(grayRec, setLeft);
                        board.Children.Add(grayRec);
                        chessType = true;
                    }
                    setLeft += x;
                }
                setTop += y;
                if (i % 2 == 0)
                    chessType = false;
                else
                    chessType = true;
            }
        }


        public void Zoom_ChessBoard(Canvas board)    //resize thay đổi kích thước khi phóng to bàn cờ
        {
            if (board.Children.Count == 0)
                return;
            double x = board.ActualWidth / gameSize;
            double y = board.ActualHeight / gameSize;
            double setTop = 0, setLeft = 0;
            for (int i = 0; i < board.Children.Count; i++)
            {
                if (i % gameSize == 0)
                {
                    if (i > 0)
                    {
                        setTop += y;
                        setLeft = 0;
                    }
                }
                else
                    setLeft += x;
                Grid temp = (Grid)board.Children[i];
                temp.Height = y;
                temp.Width = x;
                Canvas.SetTop(temp, setTop);
                Canvas.SetLeft(temp, setLeft);
                board.Children[i] = temp;
            }
        }   

        //pos là tọa độ tương ứng trên ô ờ
        //board dungf để cập nhật bàn cờ (UI) nếu nước đi hợp lệ
        //xong
        public bool PlayAt(Canvas board, Point pos)   //Hàm dùng để đánh cờ dựa
        {
            double row = pos.X;
            double col = pos.Y;
           
            int chessManPos = (int)row + (int)col * gameSize;
            if (chessManPos > gameSize * gameSize || chessManPos < 0) return false;
            

            Grid temp = (Grid)board.Children[chessManPos];
            Ellipse chessMan = new Ellipse();
            if (currPlayer == Cell.Black)
                chessMan.Fill = Brushes.Black;
            else
                chessMan.Fill = Brushes.Red;
            if (temp.Children.Count != 0) return false;

            if (chessBoard.GetChessman((int)row, (int)col) == Cell.None)
            {
                temp.Children.Add(chessMan);
                board.Children[chessManPos] = temp;

                chessBoard.SetChessman(currPlayer, (int)row, (int)col);

                if (chessBoard.CountPlayerItem((int)row, (int)col, 1, 0, currPlayer) >= 5
                    || chessBoard.CountPlayerItem((int)row, (int)col, 0, 1, currPlayer) >= 5
                    || chessBoard.CountPlayerItem((int)row, (int)col, 1, 1, currPlayer) >= 5
                    || chessBoard.CountPlayerItem((int)row, (int)col, 1, -1, currPlayer) >= 5)
                {
                    MessageBox.Show("         " + currPlayer.ToString() + " Is Win");
                    MessageBox.Show("Click 'New' to continue");
                }
                //chuyển lượt chơi cho người kia
                if (currPlayer == Cell.Black)
                    currPlayer = Cell.Red;
                else currPlayer = Cell.Black;
                return true;
            }
            
            return false;
            
        }

        public bool PlayMachine(Canvas board, Point pos)   //Hàm người đánh với máy: Người đánh trước (màu đỏ)
        {
            double row = pos.X;
            double col = pos.Y;

            int chessManPos = (int)row + (int)col * gameSize;
            if (chessManPos > gameSize * gameSize || chessManPos < 0) return false;


            Grid temp = (Grid)board.Children[chessManPos];
            Ellipse chessMan = new Ellipse();
            if (currPlayer == Cell.Black)
                chessMan.Fill = Brushes.Black;
            else
                chessMan.Fill = Brushes.Red;
            if (temp.Children.Count != 0) return false;

            if (chessBoard.GetChessman((int)row, (int)col) == Cell.None)
            {
                temp.Children.Add(chessMan);
                board.Children[chessManPos] = temp;

                chessBoard.SetChessman(currPlayer, (int)row, (int)col);

                if (chessBoard.CountPlayerItem((int)row, (int)col, 1, 0, currPlayer) >= 5
                    || chessBoard.CountPlayerItem((int)row, (int)col, 0, 1, currPlayer) >= 5
                    || chessBoard.CountPlayerItem((int)row, (int)col, 1, 1, currPlayer) >= 5
                    || chessBoard.CountPlayerItem((int)row, (int)col, 1, -1, currPlayer) >= 5)
                {
                    if (currPlayer == Cell.Red) {
                        MessageBox.Show(" Player Is Win");
                    }
                    else
                        MessageBox.Show("MaChine Is Win");
                    MessageBox.Show("Click 'New' to continue");
                }
                //chuyển lượt chơi cho người kia
                if (currPlayer == Cell.Black)
                    currPlayer = Cell.Red;
                else currPlayer = Cell.Black;
                return true;
            }

            return false;

        }
    }
}
