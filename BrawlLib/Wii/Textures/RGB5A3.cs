﻿using System;
using BrawlLib.Imaging;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class RGB5A3 : TextureConverter
    {
        public override int BitsPerPixel => 16;
        public override int BlockWidth => 4;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format32bppArgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.RGB5A3;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            var sPtr = (wRGB5A3Pixel*) blockAddr;
            //ARGBPixel* dPtr = (ARGBPixel*)destAddr;
            for (var y = 0; y < BlockHeight; y++, dPtr += width)
            for (var x = 0; x < BlockWidth;)
                dPtr[x++] = (ARGBPixel) (*sPtr++);
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            var dPtr = (wRGB5A3Pixel*) blockAddr;
            for (var y = 0; y < BlockHeight; y++, sPtr += width)
            for (var x = 0; x < BlockWidth;)
                *dPtr++ = (wRGB5A3Pixel) sPtr[x++];
        }
    }
}