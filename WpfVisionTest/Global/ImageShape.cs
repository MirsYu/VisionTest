using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDesigner;

namespace WpfVisionTest.Global
{
    public static class ImageShape
    {

        public static void DrawRect(MVDRenderActivex Render,CMvdRectangleF mvdRectangleF)
        {
            Render.AddShape(mvdRectangleF);
            Render.Display(MVDRenderActivex.MVD_REFRESH_MODE.Sync);
        }

        public static void DrawRect(MVDRenderActivex Render, CMvdRectangleF mvdRectangleF)
        {
            Render.AddShape(mvdRectangleF);
            Render.Display(MVDRenderActivex.MVD_REFRESH_MODE.Sync);
        }
    }
}
