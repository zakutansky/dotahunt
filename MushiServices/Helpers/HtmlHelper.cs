using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MushiServices.Helpers
{
    public class HtmlHelper
    {

        /// <summary>
        /// Downloas the document.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public static HtmlDocument DownloaDocument(string url, WebClient client)
        {
            HtmlWeb hwWeb = new HtmlWeb();
            //var page = client.DownloadString(url);
            var doc = hwWeb.Load(url);
            //doc.Encoding. = Encoding.UTF8;
            return doc;
        }
    }
}
