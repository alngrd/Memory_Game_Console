using System;
using Back;
using Front;
namespace Mediator
{
    internal class GameDriver
    {
        private static Player s_CurrentPlayer;
        private static Player s_NextPlayer;
        private static Game s_ThisMemoryGame;
        private static bool s_GameIncludesComputerPlayer;

        /**
         * Gets board size, player names and player type from inteface class.
         * initializes a new game with those details
         **/
        internal static bool InitializeGame()
        {
            int numberOfRowsInBoard = 0;
            int numberOfColumnsInBoard = 0;
            string[] players;

            GameInterface.BeginGame();
            players = GameInterface.GetPlayersFromUsers();
            GameInterface.GetBoardSizeFromUser(ref numberOfColumnsInBoard, ref numberOfRowsInBoard);
            if (players[1] == null)
            {
                s_ThisMemoryGame = new Game(numberOfRowsInBoard, numberOfColumnsInBoard, players[0]);
                s_GameIncludesComputerPlayer = true;
            }
            else
            {
                s_ThisMemoryGame = new Game(numberOfRowsInBoard, numberOfColumnsInBoard, players[0], players[1]);
            }

            return runGame();
        }

        /**
        * Checks who current player is.
        * while the board isn't full yet- runs a turn for the current player.
        * after the turn- reports game status to interface.
        **/
        private static bool runGame()
        {
            s_CurrentPlayer = s_ThisMemoryGame.Players[0];
            s_NextPlayer = s_ThisMemoryGame.Players[1];
            GameInterface.PrintBoard(s_ThisMemoryGame.CurrentBoard.BoardToString());
            while (!s_ThisMemoryGame.CurrentBoard.IsFull())
            {
                GameInterface.WriteGameStatus(s_ThisMemoryGame.Players[0].Score, s_ThisMemoryGame.Players[1].Score, s_CurrentPlayer.Name, s_ThisMemoryGame.Players[0].Name, s_ThisMemoryGame.Players[1].Name);
                playTurn(ref s_CurrentPlayer);
            }

            return GameInterface.EndGame(s_ThisMemoryGame.Players[0].Score, s_ThisMemoryGame.Players[1].Score, s_ThisMemoryGame.Players[0].Name, s_ThisMemoryGame.Players[1].Name);
        }

        /**
        * Gets card choises from the player.
        * Flips the chosen cards in the board and presents the current board to players.
        * Checks if a match was found and updates score.
        * Updates computer memory if needed. (if there is a computer player)
        **/
        private static void playTurn(ref Player io_CurrentPlayer)
        {
            CardIndex firstChosenCard = chooseFirstCard(ref io_CurrentPlayer);
            CardIndex secondChosenCard;
            char onFirstCard = s_ThisMemoryGame.FlipUpCard(firstChosenCard);
            char onSecondCard;

            GameInterface.PrintBoard(s_ThisMemoryGame.CurrentBoard.BoardToString());
            syncDataToCompterLogic(ref firstChosenCard, onFirstCard);
            if (!io_CurrentPlayer.IsHuman)
            {
                s_ThisMemoryGame.Computer.UpdateFirstCardValue(onFirstCard);
                GameInterface.FreezeBoard();
            }

            secondChosenCard = chooseSecondCard(ref io_CurrentPlayer);
            onSecondCard = s_ThisMemoryGame.FlipUpCard(secondChosenCard);
            GameInterface.PrintBoard(s_ThisMemoryGame.CurrentBoard.BoardToString());
            GameInterface.FreezeBoard();
            if (onFirstCard == onSecondCard)
            {
                io_CurrentPlayer.Score++;
                GameInterface.PrintBoard(s_ThisMemoryGame.CurrentBoard.BoardToString());
            }
            else
            {
                syncDataToCompterLogic(ref secondChosenCard, onSecondCard);
                s_ThisMemoryGame.FlipDownCard(firstChosenCard);
                s_ThisMemoryGame.FlipDownCard(secondChosenCard);
                GameInterface.PrintBoard(s_ThisMemoryGame.CurrentBoard.BoardToString());
                swapCurrentPlayer();
            }
        }

        /**
         * If needed, activates the update of the computer player's memory.
        **/
        private static void syncDataToCompterLogic(ref CardIndex io_ChosenCard, char i_OnChosenCard)
        {
            if (s_GameIncludesComputerPlayer)
            {
                s_ThisMemoryGame.Computer.SyncDataToCompterLogic(io_ChosenCard, i_OnChosenCard);
            }
        }

        private static void swapCurrentPlayer()
        {
            Player tempPlayer = s_NextPlayer;

            s_NextPlayer = s_CurrentPlayer;
            s_CurrentPlayer = tempPlayer;
        }

        /**
         * Checks player type and gets first card choice from the current player
         **/
        private static CardIndex chooseFirstCard(ref Player io_CurrentPlayer)
        {
            CardIndex chosenCard;

            if (io_CurrentPlayer.IsHuman)
            {
                int row = 0;
                int column = 0;

                GameInterface.GetCardIndexFromPlayer(ref row, ref column, s_ThisMemoryGame.CurrentBoard.NumberOfRows, s_ThisMemoryGame.CurrentBoard.NumberOfColumns);
                chosenCard = new CardIndex(row, column);
                while (!s_ThisMemoryGame.CurrentBoard.IsLegalCardIndex(row, column))
                {
                    GameInterface.PrintCardAlreadyOpen(chosenCard.Row, chosenCard.Column);
                    GameInterface.GetCardIndexFromPlayer(ref row, ref column, s_ThisMemoryGame.CurrentBoard.NumberOfRows, s_ThisMemoryGame.CurrentBoard.NumberOfColumns);
                    chosenCard.Column = column;
                    chosenCard.Row = row;
                }
            }
            else
            {
                chosenCard = s_ThisMemoryGame.Computer.ChooseFirstCard(s_ThisMemoryGame.CurrentBoard);
            }

            return chosenCard;
        }

        /**
        * Checks player type and gets second card choice from the current player
        **/
        private static CardIndex chooseSecondCard(ref Player io_CurrentPlayer)
        {
            CardIndex chosenCard;

            if (io_CurrentPlayer.IsHuman)
            {
                chosenCard = chooseFirstCard(ref io_CurrentPlayer);
            }
            else
            {
                chosenCard = s_ThisMemoryGame.Computer.ChooseSecondCard(s_ThisMemoryGame.CurrentBoard);
            }

            return chosenCard;
        }
    }
}