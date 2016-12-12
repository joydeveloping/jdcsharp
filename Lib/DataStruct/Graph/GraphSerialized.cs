﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Lib.DataStruct.Graph
{
    /// <summary>
    /// Graph information for more flexible serialization.
    /// </summary>
    [XmlType("Graph")]
    public class GraphSerialized
    {
        /// <summary>
        /// Dimensionality.
        /// </summary>
        [XmlAttribute]
        public GraphDimensionality Dimensionality;

        /// <summary>
        /// Draw properties of graph.
        /// </summary>
        public class DrawPropertiesStrings
        {
            /// <summary>
            /// Default node draw properties.
            /// </summary>
            [XmlAttribute]
            public string DefaultNodeDrawProperties;

            /// <summary>
            /// Default selected node draw properties.
            /// </summary>
            [XmlAttribute]
            public string DefaultSelectedNodeDrawProperties;

            /// <summary>
            /// Default captured node draw properties.
            /// </summary>
            [XmlAttribute]
            public string DefaultCapturedNodeDrawProperties;

            /// <summary>
            /// Default edge draw properties.
            /// </summary>
            [XmlAttribute]
            public string DefaultEdgeDrawProperties;

            /// <summary>
            /// Default selected edge draw properties.
            /// </summary>
            [XmlAttribute]
            public string DefaultSelectedEdgeDrawProperties;
        };

        /// <summary>
        /// Draw properties.
        /// </summary>
        public DrawPropertiesStrings DrawProperties;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GraphSerialized()
        {
        }

        /// <summary>
        /// Constructor from graph.
        /// </summary>
        /// <param name="g">graph</param>
        public GraphSerialized(Graph g)
        {
            Dimensionality = g.Dimensionality;

            DrawProperties = new DrawPropertiesStrings();
            DrawProperties.DefaultNodeDrawProperties = g.DrawProperties.DefaultNodeDrawProperties.ToString();
            DrawProperties.DefaultSelectedNodeDrawProperties = g.DrawProperties.DefaultSelectedNodeDrawProperties.ToString();
            DrawProperties.DefaultCapturedNodeDrawProperties = g.DrawProperties.DefaultCapturedNodeDrawProperties.ToString();
            DrawProperties.DefaultEdgeDrawProperties = g.DrawProperties.DefaultEdgeDrawProperties.ToString();
            DrawProperties.DefaultSelectedEdgeDrawProperties = g.DrawProperties.DefaultSelectedEdgeDrawProperties.ToString();
        }

        /// <summary>
        /// Serialize graph.
        /// </summary>
        /// <param name="file_name">file</param>
        public void XmlSerialize(string file_name)
        {
            XmlSerializer serializer = new XmlSerializer(GetType());
            TextWriter writer = new StreamWriter(file_name);

            serializer.Serialize(writer, this);
            writer.Flush();
            writer.Close();
        }
    }
}
