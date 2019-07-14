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

namespace RSGrandExchangeCompanion
{
    /// <summary>
    /// Logika interakcji dla klasy ItemView.xaml
    /// </summary>
    public partial class ItemView : Window
    {
        public ItemView(ItemModel item)
        {
            InitializeComponent();
            DataContext = new ItemViewModel(item);
        }


    }
}
