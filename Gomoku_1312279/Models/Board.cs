using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_1312279.Models
{
    public enum Cell{
        None,
        Black,
        Red
    };

    public class Board
    {
        Cell[,] board;
        int gameSize; //kích cỡ game/bàn cờ
        private int v;

        public Board(int size)
        {
            gameSize = size;
            Create_Board();
        }

        public void SetChessman(Cell color, int x, int y)
        {
            if (IsInBoard(x, y))
                board[x, y] = color;           
        }

        public Cell GetChessman(int x, int y)
        {
            if (IsInBoard(x, y))
                return board[x, y];
            else
                throw new ArgumentException();
        }



        public bool IsInBoard(int row, int col)
        {
            return row >= 0 && row < gameSize && col >= 0 && col <gameSize;
        }

        //đếm số ô cùng màu
        //dùng sau mỗi lượt đánh
        public int CountPlayerItem(int row, int col, int drow, int dcol, Cell curPlayer)
        {
            if (curPlayer == Cell.None)
                return 0;
            int crow = row + drow;
            int ccol = col + dcol;
            int count = 1;
            while (IsInBoard(crow, ccol) && board[crow, ccol] == curPlayer)
            {
                count++;
                crow = crow + drow;
                ccol = ccol + dcol;
            }
            crow = row - drow;
            ccol = col - dcol;
            while (IsInBoard(crow, ccol) && board[crow, ccol] == curPlayer)
            {
                count++;
                crow = crow - drow;
                ccol = ccol - dcol;
            }
            return count;
        }

        //tạo ma trận mới
        //ma trận dùng cho việc tính toán
        private void Create_Board()
        {
            board = new Cell[gameSize, gameSize];
        }
    }

}
