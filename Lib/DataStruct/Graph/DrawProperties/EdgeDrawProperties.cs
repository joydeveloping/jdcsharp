﻿// Author: Alexey Rybakov

using System;

using Lib.Draw;

namespace Lib.DataStruct.Graph.DrawProperties
{
    /// <summary>
    /// Edge draw properties.
    /// </summary>
    public class EdgeDrawProperties : ICloneable
    {
        /// <summary>
        /// Color.
        /// </summary>
        public Color Color = null;

        /// <summary>
        /// Thickness.
        /// </summary>
        public double Thickness = 0.0;

        /// <summary>
        /// Points margin.
        /// </summary>
        public double NodesMargin = 0;

        /// <summary>
        /// Default constructor..
        /// </summary>
        public EdgeDrawProperties()
            : this(new Color(0xFF, 0xAA, 0xAA, 0xAA))
        {
        }

        /// <summary>
        /// Common colored esge.
        /// </summary>
        /// <param name="color">color</param>
        public EdgeDrawProperties(Color color)
        {
            Color = color;
            Thickness = 1.0;
            NodesMargin = 8.0;
        }

        /// <summary>
        /// Constructoor from string.
        /// </summary>
        /// <param name="str">string</param>
        public EdgeDrawProperties(string str)
        {
            string[] s = str.Split(new char[] { ';', '=' });

            // Parse.
            Color = new Color(s[1]);
            Thickness = Double.Parse(s[3]);
            NodesMargin = Double.Parse(s[5]);
        }

        /// <summary>
        /// Clone.
        /// </summary>
        /// <returns>clone</returns>
        public object Clone()
        {
            EdgeDrawProperties edprops = new EdgeDrawProperties();

            edprops.Color = Color.Clone() as Color;
            edprops.Thickness = Thickness;
            edprops.NodesMargin = NodesMargin;

            return edprops;
        }

        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return String.Format("color={0};thickness={1};nodes_margin={2}",
                                 Color, Thickness, NodesMargin);
        }
    }
}
