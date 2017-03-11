﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.Maths.Geometry.Geometry3D;
using Lib.Maths.Geometry;

namespace Lib.MathMod.Grid
{
    /// <summary>
    /// Block.
    /// </summary>
    public class Block : DescartesObject
    {
        /// <summary>
        /// Grid.
        /// </summary>
        public StructuredGrid Grid;

        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Partition number.
        /// </summary>
        public int PartitionNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Nodes.
        /// </summary>
        public Point[,,] Nodes = null;

        /// <summary>
        /// Constructor from identifier and sizes.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="id">identifier</param>
        /// <param name="isize">count of cells in I direction</param>
        /// <param name="jsize">count of cells in J direction</param>
        /// <param name="ksize">count of cells in K direction</param>
        public Block(StructuredGrid g, int id, int isize, int jsize, int ksize)
            : base(new ISegm(0, isize), new ISegm(0, jsize), new ISegm(0, ksize))
        {
            Grid = g;
            Id = id;
            PartitionNumber = 0;
        }

        /// <summary>
        /// Reshape block.
        /// </summary>
        /// <param name="isize">new block size in I direction</param>
        /// <param name="jsize">new block size in J direction</param>
        /// <param name="ksize">new block size in K direction</param>
        public void Reshape(int isize, int jsize, int ksize)
        {
            I.H = isize;
            J.H = jsize;
            K.H = ksize;
            Nodes = null;
        }

        /// <summary>
        /// Allocate memory.
        /// </summary>
        public void Allocate()
        {
            // Allocate nodes.
            Nodes = new Point[INodes, JNodes, KNodes];

            for (int i = 0; i < INodes; i++)
            {
                for (int j = 0; j < JNodes; j++)
                {
                    for (int k = 0; k < KNodes; k++)
                    {
                        Nodes[i, j, k] = new Point();
                    }
                }
            }
        }

        /// <summary>
        /// Get corner node in given 3 directions.
        /// </summary>
        /// <param name="d1">first direction</param>
        /// <param name="d2">second direction</param>
        /// <param name="d3">third direction</param>
        /// <returns>corner node</returns>
        public Point CornerNode(Dir d1, Dir d2, Dir d3)
        {
            if (!Dir.IsBasis(d1, d2, d3))
            {
                return null;
            }

            // Generate the mask.
            int mask = (1 << d1.N) | (1 << d2.N) | (1 << d3.N);

            int i = 0;
            int j = 0;
            int k = 0;

            if ((mask & (1 << Dir.I1N)) != 0)
            {
                i = ISize;
            }

            if ((mask & (1 << Dir.J1N)) != 0)
            {
                j = JSize;
            }

            if ((mask & (1 << Dir.K1N)) != 0)
            {
                k = KSize;
            }

            return Nodes[i, j, k];
        }

        /// <summary>
        /// Cast to string.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return String.Format("{0,4} ({1,3}): {2,8} cells ({3,3}, {4,3}, {5,3})",
                                 Id, PartitionNumber, CellsCount, ISize, JSize, KSize);
        }
    }
}
