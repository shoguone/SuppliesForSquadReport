using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuppliesForSquadReport.Utility
{
    public class ReportRotator
    {
        public static void Rotate(StiReport report)
        {
            foreach (StiPage page in report.RenderedPages)
            {
                report.RenderedPages.GetPage(page);
                double pageWidth = page.Width;
                double pageHeight = page.Height;

                page.DisplayRectangle = new Stimulsoft.Base.Drawing.RectangleD(0, 0, page.DisplayRectangle.Height, page.DisplayRectangle.Width);
                if (page.Orientation == StiPageOrientation.Landscape)
                {
                    page.Orientation = StiPageOrientation.Portrait;
                }
                else
                {
                    page.Orientation = StiPageOrientation.Landscape;
                }

                System.Collections.Hashtable newComps = new System.Collections.Hashtable();

                foreach (StiComponent comp in page.Components)
                {
                    Stimulsoft.Base.Drawing.RectangleD compRect = comp.DisplayRectangle;
                    Stimulsoft.Base.Drawing.RectangleD newRect = new Stimulsoft.Base.Drawing.RectangleD(
                        compRect.Y,
                        pageWidth - compRect.Right,
                        compRect.Height,
                        compRect.Width);

                    comp.DisplayRectangle = newRect;

                    IStiEnumAngle angleComp = comp as IStiEnumAngle;
                    if (angleComp != null)
                    {
                        StiAngle angle = angleComp.Angle;
                        if (angle == StiAngle.Angle0) angle = StiAngle.Angle90;
                        else if (angle == StiAngle.Angle90) angle = StiAngle.Angle180;
                        else if (angle == StiAngle.Angle180) angle = StiAngle.Angle270;
                        else if (angle == StiAngle.Angle270) angle = StiAngle.Angle0;
                        angleComp.Angle = angle;
                    }

                    StiText stiText = comp as StiText;
                    if (stiText != null)
                    {
                        stiText.Angle += 90;
                        if (stiText.Angle >= 360) stiText.Angle -= 360;
                    }

                    //StiImage image = comp as StiImage;
                    //if (image != null)
                    //{
                    //    if (image.ImageRotation == StiImageRotation.None) image.ImageRotation = StiImageRotation.Rotate90CCW;
                    //    else if (image.ImageRotation == StiImageRotation.Rotate90CCW) image.ImageRotation = StiImageRotation.Rotate180;
                    //    else if (image.ImageRotation == StiImageRotation.Rotate180) image.ImageRotation = StiImageRotation.Rotate90CW;
                    //    else if (image.ImageRotation == StiImageRotation.Rotate90CW) image.ImageRotation = StiImageRotation.None;
                    //}

                    IStiBorder border = comp as IStiBorder;
                    if (border != null)
                    {
                        //if (border.Border is Stimulsoft.Base.Drawing.StiAdvancedBorder)
                        //{
                        //    Stimulsoft.Base.Drawing.StiAdvancedBorder advBorder = border.Border as Stimulsoft.Base.Drawing.StiAdvancedBorder;
                        //    Stimulsoft.Base.Drawing.StiAdvancedBorder newBorder = new Stimulsoft.Base.Drawing.StiAdvancedBorder(
                        //        new Stimulsoft.Base.Drawing.StiBorderSide(advBorder.RightSide.Color, advBorder.RightSide.Size, advBorder.RightSide.Style),
                        //        new Stimulsoft.Base.Drawing.StiBorderSide(advBorder.LeftSide.Color, advBorder.LeftSide.Size, advBorder.LeftSide.Style),
                        //        new Stimulsoft.Base.Drawing.StiBorderSide(advBorder.TopSide.Color, advBorder.TopSide.Size, advBorder.TopSide.Style),
                        //        new Stimulsoft.Base.Drawing.StiBorderSide(advBorder.BottomSide.Color, advBorder.BottomSide.Size, advBorder.BottomSide.Style),
                        //        advBorder.DropShadow,
                        //        advBorder.ShadowSize,
                        //        advBorder.ShadowBrush);
                        //    border.Border = newBorder;
                        //}
                        //else
                        //{
                            Stimulsoft.Base.Drawing.StiBorder oldBorder = border.Border;
                            Stimulsoft.Base.Drawing.StiBorder newBorder = (Stimulsoft.Base.Drawing.StiBorder)oldBorder.Clone();
                            newBorder.Side = Stimulsoft.Base.Drawing.StiBorderSides.None;
                            if (oldBorder.IsTopBorderSidePresent) newBorder.Side |= Stimulsoft.Base.Drawing.StiBorderSides.Left;
                            if (oldBorder.IsBottomBorderSidePresent) newBorder.Side |= Stimulsoft.Base.Drawing.StiBorderSides.Right;
                            if (oldBorder.IsRightBorderSidePresent) newBorder.Side |= Stimulsoft.Base.Drawing.StiBorderSides.Top;
                            if (oldBorder.IsLeftBorderSidePresent) newBorder.Side |= Stimulsoft.Base.Drawing.StiBorderSides.Bottom;
                            border.Border = newBorder;
                        //}
                    }

                    if (comp is StiVerticalLinePrimitive)
                    {
                        StiHorizontalLinePrimitive horLine = new StiHorizontalLinePrimitive();
                        horLine.Parent = comp.Parent;
                        horLine.DisplayRectangle = newRect;
                        newComps[comp] = horLine;
                    }
                    if (comp is StiHorizontalLinePrimitive)
                    {
                        StiVerticalLinePrimitive vertLine = new StiVerticalLinePrimitive();
                        vertLine.Parent = comp.Parent;
                        vertLine.DisplayRectangle = newRect;
                        newComps[comp] = vertLine;
                    }

                }
                if (newComps.Count > 0)
                {
                    foreach (System.Collections.DictionaryEntry de in newComps)
                    {
                        int compIndex = page.Components.IndexOf((StiComponent)de.Key);
                        page.Components[compIndex] = (StiComponent)de.Value;
                    }
                }
            }
        }
    }
}
