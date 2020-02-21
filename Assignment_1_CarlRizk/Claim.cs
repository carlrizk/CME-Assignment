using System;

namespace CarlRizk.Assignment_1
{
    public class Claim : IEquatable<Claim>
    {
        private static int CURRENT_ID = 0;

        public int ID { get; private set; }
        public DateTime IncurredDate { get; private set; }
        public string PolicyNumber { get; private set; }
        public float ClaimedAmount { get; private set; }

        public Claim(DateTime incurredDate, string policyNumber, float claimedAmout)
        {
            ID = CURRENT_ID++;

            IncurredDate = incurredDate;
            PolicyNumber = policyNumber;
            ClaimedAmount = claimedAmout;
        }

        public bool Equals(Claim other)
        {
            return ID == other.ID;
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
