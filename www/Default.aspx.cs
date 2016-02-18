
namespace www
{
    using System;
    using System.Web;
    using System.Web.UI;

    public partial class Default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            var service = new GameWebService();

            Response.Write("Count: " + service.GetCount());
            service.AddCount();

            base.OnLoad(e);
        }
    }
}

