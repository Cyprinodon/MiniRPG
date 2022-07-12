using System;

namespace MiniRPG.Utilities
{
    class ProgressCounter
    {
        public bool IsDone { get => Value == Cap; }
        public int Value { get; protected set; }
        public int Cap { get; protected set; }

        private Exception Error = new Exception("Cap value of ProgressCounter must be a positive number.");
        
        public ProgressCounter(int max)
        {
            if (max < 1) throw Error;
            
            Cap = max;
            Value = 0;
        }

        public ProgressCounter(int max, int value)
        {
            if (max < 1 || value < 0) throw Error;
            if(value > max) throw new Exception("Value of ProgressCounter must not exceed it's Cap.");

            Cap = max;
            Value = value;
        }

        public void CountUp(int amount = 1)
        {
            if (amount < 1) throw Error;

            int value = Value + amount;
            if (value < Cap) Value = value;
            else Value = Cap;
        }

        public void CountDown(int amount = 1)
        {
            if(amount < 1) throw Error;

            int value = Value - amount;
            if (value > 0) Value = value;
            else Value = Cap;
        }

        public override string ToString()
        {
            return $"{Value}/{Cap}";
        }
    }
}
