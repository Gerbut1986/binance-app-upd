namespace BinanceOptionsApp.Helpers
{
    public class Ring
    {
        int count;
        int pos;
        bool full;
        int bufferSize;
        public decimal[] buffer;
        public Ring(int size)
        {
            buffer = new decimal[size];
            full=false;
            pos=0;
            bufferSize=size;
            count=0;
        }
        public int Size()
        {
            return bufferSize;
        }
        public void Push(decimal value)
        {
            buffer[pos++] = value;
            if (count < bufferSize) count++;
            if (pos >= bufferSize)
            {
                pos = 0;
                full = true;
            }
        }
        public int Count()
        {
            return count;
        }
        public bool IsFull()
        {
            return full;
        }
        public int HeadIndex(int _index)
        {
            _index = (pos + bufferSize - 1 - _index) % bufferSize;
            return _index;
        }
        public decimal Head(int _index)
        {
            _index = (pos + bufferSize - 1 - _index) % bufferSize;
            return buffer[_index];
        }
        public decimal Tail(int _index)
        {
            _index = (pos + _index) % bufferSize;
            return buffer[_index];
        }
        public void SetHead(int _index, decimal value)
        {
            _index = (pos + bufferSize - 1 - _index) % bufferSize;
            buffer[_index] = value;
        }
        public void SetTail(int _index, decimal value)
        {
            _index = (pos + _index) % bufferSize;
            buffer[_index] = value;
        }
    }
}