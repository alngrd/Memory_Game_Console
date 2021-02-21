using System;
using System.Collections.Generic;
using System.Text;

namespace Back
{
    internal class Board
    {
        private int m_OpenCards;
        private char[,] m_Board;
        private readonly int r_BoardSize;
        private readonly int r_NumberOfRows;
        private readonly int r_NumberOfColumns;

        internal Board(int i_NumberOfRows, int i_NumberOfColumns)
        {
            this.m_Board = new char[i_NumberOfRows, i_NumberOfColumns];
            this.m_OpenCards = 0;
            this.r_BoardSize = i_NumberOfRows * i_NumberOfColumns;
            this.r_NumberOfRows = i_NumberOfRows;
            this.r_NumberOfColumns = i_NumberOfColumns;
        }

        internal int NumberOfRows
        {
            get
            {
                return r_NumberOfRows;
            }

        }
        internal int NumberOfColumns
        {
            get
            {
                return r_NumberOfColumns;
            }

        }


        /**
        * Sets a random game board.
        **/
        internal void SetRandom()
        {
            List<char> letters = new List<char>();
            int amountOfLettersLeft = this.r_BoardSize;
            int randomIndexOfLetter = 0;
            Random randomIndexGenerator = new Random();

            for (int i = 0; i < this.r_BoardSize / 2; i++)
            {
                letters.Add((char)(i + 65));
                letters.Add((char)(i + 65));
            }

            for (int row = 0; row < this.m_Board.GetLength(0); row++)
            {
                for (int column = 0; column < this.m_Board.GetLength(1); column++)
                {
                    randomIndexOfLetter = randomIndexGenerator.Next(amountOfLettersLeft);
                    this.m_Board[row, column] = letters[randomIndexOfLetter];
                    letters.RemoveAt(randomIndexOfLetter);
                    amountOfLettersLeft--;
                }
            }
        }

        /**
         * Checks if the board is completely open
        **/
        internal bool IsFull()
        {
            return (this.m_OpenCards == this.r_BoardSize);
        }

        internal char GetCardValue(CardIndex i_CardLocation)
        {
            return this.m_Board[i_CardLocation.Row, i_CardLocation.Column];
        }


        /**
         * Checks if card is already open on board.
        **/
        internal bool IsLegalCardIndex(int i_Row, int i_Column)
        {
            return !((this.m_Board[i_Row, i_Column] >= 'A') && (this.m_Board[i_Row, i_Column] <= 'Z'));
        }

        internal void OpenCard(char i_Letter, CardIndex i_CardLocation)
        {
            this.m_Board[i_CardLocation.Row, i_CardLocation.Column] = i_Letter;
            m_OpenCards++;
        }

        internal void CloseCard(CardIndex i_CardLocation)
        {
            this.m_Board[i_CardLocation.Row, i_CardLocation.Column] = '\0';
            m_OpenCards--;
        }

        internal string BoardToString()
        {
            int numberOfRows = this.m_Board.GetLength(0);
            int numberOfColumns = this.m_Board.GetLength(1);
            const int numberOfEqualSignsPerIndex = 4;
            int seperatingRowLength = numberOfColumns * numberOfEqualSignsPerIndex;
            StringBuilder boardPrint = new StringBuilder();
            string seperatingRow = buildSeperatingRow(seperatingRowLength);

            boardPrint.Append(" ");
            for (int i = 1; i <= numberOfColumns; i++)
            {
                boardPrint.Append("   ");
                boardPrint.Append((char)('@' + i));
            }

            appendSeperatingRow(ref boardPrint, seperatingRow);
            for (int column = 0; column < numberOfColumns; column++)
            {
                boardPrint.Append(column + 1 + " |");
                for (int row = 0; row < numberOfRows; row++)
                {
                    boardPrint.Append(" " + this.m_Board[column, row] + " |");
                }

                appendSeperatingRow(ref boardPrint, seperatingRow);
            }

            return boardPrint.ToString();
        }

        /**
         * Builds the ='s seperating row according to given forman and board size
        **/
        private String buildSeperatingRow(int i_SeperatingRowLength)
        {
            StringBuilder seperatingRow = new StringBuilder();

            seperatingRow.Append("  ");
            seperatingRow.Append("==");
            for (int i = 0; i < i_SeperatingRowLength; i++)
            {
                seperatingRow.Append("=");
            }

            return seperatingRow.ToString();
        }

        private void appendSeperatingRow(ref StringBuilder io_BoardPrint, string i_SeperatingRow)
        {
            io_BoardPrint.Append("\n");
            io_BoardPrint.Append(i_SeperatingRow.ToString());
            io_BoardPrint.Append("\n");
        }
    }
}