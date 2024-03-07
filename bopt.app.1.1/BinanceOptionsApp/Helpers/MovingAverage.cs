namespace BinanceOptionsApp.Helpers
{
    public class MovingAverage
    {
        decimal[] buffer;
        int pos;
        bool filled;
        decimal sum;

        public MovingAverage(int period)
        {
            if (period < 1) period = 1;
            buffer = new decimal[period];
        }
        public decimal Process(decimal value)
        {
            sum += value - buffer[pos];
            buffer[pos++] = value;
            if (pos == buffer.Length)
            {
                pos = 0;
                filled = true;
            }

            if (filled)
            {
                Output = sum / buffer.Length;
            }
            else
            {
                Output = sum / pos;
            }
            return Output;
        }
        public decimal Output;
    }

    public class DoubleMovingAverage
    {
        private double[] buffer;
        private int pos;
        private bool filled;
        private double sum;
        public double Output;

        public DoubleMovingAverage(int period)
        {
            if (period < 1)
                period = 1;
            this.buffer = new double[period];
        }

        public double Process(double value)
        {
            this.sum += value - this.buffer[this.pos];
            this.buffer[this.pos++] = value;
            if (this.pos == this.buffer.Length)
            {
                this.pos = 0;
                this.filled = true;
            }
            this.Output = !this.filled ? this.sum / (double)this.pos : this.sum / (double)this.buffer.Length;
            return this.Output;
        }
    }
}
