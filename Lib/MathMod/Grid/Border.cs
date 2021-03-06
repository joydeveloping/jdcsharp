﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.Maths.Numbers;
using Lib.Maths.Geometry;
using Lib.Maths.Geometry.Geometry3D;

namespace Lib.MathMod.Grid
{
    /// <summary>
    /// Border of block.
    /// </summary>
    public abstract class Border : ThinDescartesObject
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Block.
        /// </summary>
        public Block B
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="b">block</param>
        /// <param name="i">I direction nodes interval</param>
        /// <param name="j">J direction nodes interval</param>
        /// <param name="k">K direction nodes interval </param>
        public Border(int id, Block b, ISegm i, ISegm j, ISegm k)
            : base(i, j, k)
        {
            Id = id;
            B = b;
        }

        /// <summary>
        /// Check if border is iface.
        /// </summary>
        /// <returns><c>true</c> - if it is iface, <c>false</c> - otherwise</returns>
        public abstract bool IsIface();

        /// <summary>
        /// Check if border if bcond.
        /// </summary>
        /// <returns><c>true</c> - if it is bcond, <c>false</c> - otherwise</returns>
        public abstract bool IsBCond();

        /// <summary>
        /// Get corner node by two directions.
        /// </summary>
        /// <param name="d1">first direction</param>
        /// <param name="d2">second direction</param>
        /// <returns></returns>
        public Point CornerNode(Dir d1, Dir d2)
        {
            if (!Dir.IsBasis(D, d1, d2))
            {
                return null;
            }

            int s1 = d1.N;
            int s2 = d2.N;

            if (d1.Gen.N > d2.Gen.N)
            {
                s1 = d2.N;
                s2 = d1.N;
            }

            int i = I0;
            int j = J0;
            int k = K0;

            if (s1 == Dir.I1N)
            {
                i = I1;
            }
            else if (s1 == Dir.J1N)
            {
                j = J1;
            }

            if (s2 == Dir.J1N)
            {
                j = J1;
            }
            else if (s2 == Dir.K1N)
            {
                k = K1;
            }

            return new Point(B.C[i, j, k, 0], B.C[i, j, k, 1], B.C[i, j, k, 2]);
        }

        /// <summary>
        /// Check match of two borders with directions.
        /// </summary>
        /// <param name="b1">first border</param>
        /// <param name="od11">first direction of first border</param>
        /// <param name="od12">second direction of first border</param>
        /// <param name="b2">second border</param>
        /// <param name="od21">first direction of second border</param>
        /// <param name="od22">second direction of second border</param>
        /// <returns><c>true</c> - if objects match, <c>false</c> - otherwise</returns>
        public static bool IsMatch(Border b1, Dir od11, Dir od12,
                                   Border b2, Dir od21, Dir od22)
        {
            Point p1 = b1.CornerNode(od11, od12);
            Point p2 = b2.CornerNode(od21, od22);

            if ((p1 == null) || (p2 == null))
            {
                return false;
            }

            return (p1 - p2).Mod2 < Constants.Eps;
        }

        /// <summary>
        /// Check sizes match of two borders.
        /// </summary>
        /// <param name="b">border to check with</param>
        /// <param name="dirs">dirs relations</param>
        /// <returns><c>true</c> - if sizes match, <c>false</c> - otherwise</returns>
        public bool IsSizesMatch(Border b, Dirs3 dirs)
        {
            return (Size(Dir.I) == b.Size(dirs.I.Gen))
                   && (Size(Dir.J) == b.Size(dirs.J.Gen))
                   && (Size(Dir.K) == b.Size(dirs.K.Gen));
        }

        /// <summary>
        /// Find directions match for other border.
        /// </summary>
        /// <param name="b">second object</param>
        /// <param name="is_codirectional">codirectional flag</param>
        /// <param name="eps">epsilon</param>
        /// <returns>directions - if objects math, null - otherwise</returns>
        public Dirs3 DirectionsMatchFixed(Border b, bool is_codirectional, double eps)
        {
            BorderCorners bc_this = new BorderCorners(this);
            BorderCorners bc = new BorderCorners(b);
            Dirs3 dirs = bc_this.DirectionsMatchFixed(bc, is_codirectional, eps);

            if (dirs == null)
            {
                return null;
            }

            return IsSizesMatch(b, dirs) ? dirs : null;
        }

        /// <summary>
        /// Find directions match with parallel move.
        /// </summary>
        /// <param name="b">second object</param>
        /// <param name="is_codirectional">codirectional flag</param>
        /// <param name="eps">epsilon</param>
        /// <returns>directions - if objects match, null - otherwise</returns>
        public Dirs3 DirectionsMatchParallelMove(Border b, bool is_codirectional, double eps)
        {
            BorderCorners bc_this = new BorderCorners(this);
            BorderCorners bc = new BorderCorners(b);
            Dirs3 dirs = bc_this.DirectionsMatchParallelMove(bc, is_codirectional, eps);

            if (dirs == null)
            {
                return null;
            }

            return IsSizesMatch(b, dirs) ? dirs : null;
        }

        /// <summary>
        /// Find directions match with RotX.
        /// </summary>
        /// <param name="b">second object</param>
        /// <param name="is_codirectional">codirectional flag</param>
        /// <param name="eps">epsilon</param>
        /// <returns>directions - if objects match, null - otherwise</returns>
        public Dirs3 DirectionsMatchRotX(Border b, bool is_codirectional, double eps)
        {
            BorderCorners bc_this = new BorderCorners(this);
            BorderCorners bc = new BorderCorners(b);
            Dirs3 dirs = bc_this.DirectionsMatchRotX(bc, is_codirectional, eps);

            if (dirs == null)
            {
                return null;
            }

            return IsSizesMatch(b, dirs) ? dirs : null;
        }
    }
}
