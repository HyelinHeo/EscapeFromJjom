using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct TileType
{
    public int TileCount;
    public int TilePercent;
    public TileType(int tileCount, int percent)
    {
        TileCount = tileCount;
        TilePercent = percent;
    }
}
