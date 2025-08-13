#include <iostream>
using namespace std;

// Khởi tạo
char board[3][3];
char current_player;
int moveCount;

// Khởi tạo bảng
void initBoard()
{
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            board[i][j] = ' ';
    current_player = 'X';
    moveCount = 0;
}

// Hiển thị bảng
void printBoard()
{
    cout << "\n";
    for (int i = 0; i < 3; i++)
    {
        cout << " " << board[i][0] << " | " << board[i][1] << " | " << board[i][2] << "\n";
        if (i < 2)
            cout << "---+---+---\n";
    }
    cout << "\n";
}

// Kiểm tra thắng
bool checkWin(char player)
{
    for (int i = 0; i < 3; i++)
        if (board[i][0] == player && board[i][1] == player && board[i][2] == player)
            return true;

    for (int j = 0; j < 3; j++)
        if (board[0][j] == player && board[1][j] == player && board[2][j] == player)
            return true;

    if (board[0][0] == player && board[1][1] == player && board[2][2] == player)
        return true;
    if (board[0][2] == player && board[1][1] == player && board[2][0] == player)
        return true;

    return false;
}

// Lượt chơi
void playerMove()
{
    int row, col;
    while (true)
    {
        cout << "Player " << current_player << " enter row and column (1-3 1-3): ";
        cin >> row >> col;
        row--; col--;

        if (row >= 0 && row < 3 && col >= 0 && col < 3)
        {
            if (board[row][col] == ' ')
            {
                board[row][col] = current_player;
                moveCount++;
                break;
            }
            else
            {
                cout << "Cell is already taken! Try again.\n";
            }
        }
        else
        {
            cout << "Invalid position! Try again.\n";
        }
    }
}

// Đổi lượt
void switchPlayer()
{
    current_player = (current_player == 'X') ? 'O' : 'X';
}

int main()
{
    initBoard();
    while (true)
    {
        printBoard();
        playerMove();

        if (checkWin(current_player))
        {
            printBoard();
            cout << "Player " << current_player << " is the winner!\n";
            break;
        }

        if (moveCount == 9)
        {
            printBoard();
            cout << "Draw!\n";
            break;
        }

        switchPlayer();
    }
    return 0;
}
