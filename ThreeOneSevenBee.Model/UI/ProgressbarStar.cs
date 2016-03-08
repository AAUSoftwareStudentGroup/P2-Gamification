using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Model.UI
{
    public class ProgressbarStar
    {
        private int _maxProgress;
        private int _currentProgress;

        public int GetProgress
        {
            get { return _currentProgress; }
            set { _currentProgress = value; }
        }

        public int GetMaxProgress
        {
            get { return _maxProgress; }
            set { _maxProgress = value; }
        }

        private List<int> _stars;

        public ProgressbarStar(int progress, int maxValue, params int[] stars)
        {
            this._currentProgress = progress;
            this._maxProgress = maxValue;
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
            if (_stars.Contains(star))
            {
                _stars.Remove(star);
            }
        }

        public int GetStars()
        {
            int starsCount = 0;
            int totalStars = 0;

            foreach (int i in _stars)
            {
                totalStars++;
                if (i <= _currentProgress)
                {
                    starsCount++;
                }
            }
            // Returns amount of reached stars.
            return starsCount;
        }
    }
}
