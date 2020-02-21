namespace CarlRizk.Assignment_1
{
    public class ClaimsStatistics
    {
        public int Count { get; private set; }
        public float Total { get; private set; }
        public float Min { get; private set; }
        public float Max { get; private set; }

        public ClaimsStatistics(int count, float total, float min, float max)
        {
            Count = count;
            Total = total;
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"Count: {Count}\n" +
                $"Total: {Total}\n" +
                $"Min: {Min}\n" +
                $"Max: {Max}";
        }
    }
}
