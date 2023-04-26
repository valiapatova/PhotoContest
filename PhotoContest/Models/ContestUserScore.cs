using System;

namespace PhotoContest.Models
{
    public class ContestUserScore: IComparable
    {
        public double Score { get; }
        public User user { get; }

        public ContestUserScore(double score, User user)
        {
            this.Score = score;
            this.user = user;
        }

        public int CompareTo(object obj)
        {
            ContestUserScore other = (ContestUserScore) obj;

            if (Score < other.Score)
            {
                return 1;
            }
            else if (Score > other.Score)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
