using System;
using System.Collections.Generic;

namespace Front
{
    class GameInterface
    {
        private const string k_ExitSign = "Q";
        private const int k_MinSizeOfRowsAndColumns = 4;
        private const int k_MaxSizeOfRowsAndColumns = 6;

        /**
         * Prints greetings and the rules of the game
        **/
        internal static void BeginGame()
        {
            Console.WriteLine("Hello there, we are going to play the Memory Game.");
            Console.WriteLine("The board consists of multiple identical pairs of cards, all facing down.");
            Console.WriteLine("Each turn you will need to choose two cards to flip open (among the down facing cards), one after the other.");
            Console.WriteLine("If the cards you've choosen are the same they will stay open, plus you get a point, plus you get another turn! Otherwise the cards will fill back after two seconds.");
            Console.WriteLine("The goal of the game is to have the most points when the board is completly open.");
            Console.WriteLine("Let's play!!");
        }

        /**
         * Gets players names and the decision if the game will be player vs computor or player vs player. 
        **/
        internal static String[] GetPlayersFromUsers()
        {
            String[] players = new string[2];

            players[0] = getName("first");
            if (isThereASecondPlayer())
            {
                Console.WriteLine("It's so nice to have a friend to play with.");
                players[1] = getName("second");
            }

            return players;
        }

        /**
         * Asks if the game will be player vs computor or player vs player.
        **/
        private static bool isThereASecondPlayer()
        {
            String friendOrComputer;

            Console.WriteLine("Do you want to play against a friend(enter \"0\") or against the computer(enter \"1\")?");
            friendOrComputer = Console.ReadLine();
            while (!friendOrComputer.Equals("0") && !friendOrComputer.Equals("1"))
            {
                Console.WriteLine("Oopsie doopsie, it seems that you have entered a wrong input, lets try again.");
                Console.WriteLine("Do you want to play against a friend(enter \"0\") or against the computer(enter \"1\")?");
                friendOrComputer = Console.ReadLine();
            }

            return friendOrComputer.Equals("0");
        }

        private static string getName(string i_PlayerPossition)
        {
            string playerName;

            Console.WriteLine(String.Format("Please enter the name of the {0} player.", i_PlayerPossition));
            playerName = Console.ReadLine();
            while (playerName.Equals(""))
            {
                Console.WriteLine("Don't be silly, everybody has a name.");
                Console.WriteLine(String.Format("Please enter the name of the {0} player.", i_PlayerPossition));
                playerName = Console.ReadLine();
            }

            return playerName;
        }

        internal static void GetBoardSizeFromUser(ref int io_NumberOfColumnsInBoard, ref int io_NumberOfRowsInBoard)
        {
            Console.WriteLine("Now we need to decide how big we want the board.");
            Console.WriteLine(String.Format("The rows and columns lengths can be between {0} and {1}, and their multipication has to be even.", k_MinSizeOfRowsAndColumns, k_MaxSizeOfRowsAndColumns));
            io_NumberOfRowsInBoard = getNumberOfRowsOrColumnsForBoardSize("rows");
            io_NumberOfColumnsInBoard = getNumberOfRowsOrColumnsForBoardSize("columns");
            while ((io_NumberOfRowsInBoard * io_NumberOfColumnsInBoard) % 2 != 0)
            {
                Console.WriteLine("Oh dang!! You've entered two numbers that their multipication is odd, let's try again.");
                io_NumberOfRowsInBoard = getNumberOfRowsOrColumnsForBoardSize("rows");
                io_NumberOfColumnsInBoard = getNumberOfRowsOrColumnsForBoardSize("columns");
            }
        }

        private static int getNumberOfRowsOrColumnsForBoardSize(string i_CurrentFeature)
        {
            int numberOfFeature = 0;
            bool succsessfulParse;

            Console.WriteLine(String.Format("Please enter the number of {0} you want the board to have.", i_CurrentFeature));
            succsessfulParse = int.TryParse(Console.ReadLine(), out numberOfFeature);
            while (!(succsessfulParse) || !(numberOfFeature >= k_MinSizeOfRowsAndColumns && numberOfFeature <= k_MaxSizeOfRowsAndColumns))
            {
                Console.WriteLine(String.Format("Oopsie doopsie, it seems like you've entered an invalid number of {0}, please try again.", i_CurrentFeature));
                int.TryParse(Console.ReadLine(), out numberOfFeature);
            }

            return numberOfFeature;
        }

