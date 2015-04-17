using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Items
{
    //Item to be picked up on map
    public abstract class Item 
    {
        //Rectangle to draw/calculate
        public Rectangle screenRectangle
        {
            get 
            { 
                return new Rectangle(screenX, screenY, size * Globals.scale, size * Globals.scale); 
            }
        }

        //Check if this item should be removed on next iteration of the game loop
        public bool ShouldRemove;

        //X position relative to the screen
        protected int screenX;
        //Y position relative to the screen
        protected int screenY;
        //X coordinate of the tile, relative to the layer
        protected int tileX;
        //Y coordinate of the tile, relative to the layer
        protected int tileY;
        //X coordinate of the layer
        protected int layerTileX;
        //Y coordinate of the layer
        protected int layerTileY;
        //size of the tile (not scaled)
        protected int size;

        #region Game Loop Methods

        public abstract void Init();
        
        public abstract void Update();
        
        public abstract void Draw(Graphics g);
        
        public abstract void DrawOverLayer(System.Drawing.Graphics g);

        #endregion
    }
}
