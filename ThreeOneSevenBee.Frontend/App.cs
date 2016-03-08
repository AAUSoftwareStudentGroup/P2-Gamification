using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Model;

namespace ThreeOneSevenBee.Frontend
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var t = new Template();
            Console.WriteLine(t.ToString());
        }
    }
}