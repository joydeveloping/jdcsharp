﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;

using Lib.Draw;
using Lib.DataStruct.Graph;
using Lib.DataStruct.Graph.DrawProperties;

using SWMColor = System.Windows.Media.Color;
using SWMBrushes = System.Windows.Media.Brushes;

namespace GraphMaster.Windows
{
    /// <summary>
    /// Interaction logic for EditNodesEdgesWindow.xaml
    /// </summary>
    public partial class EditNodesEdgesWindow : Window
    {
        /// <summary>
        /// Node for edit single node.
        /// </summary>
        public Node Node = null;

        /// <summary>
        /// Edge for edit single edge.
        /// </summary>
        public Edge Edge = null;

        /// <summary>
        /// Nodes list for edit many nodes.
        /// </summary>
        public List<Node> Nodes = null;

        /// <summary>
        /// Edges list for edit many edges.
        /// </summary>
        public List<Edge> Edges = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EditNodesEdgesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Switch on node draw properties.
        /// </summary>
        /// <param name="dp">draw properties</param>
        private void NodeDrawPropertiesOn(NodeDrawProperties dp)
        {
            NodeHasDrawPropertiesCB.IsChecked = true;
            NodeInnerRadiusTB.IsEnabled = true;
            NodeBorderRadiusTB.IsEnabled = true;
            NodeColorTB.IsEnabled = true;
            NodeBorderColorTB.IsEnabled = true;
            NodeInnerRadiusTB.Text = dp.InnerRadius.ToString();
            NodeBorderRadiusTB.Text = dp.BorderRadius.ToString();
            NodeColorTB.Background = new SolidColorBrush(dp.Color.ToSWMColor());
            NodeBorderColorTB.Background = new SolidColorBrush(dp.BorderColor.ToSWMColor());
        }

        /// <summary>
        /// Switch off node draw properties.
        /// </summary>
        private void NodeDrawPropertiesOff()
        {
            NodeHasDrawPropertiesCB.IsChecked = false;
            NodeInnerRadiusTB.IsEnabled = false;
            NodeBorderRadiusTB.IsEnabled = false;
            NodeColorTB.IsEnabled = false;
            NodeBorderColorTB.IsEnabled = false;
            NodeInnerRadiusTB.Text = "";
            NodeBorderRadiusTB.Text = "";
            NodeColorTB.Background = SWMBrushes.Black;
            NodeBorderColorTB.Background = SWMBrushes.Black;
        }

        /// <summary>
        /// Set node.
        /// </summary>
        private void SetNode()
        {
            NodeLabelTB.Text = Node.Label;

            if (Node.DrawProperties != null)
            {
                NodeDrawPropertiesOn(Node.DrawProperties);
            }
            else
            {
                NodeDrawPropertiesOff();
            }
        }

        /// <summary>
        /// Switch on edge draw properties.
        /// </summary>
        /// <param name="dp">draw properties</param>
        private void EdgeDrawPropertiesOn(EdgeDrawProperties dp)
        {
            EdgeHasDrawPropertiesCB.IsChecked = true;
            EdgeColorTB.IsEnabled = true;
            EdgeThicknessTB.IsEnabled = true;
            EdgeNodesMarginTB.IsEnabled = true;
            EdgeColorTB.Background = new SolidColorBrush(dp.Color.ToSWMColor());
            EdgeThicknessTB.Text = dp.Thickness.ToString();
            EdgeNodesMarginTB.Text = dp.NodesMargin.ToString();
        }

        /// <summary>
        /// Switch off edge draw properties.
        /// </summary>
        private void EdgeDrawPropertiesOff()
        {
            EdgeHasDrawPropertiesCB.IsChecked = false;
            EdgeColorTB.IsEnabled = false;
            EdgeThicknessTB.IsEnabled = false;
            EdgeNodesMarginTB.IsEnabled = false;
            EdgeColorTB.Background = SWMBrushes.Black;
            EdgeThicknessTB.Text = "";
            EdgeNodesMarginTB.Text = "";
        }

        /// <summary>
        /// Set edge.
        /// </summary>
        private void SetEdge()
        {
            EdgeLabelTB.Text = Edge.Label;

            if (Edge.DrawProperties != null)
            {
                EdgeDrawPropertiesOn(Edge.DrawProperties);
            }
            else
            {
                EdgeDrawPropertiesOff();
            }
        }

        /// <summary>
        /// Window loaded.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Node != null)
            {
                // Single node.
                NodeNodesGB.Header = "Node";
                SetNode();
            }
            else if (Nodes != null)
            {
                // Many nodes.
                NodeNodesGB.Header = "Nodes";
                NodeLabelTB.IsEnabled = false;
                NodeLabelTB.Text = "inaccessible";
                NodeDrawPropertiesOff();
            }
            else
            {
                // No nodes.
                NodeNodesGB.Visibility = Visibility.Hidden;
            }

            if (Edge != null)
            {
                // Single edge.
                EdgeEdgesGB.Header = "Edge";
                SetEdge();
            }
            else if (Edges != null)
            {
                // Many edges.
                EdgeEdgesGB.Header = "Edges";
                EdgeLabelTB.IsEnabled = false;
                EdgeLabelTB.Text = "inaccessible";
                EdgeDrawPropertiesOff();
            }
            else
            {
                // No edges.
                EdgeEdgesGB.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Checkbox for node draw properties.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void NodeHasDrawPropertiesCB_Checked(object sender, RoutedEventArgs e)
        {
            NodeDrawPropertiesOn(new NodeDrawProperties());
        }

        /// <summary>
        /// Uncheck node draw properties.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void NodeHasDrawPropertiesCB_Unchecked(object sender, RoutedEventArgs e)
        {
            NodeDrawPropertiesOff();
        }

        /// <summary>
        /// Check edge draw properties enabled.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void EdgeHasDrawPropertiesCB_Checked(object sender, RoutedEventArgs e)
        {
            EdgeDrawPropertiesOn(new EdgeDrawProperties());
        }

        /// <summary>
        /// Uncheck edge draw properties enabled.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void EdgeHasDrawPropertiesCB_Unchecked(object sender, RoutedEventArgs e)
        {
            EdgeDrawPropertiesOff();
        }

        /// <summary>
        /// Accept button click.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void AcceptB_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Cancel button click.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void CancelB_Click(object sender, RoutedEventArgs e)
        {
            // Do nothing.
            Close();
        }

        /// <summary>
        /// Get color.
        /// </summary>
        /// <param name="c">initial color</param>
        /// <returns>color</returns>
        private SWMColor GetColor(SWMColor c)
        {
            ColorDialog d = new ColorDialog();

            d.Color = new Lib.Draw.Color(c).ToSDColor();

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return new Lib.Draw.Color(d.Color).ToSWMColor();
            }

            return c;
        }

        /// <summary>
        /// Node color change click.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void NodeColorTB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SWMColor c = (NodeColorTB.Background as SolidColorBrush).Color;
            NodeColorTB.Background = new SolidColorBrush(GetColor(c));
        }

        /// <summary>
        /// Node border color change click.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void NodeBorderColorTB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SWMColor c = (NodeBorderColorTB.Background as SolidColorBrush).Color;
            NodeBorderColorTB.Background = new SolidColorBrush(GetColor(c));
        }

        /// <summary>
        /// Edge color change click.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">parameters</param>
        private void EdgeColorTB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SWMColor c = (EdgeColorTB.Background as SolidColorBrush).Color;
            EdgeColorTB.Background = new SolidColorBrush(GetColor(c));
        }
    }
}
