using System.Net;
using System.IO;

/// "http://bot.whatismyipaddress.com/"
/// "http://checkip.dyndns.org"
/// "http://www.myip.is"

public IPAddress GetMyInternetIpAddress()
{
    WebRequest hwr = HttpWebRequest.Create(new Uri("http://checkip.dyndns.org"));
    WebResponse wr = hwr.GetResponse();
    Stream stream = wr.GetResponseStream();
    StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
    string htmlResult = streamReader.ReadToEnd();
    string[] htmlSplit = htmlResult.Split(new string[]{":", "<"}, StringSplitOptions.RemoveEmptyEntries);
    string IP = htmlSplit[6].Trim();
    stream.Close();
    wr.Close();
    return IPAddress.Parse(IP);
}