        internal static void GetCardIndexFromPlayer(ref int io_IndexOfRow, ref int io_IndexOfColumn, int i_NumberOfRowsInBoard, int i_NumberOfColumnsInBoard)
        {
            string userInput;

            Console.WriteLine(String.Format("Please enter your next guess. The column should be between \"A\" and \"{0}\"." +
                " The row should be between \"1\" and \"{1}\".", (char)(i_NumberOfColumnsInBoard + 64), i_NumberOfRowsInBoard));
            userInput = Console.ReadLine();
            checkAndQuitGameIfInputIsExitSign(userInput);
            while (!(checkInputLengthIs2(userInput)) || !(checkColumnGuess(userInput[0], i_NumberOfColumnsInBoard)) || !(checkRowGuess(userInput[1], i_NumberOfRowsInBoard)))
            {
                Console.WriteLine(String.Format("Please enter your next guess. The column should be between \"A\" and \"{0}\"." +
                " The row should be between \"1\" and \"{1}\".", (char)(i_NumberOfColumnsInBoard + 64), i_NumberOfRowsInBoard));
                userInput = Console.ReadLine();
                checkAndQuitGameIfInputIsExitSign(userInput);
            }

            io_IndexOfColumn = userInput[0] - 65;
            io_IndexOfRow = userInput[1] - 49;
        }

        private static bool checkInputLengthIs2(string i_UserInput)
        {
            if (i_UserInput.Length != 2)
            {
                Console.WriteLine("You're guess is not in the right length.");
            }

            return i_UserInput.Length == 2;
        }

        /**
         * Checks if row guess is in the range of the board.
        **/
        private static bool checkRowGuess(char i_RowGuess, int i_NumberOfRowsInBoard)
        {
            if (!(i_RowGuess >= '1' && i_RowGuess <= (char)(i_NumberOfRowsInBoard + 48)))
            {
                Console.WriteLine("You're row guess is invalid. Please try again.");
            }

            return (i_RowGuess >= '1' && i_RowGuess <= (char)(i_NumberOfRowsInBoard + 48));
        }

        /**
         * Checks if column guess is in the range of the board.
        **/
        private static bool checkColumnGuess(char i_ColumnGuess, int i_NumberOfColumnInBoard)
        {
            if (!(i_ColumnGuess - 65 >= 0 && i_ColumnGuess - 65 < i_NumberOfColumnInBoard))
            {
                Console.WriteLine("You're column is invalid. Please try again.");
            }

            return (i_ColumnGuess - 65 >= 0 && i_ColumnGuess - 65 < i_NumberOfColumnInBoard);
        }

        private static void checkAndQuitGameIfInputIsExitSign(string i_UserInput)
        {
            if (i_UserInput.Equals(k_ExitSign))
            {
                Console.WriteLine("Thank you for playing, see you next time!");
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        internal static void PrintCardAlreadyOpen(int i_Row, int i_Column)
        {
            Console.WriteLine(String.Format("The card on row {0} and column {1} is already open. Please guess a down facing card.", i_Row + 1, (char)(i_Column + 65)));
        }

        internal static void PrintBoard(String i_Board)
        {
            clearConsole();
            Console.WriteLine(i_Board);
        }

        /**
         * Prints the current score and who's turn it is.
        **/
        internal static void WriteGameStatus(int i_Player1Score, int i_Player2Score, string i_CurrentPlayerName, string i_Player1Name, string i_Player2Name)
        {
            printScore(i_Player1Score, i_Player2Score, i_Player1Name, i_Player2Name, "current");
            Console.WriteLine(String.Format("{0}'s turn.", i_CurrentPlayerName));
        }

        private static void printScore(int i_Player1Score, int i_Player2Score, string i_Player1Name, string i_Player2Name, string i_StatusOfGame)
        {
            Console.WriteLine(String.Format("The {4} score is: {0} has {1} points and {2} has {3} points", i_Player1Name, i_Player1Score, i_Player2Name, i_Player2Score, i_StatusOfGame));
        }

        private static void clearConsole()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        internal static void FreezeBoard()
        {
            System.Threading.Thread.Sleep(2000);
        }

        /**
         * Prints the final score and winner of the game, and checks if player wants to play again.
         **/
        internal static bool EndGame(int i_Player1Score, int i_Player2Score, string i_Player1Name, string i_Player2Name)
        {
            if (i_Player1Score == i_Player2Score)
            {
                Console.WriteLine("OMG!!!!! it's a tie! Hooray we're all winners today");
            }
            else if (i_Player1Score > i_Player2Score)
            {
                printWinner(i_Player1Name);
            }
            else
            {
                printWinner(i_Player2Name);
            }

            printScore(i_Player1Score, i_Player2Score, i_Player1Name, i_Player2Name, "final");

            return askIfPlayAgain();
        }

        private static void printWinner(string i_PlayerName)
        {
            Console.WriteLine(String.Format("And the winner is {0}", i_PlayerName));
        }

        /**
         * Asks user if he wants to play another round.
         * Returns true if yes and false if no.
        **/
        private static bool askIfPlayAgain()
        {
            string playAgain;

            Console.WriteLine("Do you want to play agian?(y/n)");
            playAgain = Console.ReadLine();
            while (!playAgain.Equals("y") && !playAgain.Equals("n"))
            {
                Console.WriteLine("Invalid input, please try again.");
                Console.WriteLine("Do you want to play agian?(y/n)");
                playAgain = Console.ReadLine();
            }

            if (playAgain.Equals("n"))
            {
                Console.WriteLine("OK Bye, you magnificent player");
            }

            return playAgain.Equals("y");
        }
    }
}