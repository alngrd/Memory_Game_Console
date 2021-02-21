namespace Back
{
    internal class CardIndex
    {
        private int m_Row;
        private int m_Column;

        internal CardIndex(int i_Row, int i_Column)
        {
            this.m_Row = i_Row;
            this.m_Column = i_Column;
        }

        internal int Row
        {
            get
            {
                return this.m_Row;
            }
            set
            {
                this.m_Row = value;
            }
        }

        internal int Column
        {
            get
            {
                return this.m_Column;
            }
            set
            {
                this.m_Column = value;
            }
        }

        internal bool Equals(CardIndex i_CompareTo)
        {
            return (this.m_Row == i_CompareTo.Row && this.m_Column == i_CompareTo.Column);
        }
    }
}