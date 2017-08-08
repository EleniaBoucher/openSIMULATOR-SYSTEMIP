using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

/// "http://bot.whatismyipaddress.com/"
/// "http://checkip.dyndns.org"
/// "http://www.myip.is"

static string[] GetOwnIP()
{
        WebRequest req = WebRequest.Create("http://www.myip.is");
        WebResponse res = req.GetResponse();
        Stream s = res.GetResponseStream();
        StreamReader sr = new StreamReader(s);
        string streamTxt = sr.ReadToEnd();
        res.Close();
        Regex rx = new Regex(@"(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\b"); 
        MatchCollection mc = rx.Matches(streamTxt);
        string[] retStr = new string[mc.Count];
        for(int i = 0; i < mc.Count; i++)
        {
                retStr[i] = mc[i].Value.ToString();
        }
        return retStr;
}