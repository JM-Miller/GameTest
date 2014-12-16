using Destruct.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Destruct.GameStates
{
    public class TestState : GameState
    {
        public string message;

        public override void Init()
        {
            Data.data = new List<string>();
            Utilities.Server.Init();
            Utilities.Client.Init("192.168.0.13", "TEST 123");
        }

        public override void Update()
        {
            Thread speaker = new Thread(Utilities.Client.Connect);
            speaker.Start();
            Thread speaker2 = new Thread(Utilities.Client.Connect);
            speaker2.Start();
            Thread listener = new Thread(Utilities.Server.ListenTcp);
            listener.Start();
            try
            {
                message = "";
                foreach (string s in Data.data)
                    message += s + "\n";
            }
            catch
            {

            }
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.DrawString(((IPEndPoint)Utilities.Server.tcpServer.Server.LocalEndPoint).Address.ToString(), new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 20), Brushes.brushSlateGray, new System.Drawing.PointF(20, 20));
            g.DrawString(message, new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 20), Brushes.brushSlateGray, new System.Drawing.PointF(20, 45));
        }
    }
}
