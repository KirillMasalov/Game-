using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    public interface IDrawable
    {
        Point Position { get; set; }
        float Direction { get; set; }
        int Speed { get; set; }

        void Move(Size offset);
        void Move(int x, int y);
    }
}
