namespace P2Backend
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Collections.Generic; 

    public partial class Default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            foreach (String key in Request.QueryString.AllKeys) {
                requestParams.Add(key, Request.QueryString[key]);
            }
            Api api = new Api(requestParams);

			Response.Write( api.Execute() );

            base.OnLoad(e);
        }
    }
}

