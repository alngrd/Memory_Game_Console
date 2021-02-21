using System.Collections.Generic;
using Mediator;

namespace Back
{
    internal class Game
    {
        private readonly Player[] r_Players;
        private Board m_TrueBoard;
        private readonly Board r_CurrentBoard;
        private readonly ComputerLogic r_Computer;
        private const string k_ComputerName = "Computer";
        private const bool v_IsHuman = true;
        private const int k_NumOfPlayers = 2;

        /**
        * Constructor for a player vs. computer game
        **/
        internal Game(int i_BoardNumberOfRows, int i_BoardNumberOfColumns, string i_FirstPlayerName)
        {
            r_Players = new Player[k_NumOfPlayers];
            r_Players[0] = new Player(i_FirstPlayerName, v_IsHuman);
            r_Players[1] = new Player(k_ComputerName, !v_IsHuman);
            m_TrueBoard = new Board(i_BoardNumberOfRows, i_BoardNumberOfColumns);
            r_CurrentBoard = new Board(i_BoardNumberOfRows, i_BoardNumberOfColumns);
            r_Computer = new ComputerLogic();

            m_TrueBoard.SetRandom();
        }

        /**
        * Constructor for a player vs. player game
        **/
        internal Game(int i_BoardNumberOfRows, int i_BoardNumberOfColumns, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            r_Players = new Player[k_NumOfPlayers];
            r_Players[0] = new Player(i_FirstPlayerName, v_IsHuman);
            r_Players[1] = new Player(i_SecondPlayerName, v_IsHuman);
            m_TrueBoard = new Board(i_BoardNumberOfRows, i_BoardNumberOfColumns);
            r_CurrentBoard = new Board(i_BoardNumberOfRows, i_BoardNumberOfColumns);

            m_TrueBoard.SetRandom();
        }

        internal ComputerLogic Computer
        {
            get
            {
                return this.r_Computer;
            }
        }

        internal Board CurrentBoard
        {
            get
            {
                return this.r_CurrentBoard;
            }
        }

        internal Player[] Players
        {
            get
            {
                return this.r_Players;
            }
        }

        internal char FlipUpCard(CardIndex i_CardLocation)
        {
            char cardTrueValue = this.m_TrueBoard.GetCardValue(i_CardLocation);

            this.r_CurrentBoard.OpenCard(cardTrueValue, i_CardLocation);

            return cardTrueValue;
        }
        internal void FlipDownCard(CardIndex i_CardLocation)
        {
            this.r_CurrentBoard.CloseCard(i_CardLocation);
        }
    }
}