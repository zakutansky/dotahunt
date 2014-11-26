using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

public class Fingerprint
{
    public static string CdnUrl(string rootRelativePath)
    {
        #if (!DEBUG)
            int index = rootRelativePath.LastIndexOf('/');
            rootRelativePath = rootRelativePath.Substring(index + 1);

            string cdnUrl = Mushi.Properties.Settings.Default.CdnUrl;
            string relativePath = rootRelativePath + "?v=" + Mushi.Properties.Settings.Default.Version;
            return cdnUrl + relativePath;
        #else
            return System.Web.VirtualPathUtility.ToAbsolute(rootRelativePath);
        #endif
    }
}