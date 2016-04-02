using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Model.Game
{
    public class ProgressbarStar : IEnumerable<int>
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

        public IEnumerable<double> ActivatedStarPercentages()
        {
            return stars.Where((s) => s >= CurrentValue).Select((s) => CalculatePercentage(s));
        }

        public IEnumerable<double> DeactivatedStarPercentages()
        {
            return stars.Where((s) => s < CurrentValue).Select((s) => CalculatePercentage(s));
        }

        public IEnumerator<int> GetEnumerator()
        {
            return stars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
