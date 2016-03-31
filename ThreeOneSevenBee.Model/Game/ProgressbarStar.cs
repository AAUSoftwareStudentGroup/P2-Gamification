﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Model.Game
{
    public class ProgressbarStar
    {
        private int maxProgress;
        private int currentProgress;

        public int Progress
        {
            get { return currentProgress; }
            set { currentProgress = value; }
        }

        public double Percentage
        {
            get { return (double)currentProgress / maxProgress; }
        }

        public int MaxProgress
        {
            get { return maxProgress; }
            set { maxProgress = value; }
        }

        public List<int> Stars;

        public ProgressbarStar(int progress, int maxValue, params int[] stars)
        {
            this.currentProgress = progress;
            this.maxProgress = maxValue;
            Stars = new List<int>(stars);
            GetStars();
        }

        public void Add(int star)
        {
            if (!Stars.Contains(star))
            {
                Stars.Add(star);
            }
        }

        public void Remove(int star)
        {
            if (Stars.Contains(star))
            {
                Stars.Remove(star);
            }
        }

        public int GetStars()
        {
            int starsCount = 0;
            int totalStars = 0;

            foreach (int i in Stars)
            {
                totalStars++;
                if (i <= currentProgress)
                {
                    starsCount++;
                }
            }
            // Returns amount of reached stars.
            return starsCount;
        }
    }
}
