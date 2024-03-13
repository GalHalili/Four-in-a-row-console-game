namespace Ex02_01.GameLogic
{
    internal class PlayerInfo
    {
        private int m_CurrentPoints = 0;
        private readonly bool r_IsComputerPlayer = false;
        private readonly eCoinType r_PlayerCoin;

        public PlayerInfo()
        {
            r_PlayerCoin = eCoinType.P1;
        }

        public PlayerInfo(bool i_IsComputerPlayer, eCoinType i_Coin)
        {
            r_IsComputerPlayer = i_IsComputerPlayer;
            r_PlayerCoin = i_Coin;
        }

        public int CurrentPoints
        {
            get
            {
                return m_CurrentPoints;
            }
            set
            {
                m_CurrentPoints = value;
            }
        }

        public bool IsComputerPlayer
        {
            get
            {
                return r_IsComputerPlayer;
            }
        }

        public eCoinType PlayerCoin
        {
            get
            {
                return r_PlayerCoin;
            }
        }
    }
}