﻿using Microsoft.Win32;
using Stimulsoft.Report;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SuppliesForSquadReport.EntityModel;

namespace SuppliesForSquadReport
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StiReport myStiReport;
        private string fileName = Properties.Settings.Default.SuppliesReportFilePath;
        private bool choosingFile = Properties.Settings.Default.ChoosingReportFile;

        public MainWindow()
        {
            InitializeComponent();

            SquadsDataGrid.LoadingRow += SquadsDataGrid_LoadingRow;

            myStiReport = new StiReport();

            ExtractSquadsNumbers();
            
            LoadReport();
            //LoadData();
        
        }

        void SquadsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseDoubleClick += Row_MouseDoubleClick;
        }

        void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var row = (sender as DataGridRow);
                var item = (row.Item as kom);

                LoadData(item.N_KOM);
                if (myStiReport != null)
                {
                    myStiReport.Compile();
                    myStiReport.Render();
                    myStiReport.Show(true);
                }
            }
        }

        string OpenFile()
        {
            string fName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var b = openFileDialog.ShowDialog();
            if (b.HasValue && b.Value && !string.IsNullOrEmpty(openFileDialog.FileName))
            {
                fName = openFileDialog.FileName;
            }
            return fName;
        }

        void LoadReport()
        {
            if (choosingFile)
                fileName = OpenFile();
            myStiReport.Load(fileName);
        }

        void SaveReport()
        {
            if (myStiReport.IsModified && !string.IsNullOrEmpty(fileName))
            {
                myStiReport.Save(fileName);
            }

        }

        void LoadData(string comNum)
        {
            //string comNum = "88";
            kom Squad;
            List<PRIZ> Recruits;
            using (var ctx = new FBContext())
            {
                var squad = ctx.kom
                    .FirstOrDefault(k => k.N_KOM == comNum);
                var recruits = ctx.PRIZ
                    .Where(p => p.N_KOM == comNum)
                    .ToList();
                Squad = squad;
                Recruits = recruits;
            }

            //myStiReport.ReportDataSources.Clear();
            //myStiReport.ReportDataSources.Add("Squad", Squad);
            //myStiReport.ReportDataSources.Add("Recruits", Recruits);
            //myStiReport.RegReportDataSources();
            myStiReport.RegData("Squad", Squad);
            myStiReport.RegData("Recruits", Recruits);

        }
        #region for LoadData
        /* brr *
            //myStiReport.ReferencedAssemblies = new string[] {
            //                                                        "System.Dll",
            //                                                        "System.Drawing.Dll",
            //                                                        "System.Windows.Forms.Dll",
            //                                                        "System.Data.Dll",
            //                                                        "System.Xml.Dll",
            //                                                        "Stimulsoft.Controls.Dll",
            //                                                        "Stimulsoft.Base.Dll",
            //                                                        "Stimulsoft.Report.Dll",
            //@"D:\Documents\GitHub\SuppliesForSquadReport\SuppliesForSquadReport\bin\Debug\SuppliesForSquadReport.exe" };

            //myStiReport.RegData("Item", item);

            //myStiReport.Dictionary.Report.ReportDataSources.Add("Squad", item.Squad);
            //myStiReport.Dictionary.Report.ReportDataSources.Add("Recruits", item.Recruits);
            //myStiReport.Dictionary.Report.ReportDataSources.Add("Item", item);
            //myStiReport.RegReportDataSources();
            /* --- */
        #endregion for LoadData

        void ExtractSquadsNumbers()
        {
            //List<string> squadsNumbers;
            List<kom> squads;
            using (var ctx = new FBContext())
            {
                squads = ctx.kom
                    //.Where(k => k.FL_UB != 1 && k.SYST != 1)
                    .Where(k => k.SYST != 1)
                    .OrderBy(k => k.D_OTPR)
                    .ThenBy(k => k.N_KOM)
                    //.Select(k => k.N_KOM)
                    .ToList();
            }
            //SquadsList.ItemsSource = squadsNumbers;
            SquadsDataGrid.ItemsSource = squads;
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveReport();
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            //using (var ctx = new myEntities())
            //{
            //    var a = ctx.kom
            //        .FirstOrDefault(k => k.N_KOM == "88");

            //    PRIZs = ctx.PRIZ
            //        .ToList();
            //    koms = ctx.kom
            //        .ToList();
            //}

            //myStiReport.RegData("myData1", PRIZs);
            //myStiReport.RegData("myData2", koms);
        }

        private void ShowReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (myStiReport != null)
            {
                myStiReport.Show();
            }
        }

        private void DesignReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (myStiReport != null)
            {
                myStiReport.Design();
            }
        }

        private void SquadLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var lbl = (sender as Label);
                var num = (lbl.DataContext as string);

                LoadData(num);
                if (myStiReport != null)
                {
                    myStiReport.Compile();
                    myStiReport.Render();
                    myStiReport.Show(true);
                }
            }
        }
    }
}