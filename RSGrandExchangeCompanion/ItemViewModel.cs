using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RSGrandExchangeCompanion
{
    [AddINotifyPropertyChangedInterface]
    public class ItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ItemDetails { get; set; }
        public string Data { get; set; }
        public IList<DataPoint> Series { get; set; }
        public ItemModel Item { get; set; }
        public GraphModel RecivedGraph { get; set; }

        public ItemViewModel(ItemModel item)
        {
            Item = item;
            ItemDetails = item.ItemName;
            RecivedGraph = new GraphModel();
            Series = new List<DataPoint>();
            Task.Run(() => DownloadItems());
        }

        private async Task DownloadItems()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://services.runescape.com");

                        var response = await client.GetAsync($"/m=itemdb_rs/api/graph/{Item.ItemID}.json");
                        string result = await response.Content.ReadAsStringAsync();
                        JsonConvert.PopulateObject(result, RecivedGraph);
                    int days = -180;
                    var data = new List<DataPoint>();
                    foreach (var item in RecivedGraph.Average.Values)
                    {
                        data.Add(new DataPoint(days, item));
                        days++;
                    }
                    Series = data;
                }
            }
            
            catch (HttpRequestException ex)
            {
                var error = $"Http request error\n{ex.Message}\n";
                MessageBox.Show(error);
                throw;
            }
            catch (Exception ex)
            {
                var error = $"Cannot download items\n{ex.Message}\n";
                MessageBox.Show(error);
                throw;
            }

        }
    }
}