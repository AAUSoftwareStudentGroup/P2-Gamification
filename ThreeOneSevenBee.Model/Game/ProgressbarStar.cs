using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Model.Game
{
    public class ProgressbarStar : IEnumerable<double>
    {
        public int StartValue;
        public int EndValue;
        public int CurrentValue;

        public double CalculatePercentage(int value)
        {
            return (double)(value - StartValue) / (EndValue - StartValue);
        }

        public double Percentage
        {
            get { return CalculatePercentage(CurrentValue); }
        }

        private List<int> stars;

        public ProgressbarStar(int startValue, int endValue, int currentValue, params int[] stars)
        {
            StartValue = startValue;
            EndValue = endValue;
            CurrentValue = currentValue;
            this.stars = new List<int>(stars);
        }

        public void Add(int star)
        {
            if (!stars.Contains(star))
            {
                stars.Add(star);
            }
        }

        public void Remove(int star)
        {
            if (stars.Contains(star))
            {
                stars.Remove(star);
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            return stars.Select((s) => CalculatePercentage(s)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
