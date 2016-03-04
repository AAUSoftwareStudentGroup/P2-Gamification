using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework
{
    public class ProgressbarStar : ProgressbarBase
    {
        public int GetProgress
        {
            get { return CurrentProgress; }
            set { CurrentProgress = value; }
        }

        public int GetMaxProgress
        {
            get { return MaxProgress; }
            set { MaxProgress = value; }
        }

        private List<int> _stars;

        public ProgressbarStar(int progress, int maxValue, params int[] stars)
        {
            this.CurrentProgress = progress;
            this.MaxProgress = maxValue;
            _stars = new List<int>(stars);
            GetStars();
        }

        public void Add(int star)
        {
            if (!_stars.Contains(star))
            {
                _stars.Add(star);
            }
        }

        public void Remove(int star)
        {
            _stars.Remove(star);
        }

        public int GetStars()
        {
            int starsCount = 0;
            int totalStars = 0;

            foreach (int i in _stars)
            {
                totalStars++;
                if (i <= CurrentProgress)
                {
                    starsCount++;
                }
            }
            // Returns amount of reached stars.
            return starsCount;
        }
    }
}
