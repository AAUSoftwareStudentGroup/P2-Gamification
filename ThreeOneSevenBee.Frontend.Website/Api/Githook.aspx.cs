using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThreeOneSevenBee.Frontend.Website
{
    public partial class ApiGithook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
	    HttpWebRequest request = (HttpWebRequest)
        	WebRequest.Create("http://localhost:8001/");
            HttpWebResponse response = (HttpWebResponse)
        	request.GetResponse();
	}
    }
}
