﻿// Author: Alexey Rybakov

using System;
using System.Diagnostics;

namespace Lib.Maths.Geometry.Geometry3D
{
    /// <summary>
    /// 3D point.
    /// </summary>
    public class Point : Vector
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Point()
            : base()
        {
        }

        /// <summary>
        /// Constructor from coordinates.
        /// </summary>
        /// <param name="x">coordinate <c>x</c></param>
        /// <param name="y">coordinate <c>y</c></param>
        /// <param name="z">coordinate <c>z</c></param>
        public Point(double x, double y, double z)
            : base(x, y, z)
        {
        }

        /// <summary>
        /// Constructor from vector.
        /// </summary>
        /// <param name="v">vector</param>
        public Point(Vector v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Distance to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>distance</returns>
        public double Dist(Point p)
        {
            return (this - p).Mod;
        }

        /// <summary>
        /// Distance to point on axis <c>x</c>.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>distance</returns>
        public double DistX(Point p)
        {
            return Math.Abs(X - p.X);
        }

        /// <summary>
        /// Distance to point on axis <c>y</c>.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>distance</returns>
        public double DistY(Point p)
        {
            return Math.Abs(Y - p.Y);
        }

        /// <summary>
        /// Distance to point on axis <c>z</c>.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>distance</returns>
        public double DistZ(Point p)
        {
            return Math.Abs(Z - p.Z);
        }

        /// <summary>
        /// Distance to point on given axis.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="ax">axis</param>
        /// <returns>distance</returns>
        public double Dist(Point p, AxisType ax)
        {
            switch (ax)
            {
                case AxisType.X:
                    return DistX(p);

                case AxisType.Y:
                    return DistY(p);

                case AxisType.Z:
                    return DistZ(p);

                default:
                    Debug.Assert(false);
                    return 0.0;
            }
        }

        /// <summary>
        /// Adding vector to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="v">vector</param>
        /// <returns>new point</returns>
        public static Point operator +(Point p, Vector v)
        {
            return new Point((p as Vector) + v);
        }

        /// <summary>
        /// Subtracting vector from point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="v">vector</param>
        /// <returns>new point</returns>
        public static Point operator -(Point p, Vector v)
        {
            return p + (-v);
        }

        /// <summary>
        /// Random point.
        /// </summary>
        /// <param name="par">parallelepiped</param>
        /// <returns>random point</returns>
        public static new Point Random(Parallelepiped par)
        {
            return new Point(Vector.Random(par));
        }

        /// <summary>
        /// Random point on parallelepiped surface.
        /// </summary>
        /// <param name="par">parallelepiped</param>
        /// <returns>random point</returns>
        public static new Point RandomOnSurface(Parallelepiped par)
        {
            return new Point(Vector.RandomOnSurface(par));
        }

        /// <summary>
        /// Return point to parallelepiped.
        /// </summary>
        /// <param name="par">parallelepiped</param>
        public void ReturnToParallelepiped(Parallelepiped par)
        {
            if (X > par.Right)
            {
                X = par.Right;
            }

            if (X < par.Left)
            {
                X = par.Left;
            }

            if (Y > par.Top)
            {
                Y = par.Top;
            }

            if (Y < par.Bottom)
            {
                Y = par.Bottom;
            }

            if (Z > par.Front)
            {
                Z = par.Front;
            }

            if (Z < par.Back)
            {
                Z = par.Back;
            }
        }

        /// <summary>
        /// Check if point is inside of parallelepiped.
        /// </summary>
        /// <param name="par">parallelepiped</param>
        /// <returns><c>true</c> - if point is inside of parallelepiped, <c>false</c> - otherwise</returns>
        public bool IsIn(Parallelepiped par)
        {
            return par.XInterval.Contains(X) && par.YInterval.Contains(Y) && par.ZInterval.Contains(Z);
        }

        /// <summary>
        /// Toroid distance.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="par">parallelepiped</param>
        /// <returns>distance</returns>
        public double ToroidDist(Point p, Parallelepiped par)
        {
            return ToroidDir(p, par).Mod;
        }

        /// <summary>
        /// Toroid direction to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="par">parallelepiped</param>
        /// <returns>vector in direction to given point</returns>
        public Vector ToroidDir(Point p, Parallelepiped par)
        {
            Debug.Assert(IsIn(par) && p.IsIn(par), "Toroid direction is available only for inner points.");

            // First find direction for X component.
            double dx = DistX(p);
            double edx = 0.0;
            if (dx <= par.Width - dx)
            {
                // Inner dist.
                // |----- this =====> p -----|
                edx = p.X - X;
            }
            else
            {
                // Outer toroid dist.
                if (X > p.X)
                {
                    // Direction to the right.
                    // |----- p ----- this =====>|
                    edx = (par.Right - X) + (p.X - par.Left);
                }
                else
                {
                    // Direction to the left.
                    // |<===== this ----- p -----|
                    edx = (par.Left - X) + (p.X - par.Right);
                }
            }

            // The same action for Y component.
            double dy = DistY(p);
            double edy = 0.0;
            if (dy <= par.Height - dy)
            {
                edy = p.Y - Y;
            }
            else
            {
                if (Y > p.Y)
                {
                    edy = (par.Top - Y) + (p.Y - par.Bottom);
                }
                else
                {
                    edy = (par.Bottom - Y) + (p.Y - par.Top);
                }
            }

            // The same action for Z component.
            double dz = DistZ(p);
            double edz = 0.0;
            if (dz <= par.Depth - dz)
            {
                edz = p.Z - Z;
            }
            else
            {
                if (Z > p.Z)
                {
                    edz = (par.Front - Z) + (p.Z - par.Back);
                }
                else
                {
                    edz = (par.Back - Z) + (p.Z - par.Front);
                }
            }

            return new Vector(edx, edy, edz);
        }

        /// <summary>
        /// Move point in toroid.
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="par">parallelepiped</param>
        public void ToroidMove(Vector v, Parallelepiped par)
        {
            Debug.Assert(IsIn(par), "Toroid operations are available only for inner points.");
            Debug.Assert(v.Mod < par.Radius, "Too big shift for toroid operation");

            X += v.X;
            if (X > par.Right)
            {
                X -= par.Width;
            }
            else if (X < par.Left)
            {
                X += par.Width;
            }

            Y += v.Y;
            if (Y > par.Top)
            {
                Y -= par.Height;
            }
            else if (Y < par.Bottom)
            {
                Y += par.Height;
            }

            Z += v.Z;
            if (Z > par.Front)
            {
                Z -= par.Depth;
            }
            else if (Z < par.Back)
            {
                Z += par.Depth;
            }
        }
    }
}
