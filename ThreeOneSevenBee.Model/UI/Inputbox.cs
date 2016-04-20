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
        }

        public override void KeyPressed(int key, Vector2 lastClick)
        {
            Console.WriteLine("Key: " + key);
        }

    }
}
