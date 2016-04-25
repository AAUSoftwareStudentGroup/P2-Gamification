using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class Inputbox : LabelView
    {
        private string emptyString;
        public Inputbox(string emptyString) : base(emptyString)
        {
            this.emptyString = emptyString;
            BackgroundColor = new Color(0, 255, 100);
        }

        public override void KeyPressed(int key)
        {
            if(Active)
                Console.WriteLine("Key: " + key);
        }
    }
}
