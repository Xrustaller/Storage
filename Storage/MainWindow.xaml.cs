using Storage.Database;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Storage
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly StorageContext _db;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            System.Data.Entity.Database.SetInitializer(new DataBaseInitializer());
            try
            {
                _db = new StorageContext("StorageContext");
            }
            catch (Exception)
            {
                MessageBox.Show("DataBase Init Error");
                Close();
            }
        }
        private void Refresh()
        {
            try
            {
                double totalPriceSt = 0, totalPriceSo = 0;
                _db.SaveChanges();
                _db.Accepted.Load();
                _db.Storage.Load();
                _db.SoldOut.Load();
                AcceptedDataGrid.ItemsSource = null;
                AcceptedDataGrid.ItemsSource = _db.Accepted.Local.ToList();
                StorageDataGrid.ItemsSource = null;
                StorageDataGrid.ItemsSource = _db.Storage.Local.ToList();
                totalPriceSt += _db.Storage.Local.Sum(st => st.Price * st.Count);
                TotalPriceStopage.Content = "Total price: " + totalPriceSt;
                SoldOutDataGrid.ItemsSource = null;
                SoldOutDataGrid.ItemsSource = _db.SoldOut.Local.ToList();
                totalPriceSo += _db.SoldOut.Local.Sum(so => so.Price * so.Count);
                TotalPriceSoldOut.Content = "Total price: " + totalPriceSo;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Refresh DB\n" + e);
                Close();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Length <= 4)
            {
                MessageBox.Show("The name must be more than 4 characters");
                return;
            }
            try
            {
                _db.Accepted.Add(new Accepted { NameItem = NameTextBox.Text, Count = Convert.ToInt32(CountTextBox.Text), Price = Convert.ToInt32(PriceTextBox.Text), DateCreate = DateTime.Now });
                _db.Storage.Add(new Models.Storage { NameItem = NameTextBox.Text, Count = Convert.ToInt32(CountTextBox.Text), Price = Convert.ToInt32(PriceTextBox.Text), DateCreate = DateTime.Now });
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
            NameTextBox.Text = "";
            CountTextBox.Text = "0";
            PriceTextBox.Text = "0";
            Refresh();
        }
        private void DeleteMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Accepted selectedItem = (Accepted)AcceptedDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Line not selected");
                return;
            }
            try
            {
                if (_db.Storage.FirstOrDefault(o => o.NameItem == selectedItem.NameItem) != null)
                {
                    Accepted acceptedObj = _db.Accepted.FirstOrDefault(o => o.NameItem == selectedItem.NameItem);
                    Models.Storage storageObj = _db.Storage.FirstOrDefault(o => o.NameItem == selectedItem.NameItem);
                    if (acceptedObj == null || storageObj == null) throw new ArgumentNullException();
                    _db.Accepted.Remove(acceptedObj);
                    _db.Storage.Remove(storageObj);
                    Refresh();
                }
                else
                {
                    MessageBox.Show("Already sold. Unable to delete");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        private void SaleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Models.Storage selectedItem = (Models.Storage)StorageDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Line not selected");
                return;
            }
            try
            {
                Models.Storage storageObj = _db.Storage.FirstOrDefault(o => o.NameItem == selectedItem.NameItem);
                if (storageObj == null) throw new ArgumentNullException();
                _db.SoldOut.Add(new Models.SoldOut { NameItem = storageObj.NameItem, Count = storageObj.Count, Price = storageObj.Price, DateCreate = DateTime.Now });
                _db.Storage.Remove(storageObj);
                Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        private void ReturnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SoldOut selectedItem = (SoldOut)SoldOutDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Line not selected");
                return;
            }
            try
            {
                SoldOut soldOutObj = _db.SoldOut.FirstOrDefault(o => o.NameItem == selectedItem.NameItem);
                if (soldOutObj == null) throw new ArgumentNullException();
                _db.Storage.Add(new Models.Storage { NameItem = soldOutObj.NameItem, Count = soldOutObj.Count, Price = soldOutObj.Price, DateCreate = DateTime.Now });
                _db.SoldOut.Remove(soldOutObj);
                Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var reportList = new List<ReportItem>();
            var fromDate = FromDatePicker.SelectedDate;
            var toDate = ToDatePicker.SelectedDate;
            if (fromDate == null && toDate == null)
            {
                fromDate = DateTime.MinValue;
                toDate = DateTime.MaxValue;
            }
            else if (fromDate != null && toDate == null)
            {
                toDate = DateTime.MaxValue;
            }
            else if (fromDate == null)
            {
                fromDate = DateTime.MinValue;
                toDate = toDate?.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                toDate = toDate?.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            MessageBox.Show(fromDate + "\n" + toDate);
            if (fromDate > toDate)
            {
                MessageBox.Show("\"From\" must be less than \"To\"");
                return;
            }
            ReportDataGrid.ItemsSource = null;
            double totalPrice = 0;
            void AddReportList(string status)
            {
                switch (status)
                {
                    case "Accepted":
                        {
                            foreach (var accepted in _db.Accepted)
                            {
                                if (fromDate > accepted.DateCreate || accepted.DateCreate > toDate) continue;
                                reportList.Add(new ReportItem()
                                {
                                    Status = status,
                                    NameItem = accepted.NameItem,
                                    Count = accepted.Count,
                                    Price = accepted.Price,
                                    DateCreate = accepted.DateCreate
                                });
                                totalPrice += accepted.Price * accepted.Count;
                            }
                            break;
                        }
                    case "Storage":
                        {
                            foreach (var storage in _db.Storage)
                            {
                                if (fromDate > storage.DateCreate || storage.DateCreate > toDate) continue;
                                reportList.Add(new ReportItem()
                                {
                                    Status = status,
                                    NameItem = storage.NameItem,
                                    Count = storage.Count,
                                    Price = storage.Price,
                                    DateCreate = storage.DateCreate
                                });
                                totalPrice += storage.Price * storage.Count;
                            }
                            break;
                        }
                    case "SoldOut":
                        {
                            foreach (var soldOut in _db.SoldOut)
                            {
                                if (fromDate > soldOut.DateCreate || soldOut.DateCreate > toDate) continue;
                                reportList.Add(new ReportItem()
                                {
                                    Status = status,
                                    NameItem = soldOut.NameItem,
                                    Count = soldOut.Count,
                                    Price = soldOut.Price,
                                    DateCreate = soldOut.DateCreate
                                });
                                totalPrice += soldOut.Price * soldOut.Count;
                            }
                            break;
                        }
                }
            }
            switch (ReportComboBox.SelectedIndex)
            {
                case 0: // All
                    {
                        AddReportList("Storage");
                        AddReportList("SoldOut");
                        ReportDataGrid.ItemsSource = reportList;
                        TotalPriceReport.Content = "Total price: " + totalPrice;
                        break;
                    }
                case 1: // Accepted
                    {
                        AddReportList("Accepted");
                        ReportDataGrid.ItemsSource = reportList;
                        TotalPriceReport.Content = "Total price: " + totalPrice;
                        break;
                    }
                case 2: // Storage
                    {
                        AddReportList("Storage");
                        ReportDataGrid.ItemsSource = reportList;
                        TotalPriceReport.Content = "Total price: " + totalPrice;
                        break;
                    }
                case 3: // SoldOut
                    {
                        AddReportList("SoldOut");
                        ReportDataGrid.ItemsSource = reportList;
                        TotalPriceReport.Content = "Total price: " + totalPrice;
                        break;
                    }
            }
        }
    }
}
