﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Lib.Maths.Geometry;
using Lib.MathMod.SolidGrid;

namespace Lib.Draw
{
    public class HydroRectDrawer
    {
        /// <summary>
        /// Drawer.
        /// </summary>
        private RectDrawer Drawer = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="drawer">drawer</param>
        public HydroRectDrawer(RectDrawer drawer)
        {
            Debug.Assert(drawer != null);
            Drawer = drawer;
        }

        /// <summary>
        /// Draw hydro for structured grid.
        /// </summary>
        /// <param name="grid">grid</param>
        public void DrawHydro(SolidGrid grid)
        {
            DrawCellsCanvas(grid);
        }

        /// <summary>
        /// Draw cells canvas (borders).
        /// </summary>
        /// <param name="grid">grid</param>
        public void DrawCellsCanvas(SolidGrid grid)
        {
            // x - dl step
            // y - dl step

            for (int i = 0; i <= grid.NX; i++)
            {
                Drawer.DrawVLine(grid.CellL * i);
            }

            for (int i = 0; i <= grid.NY; i++)
            {
                Drawer.DrawHLine(grid.CellL * i);
            }
        }

        /// <summary>
        /// Draw field of some physical value.
        /// </summary>
        /// <param name="grid">grid</param>
        /// <param name="lo">lower bound</param>
        /// <param name="hi">higher bound</param>
        public void DrawField(SolidGrid grid, double lo, double hi)
        {
            double d = grid.CellL / 10.0;

            // Now we use rho value.

            for (int i = 0; i < grid.NX; i++)
            {
                for (int j = 0; j < grid.NY; j++)
                {
                    Color c = Color.Gray(grid.Cells[i, j, 0].U.rho, lo, hi);
                    Drawer.SetBrush(c);
                    Drawer.FillRect(grid.CellL * i - d,
                                    grid.CellL * j - d,
                                    grid.CellL * (i + 1) + d,
                                    grid.CellL * (j + 1) + d);   
                }
            }
        }
    }
}
