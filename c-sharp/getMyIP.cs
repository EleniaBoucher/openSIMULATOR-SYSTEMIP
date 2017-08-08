using System.IO;
using System.Net; 

/// "http://bot.whatismyipaddress.com/"
/// "http://checkip.dyndns.org"
/// "http://www.myip.is"

 IPAddress getMyIP()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://bot.whatismyipaddress.com/");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader str = new StreamReader(res.GetResponseStream());
            string ip = str.ReadToEnd();
            str.Close();
            res.Close();
            return IPAddress.Parse(ip);
        }