using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RSGrandExchangeCompanion
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Input { get; set; }
        public static ItemView NewWindow { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public ItemModel SelectedItem { get; set; }
        public ObservableCollection<ItemModel> ItemList { get; set; }
        public ObservableCollection<ItemModel> Items { get; set; }
        public ICommand ShowItemInfo { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand NextPage { get; set; }
        public ICommand PrevPage { get; set; }

        public MainWindowViewModel()
        {
            NumberOfPages = 1;
            CurrentPage = 1;
            ItemList = new ObservableCollection<ItemModel>();
            Items = new ObservableCollection<ItemModel>();
            SearchCommand = new RelayCommand(ClearData);
            NextPage = new RelayCommand(NextPageView);
            PrevPage = new RelayCommand(PrevPageView);
            ShowItemInfo = new RelayCommand(ShowItemWindow);

        }

        private void ShowItemWindow()
        {
            if (SelectedItem == null) return;
            NewWindow = new ItemView(SelectedItem);
            NewWindow.Show();
        }
        private void NextPageView()
        {
            if (CurrentPage >= NumberOfPages) return;
            CurrentPage++;
            ShowPage();
        }

        private void PrevPageView()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            ShowPage();
        }

        private void ShowPage()
        {
            Items.Clear();
            foreach (var item in ItemList.Skip((CurrentPage - 1) * 10).Take(10))
            {
                Items.Add(item);
            }
        }
        private async void ClearData()
        {
            CurrentPage = 1;
            ItemList.Clear();
            await DownloadItems();
        }
        private async Task DownloadItems()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://services.runescape.com");
                    for (int i = 1; i < 38; i++)
                    {

                        var recivedObject = new RootObject();
                        var response = await client.GetAsync($"/m=itemdb_rs/api/catalogue/items.json?category={i.ToString()}&alpha={Input}");
                        string result = await response.Content.ReadAsStringAsync();
                        if (result == String.Empty) continue;
                        JsonConvert.PopulateObject(result, recivedObject);
                        foreach (var item in recivedObject.Items)
                        {
                            ItemList.Add(new ItemModel()
                            {
                                ItemID = item.id,
                                ItemUrl = item.icon,
                                ItemName = item.name,
                                ItemMembers = item.members,
                                ItemPrice = item.current.price,
                                ItemChange = item.today.price
                            }); ;

                        }
                        NumberOfPages = (ItemList.Count() - 1) / 10 + 1;

                        if (Items.Count() <= 10) ShowPage();
                    }

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
