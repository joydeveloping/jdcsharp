﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

using Lib.DataFormats;
using Lib.Maths.Geometry;
using Lib.Utils;

namespace Lib.MathMod.Grid.Load
{
    /// <summary>
    /// Grid loader from PFG/IBC format.
    /// </summary>
    public class GridLoaderSaverPFG
    {
        /// <summary>
        /// Conversion.
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="p">separator</param>
        /// <returns>converted string</returns>
        static string Conv(string s, string p)
        {
            if (s.Contains(".") && (p != "."))
            {
                return s.Replace('.', ',');
            }

            if (s.Contains(",") && (p != ","))
            {
                return s.Replace(',', '.');
            }

            return s;
        }

        /// <summary>
        /// Load blocks from PFG file.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sr">stream reader</param>
        /// <param name="is_blank">isblank feature</param>
        private static void LoadBlocks(StructuredGrid g, StreamReader sr, bool is_blank)
        {
            string line;

            // Get separator for read real numbers.
            string sep = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            if ((line = sr.ReadLine()) != null)
            {
                int bc = Int32.Parse(line);

                // Allocate memory for blocks sizes.
                int[] ii = new int[bc];
                int[] jj = new int[bc];
                int[] kk = new int[bc];

                // Read blocks sizes.
                PFG.ReadBlocksSizes(sr, bc, ii, jj, kk);

                // Add all blocks and allocate memory for them
                for (int i = 0; i < bc; i++)
                {
                    // We read nodes count, but have to pass to block constructor cells count.
                    Block b = new Block(g, i, ii[i] - 1, jj[i] - 1, kk[i] - 1);

                    b.Allocate();
                    g.Blocks.Add(b);
                }

                // Read coordsinates.

                int cur_block_num = 0;
                Block cur_block = g.Blocks[cur_block_num];
                int cur_coord = 0;
                int cur_i = 0;
                int cur_j = 0;
                int cur_k = 0;
                int iblank_data_left = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] s = line.Split(' ');

                    for (int i = 0; i < s.Count(); i++)
                    {
                        if (s[i] == "")
                        {
                            // If element if empty this is the end of the line.
                            break;
                        }

                        if (iblank_data_left > 0)
                        {
                            // If we use iblank data, we must read it out.

                            iblank_data_left--;

                            continue;
                        }

                        // Load value.
                        double val = Double.Parse(Conv(s[i], sep));
                        cur_block.Nodes[cur_i, cur_j, cur_k][cur_coord] = val;

                        cur_i++;

                        // Shift cur_*.
                        if (cur_i == cur_block.INodes)
                        {
                            cur_i = 0;
                            cur_j++;

                            if (cur_j == cur_block.JNodes)
                            {
                                cur_j = 0;
                                cur_k++;

                                if (cur_k == cur_block.KNodes)
                                {
                                    cur_k = 0;
                                    cur_coord++;

                                    if (cur_coord == 3)
                                    {
                                        cur_coord = 0;
                                        cur_block_num++;

                                        if (cur_block_num == bc)
                                        {
                                            // All blocks data is readed.
                                            return;
                                        }
                                        else
                                        {
                                            if (is_blank)
                                            {
                                                iblank_data_left = cur_block.NodesCount;
                                            }
                                        }

                                        cur_block = g.Blocks[cur_block_num];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load interfaces, border conditions and scopes.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sr">stream</param>
        private static void LoadIfacesBCondsScopes(StructuredGrid g, StreamReader sr)
        {
            // Ignore two lines.
            for (int i = 0; i < 2; i++)
            {
                string line = sr.ReadLine();

                if (line == null)
                {
                    return;
                }
            }

            LoadIfaces(g, sr);
            LoadBConds(g, sr);
            LoadScopes(g, sr);
        }

        /// <summary>
        /// Load interfaces.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sr">stream reader</param>
        private static void LoadIfaces(StructuredGrid g, StreamReader sr)
        {
            string line;

            if ((line = sr.ReadLine()) != null)
            {
                int ic = Int32.Parse(line);

                // Read all interfaces.
                for (int i = 0; i < ic; i++)
                {
                    line = sr.ReadLine();
                    string[] s = line.Split(' ');
                    int id = Int32.Parse(s[0]);
                    int bi = Int32.Parse(s[1]);
                    int i0 = Int32.Parse(s[2]);
                    int i1 = Int32.Parse(s[3]);
                    int j0 = Int32.Parse(s[4]);
                    int j1 = Int32.Parse(s[5]);
                    int k0 = Int32.Parse(s[6]);
                    int k1 = Int32.Parse(s[7]);
                    int nbi = Int32.Parse(s[8]);

                    // Create interface.
                    Iface iface = new Iface(id, g.Blocks[bi - 1],
                                            new ISegm(i0 - 1, i1 - 1),
                                            new ISegm(j0 - 1, j1 - 1),
                                            new ISegm(k0 - 1, k1 - 1),
                                            g.Blocks[nbi - 1]);

                    // Paste into interfaces list.

                    for (int j = 0; j < g.IfacesCount; j++)
                    {
                        Iface cur_iface = g.Ifaces[j];

                        if (cur_iface.Id == iface.Id)
                        {
                            g.Ifaces.Insert(j + 1, iface);
                            iface = null;

                            break;
                        }
                    }

                    if (iface != null)
                    {
                        g.Ifaces.Add(iface);
                    }
                }
            }
        }

        /// <summary>
        /// Load border conditions.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sr">stream reader</param>
        private static void LoadBConds(StructuredGrid g, StreamReader sr)
        {
            string line;

            if ((line = sr.ReadLine()) != null)
            {
                int bcc = Int32.Parse(line);

                // Read all interfaces.
                for (int i = 0; i < bcc; i++)
                {
                    line = sr.ReadLine();
                    string[] s = line.Split(' ');
                    int id = Int32.Parse(s[0]);
                    int bi = Int32.Parse(s[1]);
                    int i0 = Int32.Parse(s[2]);
                    int i1 = Int32.Parse(s[3]);
                    int j0 = Int32.Parse(s[4]);
                    int j1 = Int32.Parse(s[5]);
                    int k0 = Int32.Parse(s[6]);
                    int k1 = Int32.Parse(s[7]);
                    string type = s[8];
                    string subtype = s[9];
                    string name = s[10];

                    // Create border condition.
                    BCond bcond = new BCond(id, g.Blocks[bi - 1],
                                            new ISegm(i0 - 1, i1 - 1),
                                            new ISegm(j0 - 1, j1 - 1),
                                            new ISegm(k0 - 1, k1 - 1),
                                            type, subtype, name);
                    g.BConds.Add(bcond);
                }
            }
        }

        /// <summary>
        /// Load scopes.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sr">stream reader</param>
        private static void LoadScopes(StructuredGrid g, StreamReader sr)
        {
            string line;

            if ((line = sr.ReadLine()) != null)
            {
                int sc = Int32.Parse(line);

                // Read all interfaces.
                for (int i = 0; i < sc; i++)
                {
                    line = sr.ReadLine();
                    string[] s = line.Split(' ');
                    int id = Int32.Parse(s[0]);
                    int bi = Int32.Parse(s[1]);
                    int i0 = Int32.Parse(s[2]);
                    int i1 = Int32.Parse(s[3]);
                    int j0 = Int32.Parse(s[4]);
                    int j1 = Int32.Parse(s[5]);
                    int k0 = Int32.Parse(s[6]);
                    int k1 = Int32.Parse(s[7]);
                    string type = s[8];
                    string subtype = s[9];
                    string name = s[10];

                    // Create border condition.
                    Scope scope = new Scope(id, g.Blocks[bi - 1],
                                            new ISegm(i0 - 1, i1 - 1),
                                            new ISegm(j0 - 1, j1 - 1),
                                            new ISegm(k0 - 1, k1 - 1),
                                            type, subtype, name);
                    g.Scopes.Add(scope);
                }
            }
        }

        /// <summary>
        /// Load structured grid from PFG/IBC files.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="pfg_file_name">PFG file name</param>
        /// <param name="ibc_file_name">IBC file name</param>
        /// <param name="is_iblank">isblank feature</param>
        /// <returns><c>true</c> - if grid is loaded, <c>false</c> - otherwise</returns>
        public static bool Load(StructuredGrid g,
                                string pfg_file_name, string ibc_file_name,
                                bool is_iblank)
        {
            bool is_succ = true;

            try
            {
                using (StreamReader pfg_sr = new StreamReader(pfg_file_name))
                {
                    using (StreamReader ibc_sr = new StreamReader(ibc_file_name))
                    {
                        g.Clear();
                        LoadBlocks(g, pfg_sr, is_iblank);
                        LoadIfacesBCondsScopes(g, ibc_sr);
                        g.SetIfacesNDirs();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(ExeDebug.ReportError(e.Message));
                is_succ = false;
            }

            return is_succ;
        }

        /// <summary>
        /// Save grid to PFG/IBC files.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="pfg_file_name">PFG file name</param>
        /// <param name="ibc_file_name">IBC file name</param>
        /// <param name="is_blank">iblank flag</param>
        /// <returns><c>true</c> - if grid is saved, <c>false</c> - otherwise</returns>
        public static bool Save(StructuredGrid g, 
                                string pfg_file_name, string ibc_file_name,
                                bool is_blank)
        {
            bool is_succ = true;

            try
            {
                using (StreamWriter pfg_sw = new StreamWriter(pfg_file_name))
                {
                    using (StreamWriter ibc_sw = new StreamWriter(ibc_file_name))
                    {
                        SaveBlocks(g, pfg_sw, is_blank);
                        SaveIfacesBCondsScopes(g, ibc_sw);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(ExeDebug.ReportError(e.Message));
                is_succ = false;
            }

            return is_succ;
        }

        /// <summary>
        /// Save blocks.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sw">stream</param>
        /// <param name="is_blank">iblank data flag</param>
        public static void SaveBlocks(StructuredGrid g, StreamWriter sw, bool is_blank)
        {
            int bc = g.BlocksCount;
            const int max_items_count = 5;
            const int max_iblank_items_count = 38;

            // Write blocks count.
            sw.WriteLine(bc.ToString());

            // Write blocks sizes.
            for (int bi = 0; bi < bc; bi++)
            {
                Block b = g.Blocks[bi];

                sw.WriteLine(b.INodes.ToString() + " " + b.JNodes.ToString() + " " + b.KNodes.ToString());
            }

            // Write each block.
            for (int bi = 0; bi < bc; bi++)
            {
                string line;
                int items_count;

                Block b = g.Blocks[bi];

                // Write X, Y, Z coordinates.
                for (int coord_num = 0; coord_num < 3; coord_num++)
                {
                    line = "";
                    items_count = 0;

                    for (int k = 0; k < b.KNodes; k++)
                    {
                        for (int j = 0; j < b.JNodes; j++)
                        {
                            for (int i = 0; i < b.INodes; i++)
                            {
                                if (items_count > 0)
                                {
                                    line += " ";
                                }

                                line += String.Format("{0:0.00000000e+00}", b.Nodes[i, j, k][coord_num]);
                                items_count++;

                                if (items_count == max_items_count)
                                {
                                    sw.WriteLine(line);
                                    line = "";
                                    items_count = 0;
                                }
                            }
                        }
                    }

                    if (items_count > 0)
                    {
                        sw.WriteLine(line);
                    }
                }

                // Write blank data to the end of file.
                if (is_blank)
                {
                    line = "";
                    items_count = 0;

                    for (int i = 0; i < b.NodesCount; i++)
                    {
                        if (items_count > 0)
                        {
                            line += " ";
                        }

                        line += "1";
                        items_count++;

                        if (items_count == max_iblank_items_count)
                        {
                            sw.WriteLine(line);
                            line = "";
                            items_count = 0;
                        }
                    }

                    if (items_count > 0)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }

        /// <summary>
        /// Save interfaces, border conditions, scopes.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sw">stream</param>
        public static void SaveIfacesBCondsScopes(StructuredGrid g, StreamWriter sw)
        {
            sw.WriteLine("! Version 3");
            sw.WriteLine("! DO NOT MODIFY THIS FILE.  RESULTS LIKELY TO BE UNDESIRABLE");
            SaveIfaces(g, sw);
            SaveBConds(g, sw);
            SaveScopes(g, sw);
        }

        /// <summary>
        /// Save interfaces.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sw">stream</param>
        public static void SaveIfaces(StructuredGrid g, StreamWriter sw)
        {
            sw.WriteLine(g.IfacesCount.ToString());

            for (int i = 0; i < g.IfacesCount; i++)
            {
                Iface iface = g.Ifaces[i];

                sw.WriteLine(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}",
                                           iface.Id, iface.B.Id + 1,
                                           iface.I0 + 1, iface.I1 + 1,
                                           iface.J0 + 1, iface.J1 + 1,
                                           iface.K0 + 1, iface.K1 + 1,
                                           iface.NB.Id + 1));
            }
        }

        /// <summary>
        /// Save border conditions.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sw">stream</param>
        public static void SaveBConds(StructuredGrid g, StreamWriter sw)
        {
            sw.WriteLine(g.BCondsCount.ToString());

            for (int i = 0; i < g.BCondsCount; i++)
            {
                BCond b = g.BConds[i];

                sw.WriteLine(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}",
                                           b.Id, b.B.Id + 1,
                                           b.I0 + 1, b.I1 + 1,
                                           b.J0 + 1, b.J1 + 1,
                                           b.K0 + 1, b.K1 + 1,
                                           b.Label.Type, b.Label.Subtype, b.Label.Name));
            }
        }

        /// <summary>
        /// Save scopes.
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="sw">stream</param>
        public static void SaveScopes(StructuredGrid g, StreamWriter sw)
        {
            sw.WriteLine(g.ScopesCount.ToString());

            for (int i = 0; i < g.ScopesCount; i++)
            {
                Scope s = g.Scopes[i];

                sw.WriteLine(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}",
                                           s.Id, s.B.Id + 1,
                                           s.I0 + 1, s.I1 + 1,
                                           s.J0 + 1, s.J1 + 1,
                                           s.K0 + 1, s.K1 + 1,
                                           s.Label.Type, s.Label.Subtype, s.Label.Name));
            }
        }
    }
}