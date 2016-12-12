﻿// Author: Alexey Rybakov

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

using Lib.DataStruct.Graph.DrawProperties;
using Point2D = Lib.Maths.Geometry.Geometry2D.Point;
using Point3D = Lib.Maths.Geometry.Geometry3D.Point;

namespace Lib.DataStruct.Graph
{
    /// <summary>
    /// Node.
    /// </summary>
    public class Node : SelectableObject
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Parent graph.
        /// </summary>
        public Graph Parent = null;

        /// <summary>
        /// Position (2D or 3D point).
        /// </summary>
        private object Position = null;

        /// <summary>
        /// Draw properties.
        /// </summary>
        private NodeDrawProperties _DrawProperties = null;

        /// <summary>
        /// Draw properties access.
        /// </summary>
        public NodeDrawProperties DrawProperties
        {
            get
            {
                return (_DrawProperties != null)
                       ? _DrawProperties
                       : Parent.DrawProperties.DefaultNodeDrawProperties;
            }

            set
            {
                _DrawProperties = value;
            }
        }

        /// <summary>
        /// Check if parent draw properties used.
        /// </summary>
        public bool IsParentDrawProperties
        {
            get
            {
                return _DrawProperties == null;
            }
        }

        /// <summary>
        /// 2D point.
        /// </summary>
        public Point2D Point2D
        {
            get
            {
                if (Position is Point2D)
                {
                    return Position as Point2D;
                }
                else if (Position is Point3D)
                {
                    Point3D p3d = Position as Point3D;

                    return new Point2D(p3d.X, p3d.Y);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Position = value;
            }
        }

        /// <summary>
        /// 3D point.
        /// </summary>
        public Point3D Point3D
        {
            get
            {
                if (Position is Point3D)
                {
                    return Position as Point3D;
                }
                else if (Position is Point2D)
                {
                    Point2D p2d = Position as Point2D;

                    return new Point3D(p2d.X, p2d.Y, 0.0);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Position = value;
            }
        }

        /// <summary>
        /// Position as string.
        /// </summary>
        /// <returns>string</returns>
        public string PositionString()
        {
            return (Position is Point2D)
                   ? (Position as Point2D).ToString()
                   : (Position as Point3D).ToString();
        }

        /// <summary>
        /// Incident edges access.
        /// </summary>
        public List<Edge> Edges
        {
            get;
            private set;
        }

        /// <summary>
        /// Count of incident edges.
        /// </summary>
        public int Degree
        {
            get
            {
                return Edges.Count();
            }
        }

        /// <summary>
        /// Check if node is isolated.
        /// </summary>
        public bool IsIsolated
        {
            get
            {
                return Degree == 0;
            }
        }

        /// <summary>
        /// Check if node is hanging.
        /// </summary>
        public bool IsHanging
        {
            get
            {
                return Degree == 1;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="parent">parent graph</param>
        public Node(int id, Graph parent)
        {
            Id = id;
            Parent = parent;
            Edges = new List<Edge>();
        }

        /// <summary>
        /// Check if incident to edge.
        /// </summary>
        /// <param name="edge">edge</param>
        /// <returns>result</returns>
        public bool IsIncident(Edge edge)
        {
            return Edges.Any(e => e == edge);
        }

        /// <summary>
        /// Find in edges.
        /// </summary>
        public List<Edge> InEdges
        {
            get
            {
                return Edges.FindAll(e => !e.IsOriented || (e.B == this));
            }
        }

        /// <summary>
        /// Count of in edges.
        /// </summary>
        public int InDegree
        {
            get
            {
                return InEdges.Count();
            }
        }

        /// <summary>
        /// Find out edges.
        /// </summary>
        public List<Edge> OutEdges
        {
            get
            {
                return Edges.FindAll(e => !e.IsOriented || (e.A == this));
            }
        }

        /// <summary>
        /// Count of out edges.
        /// </summary>
        public int OutDegree
        {
            get
            {
                return OutEdges.Count();
            }
        }

        /// <summary>
        /// Cast to string.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return "Node";
        }
    }
}
