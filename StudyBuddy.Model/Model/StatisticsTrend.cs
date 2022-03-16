namespace StudyBuddy.Model
{
    public class StatisticsTrend
    {
        private Trend Trend { get; }

        public string Glyph
        { 
            get { return GetTrendGlyph(); } 
        }
        
        public string Color
        {
            get { return GetTrendColor(); }
        }

        public StatisticsTrend()
        {

        }

        public StatisticsTrend(Trend trend)
        {
            this.Trend = Trend;
        }

        public StatisticsTrend(int lastPeriodCount, int thisPeriodCount)
        {
            this.Trend = CalculateTrendFromPeriodDifference(lastPeriodCount,thisPeriodCount);
        }

        private Trend CalculateTrendFromPeriodDifference(int lastPeriodCount, int thisPeriodCount)
        {
            if (lastPeriodCount < thisPeriodCount)
            {
                return Trend.Up;
            }
            else if (lastPeriodCount > thisPeriodCount)
            {
                return Trend.Down;
            }
            else
            {
                return Trend.Equal;
            }
        }

        private string _upTrendGlyph = "\uf106"; 
        private string _downTrendGlyph = "\uf107";
        private string _equalTrendGlyph = "\uf52c";
        private string _defaultTrendGlyph = "\uf12a";

        private string GetTrendGlyph()
        {
            switch (Trend)
            {
                case Trend.Up:
                    return _upTrendGlyph;
                case Trend.Down:
                    return _downTrendGlyph;
                case Trend.Equal:
                    return _equalTrendGlyph;
                default:
                    return _defaultTrendGlyph;
            }
        }

        private string _upTrendColor = "#008000"; // green
        private string _downTrendColor = "#FF0000"; // red
        private string _equalTrendColor = "#000000"; // black
        private string _defaultTrendColor = "#000000"; // black

        private string GetTrendColor() 
        {
            switch (Trend)
            {
                case Trend.Up:
                    return _upTrendColor;
                case Trend.Down:
                    return _downTrendColor;
                case Trend.Equal:
                    return _equalTrendColor;
                default:
                    return _defaultTrendColor;
            }
        }
    }

    public enum Trend
    {
        Up,
        Equal,
        Down
    }
}