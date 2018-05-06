using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaSound.RetroPixelPro
{
    public struct ColormapValue
    {
        public int primaryPaletteIndex;
        public int secondaryPaletteIndex;
        public float blend;

        public ColormapValue(int primaryPaletteIndex, int secondaryPaletteIndex, float blend)
        {
            this.primaryPaletteIndex = primaryPaletteIndex;
            this.secondaryPaletteIndex = secondaryPaletteIndex;
            this.blend = blend;
        }

        public Color32 ToColor32()
        {
            return new Color32((byte)primaryPaletteIndex, (byte)secondaryPaletteIndex, (byte)(blend * 255), (byte)255);
        }

    }
}

