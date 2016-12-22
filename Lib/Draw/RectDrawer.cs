﻿// Author: Alexey Rybakov

using System;

using Lib.Maths.Geometry.Geometry2D;

namespace Lib.Draw
{
    /// <summary>
    /// Drawing in rectangle scope.
    /// </summary>
    public abstract class RectDrawer
    {
        /// <summary>
        /// Initial scope.
        /// </summary>
        public Rect Rect
        {
            get;
            protected set;
        }

        /// <summary>
        /// Scaler.
        /// </summary>
        public RectScaler Scaler = null;

        /// <summary>
        /// Recalculate scaler.
        /// </summary>
        protected abstract void MakeScaler();

        /// <summary>
        /// Information abount bijection.
        /// </summary>
        /// <returns>string information</returns>
        public abstract string GetBijectionString();

        /// <summary>
        /// Begin draw.
        /// </summary>
        public abstract void BeginDraw();

        /// <summary>
        /// End draw.
        /// </summary>
        public abstract void EndDraw();

        /// <summary>
        /// Setting pen color.
        /// </summary>
        /// <param name="color">color</param>
        public abstract void SetPenColor(Color color);

        /// <summary>
        /// Setting pen thickness.
        /// </summary>
        /// <param name="thickness">thickness</param>
        public abstract void SetPenThickness(double thickness);

        /// <summary>
        /// Set rectangle.
        /// </summary>
        /// <param name="rect">rectangle</param>
        public void SetRect(Rect rect)
        {
            Rect = rect;
            Rect.OnChange += new EventHandler((sender, e) => MakeScaler());
            MakeScaler();
        }

        /// <summary>
        /// Set pen.
        /// </summary>
        /// <param name="color">color</param>
        /// <param name="thickness">thickness</param>
        public void SetPen(Color color, double thickness)
        {
            SetPenColor(color);
            SetPenThickness(thickness);
        }

        /// <summary>
        /// Set brush.
        /// </summary>
        /// <param name="color">color</param>
        public abstract void SetBrush(Color color);

        /// <summary>
        /// Draw line.
        /// </summary>
        /// <param name="a">begin</param>
        /// <param name="b">end</param>
        public abstract void DrawLine(Point a, Point b);

        /// <summary>
        /// Draw line with margins.
        /// </summary>
        /// <param name="a">begin</param>
        /// <param name="b">end</param>
        /// <param name="margin">margin value</param>
        public abstract void DrawMarginedLine(Point a, Point b, double margin);

        /// <summary>
        /// Draw point (circle with constant radius).
        /// </summary>
        /// <param name="p">center</param>
        /// <param name="r">radius</param>
        public abstract void DrawPoint(Point p, double r);

        /// <summary>
        /// Fill point.
        /// </summary>
        /// <param name="p">center</param>
        /// <param name="r">radius</param>
        public abstract void FillPoint(Point p, double r);

        /// <summary>
        /// Draw rectangle.
        /// </summary>
        /// <param name="r">rectangle</param>
        public abstract void DrawRect(Rect r);

        /// <summary>
        /// Fill rectangle.
        /// </summary>
        /// <param name="r">rectangle</param>
        public abstract void FillRect(Rect r);

        /// <summary>
        /// Draw ellipse.
        /// </summary>
        /// <param name="p">center</param>
        /// <param name="rx">radius <c>x</c></param>
        /// <param name="ry">radius <c>y</c></param>
        public abstract void DrawEllipse(Point p, double rx, double ry);

        /// <summary>
        /// Fill ellipse.
        /// </summary>
        /// <param name="p">center</param>
        /// <param name="rx">radius <c>x</c></param>
        /// <param name="ry">radius <c>y</c></param>
        public abstract void FillEllipse(Point p, double rx, double ry);

        /// <summary>
        /// Draw text.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="off">offset</param>
        /// <param name="text">test</param>
        /// <param name="size">size</param>
        /// <param name="family">font</param>
        public abstract void DrawText(Point p, Vector off, string text, double size, string family);

        /// <summary>
        /// Draw text with zero offset.
        /// </summary>
        /// <param name="p">base point</param>
        /// <param name="text">text</param>
        /// <param name="size">size</param>
        /// <param name="family">font</param>
        public void DrawText(Point p, string text, double size, string family)
        {
            DrawText(p, new Vector(0.0, 0.0), text, size, family);
        }

        /// <summary>
        /// Draw <c>Verdana</c> text.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="off">offset</param>
        /// <param name="text">text</param>
        /// <param name="size">size</param>
        public void DrawVerdanaText(Point p, Vector off, string text, double size)
        {
            DrawText(p, off, text, size, "Verdana");
        }

        /// <summary>
        /// Draw <c>Verdana</c> text.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="text">text</param>
        /// <param name="size">size</param>
        public void DrawVerdanaText(Point p, string text, double size)
        {
            DrawText(p, text, size, "Verdana");
        }

        /// <summary>
        /// Draw <c>Lucida Console</c> text.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="off"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        public void DrawConsoleText(Point p, Vector off, string text, double size)
        {
            DrawText(p, off, text, size, "Verdana");
        }

        /// <summary>
        /// Draw <c>Lucida Console</c> text.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="text">text</param>
        /// <param name="size">size</param>
        public void DrawConsoleText(Point p, string text, double size)
        {
            DrawText(p, text, size, "Lucida Console");
        }
    }
}
