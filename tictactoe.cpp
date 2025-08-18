#include <iostream>
#include <limits>
#include <algorithm> // để dùng std::max, std::min
using namespace std;

char board[3][3];
char current_player;
int moveCount;
int mode; // 1 = PvP, 2 = PvC
char ai = 'X';
char human = 'O';

void initBoard()
{
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            board[i][j] = ' ';
    current_player = 'X';
    moveCount = 0;
}

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

bool isMovesLeft()
{
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            if (board[i][j] == ' ')
                return true;
    return false;
}

int evaluate()
{
    if (checkWin(ai))
        return 10;
    if (checkWin(human))
        return -10;
    return 0;
}

int minimax(int depth, bool isMax)
{
    int score = evaluate();

    if (score == 10)
        return score - depth;
    if (score == -10)
        return score + depth;
    if (!isMovesLeft())
        return 0;

    if (isMax)
    {
        int best = numeric_limits<int>::min();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i][j] == ' ')
                {
                    board[i][j] = ai;
                    best = max(best, minimax(depth + 1, false));
                    board[i][j] = ' ';
                }
            }
        }
        return best;
    }
    else
    {
        int best = numeric_limits<int>::max();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i][j] == ' ')
                {
                    board[i][j] = human;
                    best = min(best, minimax(depth + 1, true));
                    board[i][j] = ' ';
                }
            }
        }
        return best;
    }
}

void playerMove()
{
    int row, col;
    while (true)
    {
        cout << "Player " << current_player << " enter row and column (1-3 1-3): ";
        cin >> row >> col;
        row--;
        col--;

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

void computerMove()
{
    int bestVal = numeric_limits<int>::min();
    int bestRow = -1, bestCol = -1;

    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (board[i][j] == ' ')
            {
                board[i][j] = ai; // FIX: dùng = chứ không phải ==
                int moveVal = minimax(0, false);
                board[i][j] = ' ';
                if (moveVal > bestVal)
                {
                    bestRow = i;
                    bestCol = j;
                    bestVal = moveVal;
                }
            }
        }
    }
    board[bestRow][bestCol] = ai;
    moveCount++;
}

void switchPlayer()
{
    current_player = (current_player == 'X') ? 'O' : 'X';
}

int main()
{
    cout << "Select mode: 1 = Player vs Player, 2 = Player vs Computer: ";
    cin >> mode;

    if (mode == 2)
    {
        cout << "Do you want to play as X or O? (X goes first): ";
        char choice;
        cin >> choice;
        choice = toupper(choice);
        if (choice == 'X')
        {
            human = 'X';
            ai = 'O';
        }
        else
        {
            human = 'O';
            ai = 'X';
        }
    }

    initBoard();

    while (true)
    {
        printBoard();

        if (mode == 1) // Player vs Player
        {
            playerMove();
        }
        else // Player vs Computer
        {
            if (current_player == human)
                playerMove();
            else
                computerMove();
        }

        if (checkWin(current_player))
        {
            printBoard();
            cout << "Player " << current_player << " wins!\n";
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
