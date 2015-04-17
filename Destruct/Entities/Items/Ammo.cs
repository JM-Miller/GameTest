using Destruct.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Items
{
    public class Ammo : Item
    {
        //Player to calculate for
        Player player;

        /// <summary>
        /// Sets all needed variables for Ammo object
        /// </summary>
        /// <param name="tileX">The X offset of the container tile</param>
        /// <param name="tileY">The Y offset of the container tile</param>
        /// <param name="containerLayer">The container tile's layer</param>
        /// <param name="player">The player to calculate for</param>
        public Ammo(int tileX, int tileY, ref TileMaps.TileLayer containerLayer, ref Player player)
        {
            this.tileX = tileX;
            this.tileY = tileY;
            this.size = 2 * Globals.scale;
            this.size = 2 * Globals.scale;
            this.layerTileX = containerLayer.xMapOffset;
            this.layerTileY = containerLayer.yMapOffset;
            this.player = player;
        }

        public override void Init()
        {
            //TODO: Reevaluate the need for Init step
        }

        public override void Update()
        {
            if (ShouldRemove) //If the item should be removed, do not calculate for it
                return;
            //Calculate X and Y positions for object, relative to the screen
            screenX = player.map.xOffset + (tileX * (Globals.defaultTileSize * Globals.scale)) + (layerTileX * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            screenY = player.map.yOffset + (tileY * (Globals.defaultTileSize * Globals.scale)) + (layerTileY * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);

            if (player.guns.Count > 0 && Globals.screenRectangle.IntersectsWith(screenRectangle) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Space))
            {
                ShouldRemove = true; //Remove the item
                PlayerTools.Gun gun = player.guns[new Random().Next(0, player.guns.Count)]; //Get a random gun held by the player
                gun.totalAmmo += new Random().Next(1, gun.ammoSize); //Add a random ammount of ammo to that gun
            }
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            if (player.guns.Count > 0) //If the player has a gun to put ammo in
            g.FillRectangle(Brushes.brushBlack, screenRectangle);
        }

        public override void DrawOverLayer(System.Drawing.Graphics g)
        {


            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, screenRectangle.Width, screenRectangle.Height);
            if (player.guns.Count <= 0)
                return;

            if (!this.screenRectangle.IntersectsWith(Globals.screenRectangle))
                return;

            if (screenRect.IntersectsWith(this.screenRectangle))
            {
                g.DrawString("Press SPACE to pick up\n" + "Ammo", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
            }
        }
    }
}
