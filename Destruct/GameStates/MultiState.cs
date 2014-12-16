using Destruct.Entities;
using Destruct.Entities.TileMaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Destruct.GameStates
{
    public class MultiState : GameState
    {
        SolidBrush clearBrush;
        Color clearColor;
        Rectangle screen;
        List<Entities.Entity> entities;
        List<Entities.Items.Item> items;
        TileMap map;
        public Player p;
        public bool isServer;
        public string serverAddress;

        public MultiState(bool server)
        {
            this.isServer = server;
            serverAddress = "192.168.0.13";
        }

        public override void Init()
        {
            clearColor = Color.Green;
            clearBrush = new SolidBrush(clearColor);
            screen = new Rectangle(0, 0, Globals.screenSize, Globals.screenSize);
            entities = new List<Entities.Entity>();
            //map = new TileMap(TileCreator.GetTestTiles(), this);
            p = new Entities.Player(map);
            entities.Add(p);
            items = new List<Entities.Items.Item>();
            if (!isServer)
            {
                Globals.playerId = 2;
                Utilities.Client.Init(serverAddress, Globals.playerId + "," + p.x + "," + p.y);
                Utilities.Client.message = Globals.playerId + "," + p.x + "," + p.y;
                Thread speaker = new Thread(Utilities.Client.Connect);
                speaker.Start();
            }
            else
            {
                Data.data = new List<string>();
                Utilities.Server.Init();
                Thread listener = new Thread(Utilities.Server.ListenTcp);
                listener.Start();
                if (Data.data != null && Data.data.Count > 0)
                {
                    entities.Add(new OtherPlayer());
                }
            }
        }

        int count;

        public override void Update()
        {
            //map.Update(p.x, p.y, items);
            foreach (Entities.Entity e in entities)
                e.Update();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(p);
                if (items[i].remove)
                    items.Remove(items[i]);
            }
            if (!isServer)
            {
                Utilities.Client.message = Globals.playerId + "," + p.x + "," + p.y;
            }
            else
            {
                
                if (Data.data != null && Data.data.Count > 0 && count < 1)
                {
                    entities.Add(new OtherPlayer());
                    count++;
                }
            }
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(clearBrush, screen);
            //map.Draw(g);
            foreach (Entities.Entity e in entities)
                e.Draw(g);
            foreach (Entities.Items.Item item in items)
                item.Draw(g);
            //map.DrawRoof(g);
            if (p.activeGun != null)
                p.activeGun.DrawAmmoText(g);
            foreach (Entities.Items.Item item in items)
                item.DrawText(g);
        }
    }
}
