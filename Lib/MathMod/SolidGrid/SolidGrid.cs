﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.Maths.Geometry.Geometry3D;

namespace Lib.MathMod.SolidGrid
{
    /// <summary>
    /// Solid grid.
    /// </summary>
    public class SolidGrid
    {
        /// <summary>
        /// Cells count in OX direction.
        /// </summary>
        public int NX;

        /// <summary>
        /// Cells count in OY direction.
        /// </summary>
        public int NY;

        /// <summary>
        /// Cells count in OZ direction.
        /// </summary>
        public int NZ;

        /// <summary>
        /// Space step.
        /// </summary>
        public double CellL;

        /// <summary>
        /// Square of cell edge.
        /// </summary>
        public double CellS;

        /// <summary>
        /// Volume of cell.
        /// </summary>
        public double CellV;

        /// <summary>
        /// Cells.
        /// </summary>
        public Cell[,,] Cells;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nx">cells count in OX direction</param>
        /// <param name="ny">cells count in OY direction</param>
        /// <param name="nz">cells count in OZ direction</param>
        /// <param name="dl">space step</param>
        public SolidGrid(int nx, int ny, int nz, double dl)
        {
            NX = nx;
            NY = ny;
            NZ = nz;
            CellL = dl;
            CellS = CellL * CellL;
            CellV = CellS * CellL;

            Cells = new Cell[NX, NY, NZ];

            for (int i = 0; i < NX; i++)
            {
                for (int j = 0; j < NY; j++)
                {
                    for (int k = 0; k < NZ; k++)
                    {
                        Cells[i, j, k] = new Cell();
                    }
                }
            }
        }

        /// <summary>
        /// Size in OX direction.
        /// </summary>
        public double XSize
        {
            get
            {
                return NX * CellL;
            }
        }

        /// <summary>
        /// Size in OY direction.
        /// </summary>
        public double YSize
        {
            get
            {
                return NY * CellL;
            }
        }

        /// <summary>
        /// Size in OZ direction.
        /// </summary>
        public double ZSize
        {
            get
            {
                return NZ * CellL;
            }
        }

        /// <summary>
        /// Convert U to D for all cells.
        /// </summary>
        public void UtoD()
        {
            for (int xi = 0; xi < NX; xi++)
            {
                for (int yi = 0; yi < NY; yi++)
                {
                    for (int zi = 0; zi < NZ; zi++)
                    {
                        Cells[xi, yi, zi].UtoD(CellV);
                    }
                }
            }
        }

        /// <summary>
        /// Convert D to U for all cells.
        /// </summary>
        public void DtoU()
        {
            for (int xi = 0; xi < NX; xi++)
            {
                for (int yi = 0; yi < NY; yi++)
                {
                    for (int zi = 0; zi < NZ; zi++)
                    {
                        Cells[xi, yi, zi].DtoU(CellV);
                    }
                }
            }
        }

        /// <summary>
        /// X next U data.
        /// </summary>
        /// <param name="xi">X direction index</param>
        /// <param name="yi">Y direction index</param>
        /// <param name="zi">Z direction index</param>
        /// <returns></returns>
        public U XNextU(int xi, int yi, int zi)
        {
            return (xi < NX - 1)
                   ? Cells[xi + 1, yi, zi].U
                   : Cells[xi, yi, zi].U.MirrorX;
        }

        /// <summary>
        /// X prev U data.
        /// </summary>
        /// <param name="xi">X direction index</param>
        /// <param name="yi">Y direction index</param>
        /// <param name="zi">Z direction index</param>
        /// <returns></returns>
        public U XPrevU(int xi, int yi, int zi)
        {
            return (xi > 0)
                   ? Cells[xi - 1, yi, zi].U
                   : Cells[xi, yi, zi].U.MirrorX;
        }

        /// <summary>
        /// Y next U data.
        /// </summary>
        /// <param name="xi">X direction index</param>
        /// <param name="yi">Y direction index</param>
        /// <param name="zi">Z direction index</param>
        /// <returns></returns>
        public U YNextU(int xi, int yi, int zi)
        {
            return (yi < NY - 1)
                   ? Cells[xi, yi + 1, zi].U
                   : Cells[xi, yi, zi].U.MirrorY;
        }

        /// <summary>
        /// Y prev U data.
        /// </summary>
        /// <param name="xi">X direction index</param>
        /// <param name="yi">Y direction index</param>
        /// <param name="zi">Z direction index</param>
        /// <returns></returns>
        public U YPrevU(int xi, int yi, int zi)
        {
            return (yi > 0)
                   ? Cells[xi, yi - 1, zi].U
                   : Cells[xi, yi, zi].U.MirrorY;
        }

        /// <summary>
        /// Z next U data.
        /// </summary>
        /// <param name="xi">X direction index</param>
        /// <param name="yi">Y direction index</param>
        /// <param name="zi">Z direction index</param>
        /// <returns></returns>
        public U ZNextU(int xi, int yi, int zi)
        {
            return (zi < NZ - 1)
                   ? Cells[xi, yi, zi + 1].U
                   : Cells[xi, yi, zi].U.MirrorZ;
        }

        /// <summary>
        /// Z prev U data.
        /// </summary>
        /// <param name="xi">X direction index</param>
        /// <param name="yi">Y direction index</param>
        /// <param name="zi">Z direction index</param>
        /// <returns></returns>
        public U ZPrevU(int xi, int yi, int zi)
        {
            return (zi > 0)
                   ? Cells[xi, yi, zi - 1].U
                   : Cells[xi, yi, zi].U.MirrorZ;
        }
    }
}
