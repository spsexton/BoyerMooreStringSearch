using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StringSearchLib;

namespace BoyerMooreStringSearch
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            TextReader tr = new StreamReader(@".\melville.txt");
            string sBuffer = tr.ReadToEnd();
            tr.Close();

            string sSearch1 = "I grapple";
            string sSearch2 = "I grapple with thee; from hell's heart I stab at thee";
            Stopwatch sw;

            //-------------------------------------------------------------
            //--- Test #1: Built-in .NET String.IndexOf
            //-------------------------------------------------------------
            sw = Stopwatch.StartNew();
            int nIndex1 = sBuffer.IndexOf(sSearch1);
            sw.Stop();
            MessageBox.Show(string.Format("Test 1 (IndexOf), shorter string, took {0} milliseconds. Index={1}", sw.Elapsed.TotalMilliseconds.ToString(), nIndex1));

            sw = Stopwatch.StartNew();
            nIndex1 = sBuffer.IndexOf(sSearch2);
            sw.Stop();
            MessageBox.Show(string.Format("Test 1 (IndexOf), longer string, took {0} milliseconds.  Index={1}", sw.Elapsed.TotalMilliseconds.ToString(), nIndex1));

            //-------------------------------------------------------------
            //--- Test #2: Simple brute force search
            //-------------------------------------------------------------
            sw = Stopwatch.StartNew();
            int nIndex2 = sBuffer.IndexOfBruteForce(sSearch1);
            sw.Stop();
            MessageBox.Show(string.Format("Test 2 (IndexOfBruteForce), shorter string, took {0} milliseconds.  Index={1}", sw.Elapsed.TotalMilliseconds.ToString(), nIndex2));

            sw = Stopwatch.StartNew();
            nIndex2 = sBuffer.IndexOfBruteForce(sSearch2);
            sw.Stop();
            MessageBox.Show(string.Format("Test 2 (IndexOfBruteForce), longer string, took {0} milliseconds.  Index={1}", sw.Elapsed.TotalMilliseconds.ToString(), nIndex2));

            //-------------------------------------------------------------
            //--- Test #3: Boyer Moore, part 1 -- using single jump table
            //-------------------------------------------------------------
            sw = Stopwatch.StartNew();
            int nIndex3 = sBuffer.IndexOfBM1(sSearch1);
            sw.Stop();
            MessageBox.Show(string.Format("Test 3 (IndexOfBM1), shorter string, took {0} milliseconds.  Index={1}", sw.Elapsed.TotalMilliseconds.ToString(), nIndex3));

            sw = Stopwatch.StartNew();
            nIndex3 = sBuffer.IndexOfBM1(sSearch2);
            sw.Stop();
            MessageBox.Show(string.Format("Test 3 (IndexOfBM1), longer string, took {0} milliseconds.  Index={1}", sw.Elapsed.TotalMilliseconds.ToString(), nIndex3));
        }
    }
}
