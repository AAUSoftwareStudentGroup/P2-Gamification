﻿using System;
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
			string queryString;
			if (Request.Form.Count > 0) {
				queryString = Request.Form.ToString ();
			} else {
				queryString = Request.QueryString.ToString ();
			}

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create ("http://webmat.cs.aau.dk/api?"+queryString);
			req.KeepAlive = false;
			req.ProtocolVersion = HttpVersion.Version10;
			req.Method = "GET";
			req.Timeout = 30000;

		    Stream dataStream;

		    try
		    {
		        dataStream = req.GetResponse().GetResponseStream();
		        StreamReader reader = new StreamReader(dataStream);
		        string responseString = reader.ReadToEnd();
		        Response.Write(responseString);
		    }
		    catch (Exception)
		    {
		        Response.Write("404");
		    }
		}
	}
}
