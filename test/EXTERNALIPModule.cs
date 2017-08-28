/*
 * Copyright (c) Contributors, http://opensimwiki.de/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/*
//myip.php
<title><? php echo "My public IP Address"; ?></title>
<? php echo $_SERVER['REMOTE_ADDR']; ?>
*/

/*
;Opensim.ini
[EXTERNALIP]
	; Enable or disable ExternalIP (true/false) false
    enabled = false
	
	; Internet address where the IP is queried.
	ExternalIPAdress = "http://bot.whatismyipaddress.com/"
	
	; Refresh time in minutes
	ExternalIPRefreshTime = 30
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using OpenSim.Framework;
using OpenSim.Framework.Servers;
using OpenSim.Framework.Servers.HttpServer;
using OpenSim.Region.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using OpenSim.Server.Base;

using Mono.Addins;
using log4net;
using Nini.Config;

using OpenMetaverse;
using OpenSim.Framework.Client;
using OpenSim.Services.Interfaces;
using System.Threading.Tasks;
using System.Timers;


namespace OpenSim.EXTERNALIP
{
        
        public class ExternalIP()
        
        {
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://opensimwiki.de/myip.php");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(m_ExternalIPAdress);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader str = new StreamReader(res.GetResponseStream());
            string ip = str.ReadToEnd();
            str.Close();
            res.Close();
            return IPAddress.Parse(ip);
        }


        private void LoadConfiguration(IConfigSource config)
        {
            IConfig cnf = config.Configs[EXTERNALIP];
            if (cnf == null)
            {
                m_log.WarnFormat("[EXTERNALIP]: Missing EXTERNALIP configuration");
                return;
            }

            m_ExternalIPAdress = cnf.GetString("ExternalIPAdress", string.Empty);

            /// This is from BaseServiceConnector
            string authType = Util.GetConfigVarFromSections<string>(config, "AuthType", new string[] { "Network", "EXTERNALIP" }, "None");

            switch (authType)
            {
                case "BasicHttpAuthentication":
                    m_Auth = new BasicHttpAuthentication(config, "EXTERNALIP");
                    break;
            }
            ///
            m_log.DebugFormat("[EXTERNALIP.Connector]: EXTERNALIP server at {0} with auth {1}",
                m_ExternalIPAdress, (m_Auth == null ? "None" : m_Auth.GetType().ToString()));
        }


        private void ExternalIPTimer()
        {
            Timer timer = new Timer(ExternalIPRefreshTime);
            timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Start();
            ExternalIP(); //neu starten ExternalIP()
        }



}
