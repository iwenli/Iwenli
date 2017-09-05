using Iwenli.Web.WebLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web.Module
{
    public class WebLogModule: IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(System.Web.HttpApplication application)
        {
            application.BeginRequest += application_BeginRequest;
            application.EndRequest += application_EndRequest;
        }

        WebLogInfo m_webLogInfo;

        void application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication _application = (HttpApplication)sender;
            HttpContext _context = _application.Context;
            m_webLogInfo = new WebLogInfo(_context);
            m_webLogInfo.InitInfo();
        }

        void application_EndRequest(object sender, EventArgs e)
        {
            if (m_webLogInfo != null)
            {
                System.Threading.Thread _t = new System.Threading.Thread(new System.Threading.ThreadStart(m_webLogInfo.Note));
                _t.Start();
            }
        }
    }
}
