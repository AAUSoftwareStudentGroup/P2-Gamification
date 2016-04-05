using System;
using System.Web;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Text;

namespace threeonesevenbee.Frontend.Website
{
	
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString.Count == 0) {
				Response.Write ("no action");
				return;
			}
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create ("http://webmat.srv.aau.dk/api?"+Request.QueryString);
			req.KeepAlive = false;
			req.ProtocolVersion = HttpVersion.Version10;
			req.Method = "GET";
			req.Timeout = 3000;

			Stream dataStream = req.GetResponse ().GetResponseStream ();
			StreamReader reader = new StreamReader (dataStream);
			string responseString = reader.ReadToEnd ();
			Response.Write (responseString);
		}
	}
}