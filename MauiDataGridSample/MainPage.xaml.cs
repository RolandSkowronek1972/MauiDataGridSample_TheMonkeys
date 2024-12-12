using Microsoft.Maui.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ToCIVP_II
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new();

        public bool IsRefreshing { get; set; }
        public ObservableCollection<TheProducts> ProductsList { get; set; } = new();
        public ObservableCollection<TheProducts> InitialList { get; set; } = new();
        public ObservableCollection<TheProducts> PreviousList { get; set; } = new();
        public Command RefreshCommand { get; set; }

        private int searchCountValue = 0;

        public MainPage()
        {
            RefreshCommand = new Command(async () =>
            {
                await LoadProductsList();

                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
            });

            BindingContext = this;

            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await LoadProductsList();
        }

        private async Task LoadProductsList()
        {
            var productsList = await httpClient.GetFromJsonAsync<TheProducts[]>("https://api.jsonsilo.com/public/d1395a15-a054-44dd-bae3-7160968e1c19");

            ProductsList.Clear();

            foreach (var product in productsList)
            {
                ProductsList.Add(product);
            }

            InitialList = new ObservableCollection<TheProducts>(ProductsList);
        }

        private async void loadToProductlist(List<TheProducts> thisList)
        {
            foreach (TheProducts Product in thisList)
            {
                ProductsList.Add(Product);
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            var searchBar = (SearchBar)sender;
            var SerchingName = searchBar.Text;
            List<TheProducts> ShortInitialList = InitialList.Where(x => x.Name.Contains(SerchingName)).ToList();
            //  ProductsList.Clear();
            //   ProductsList = new ObservableCollection<TheProducts>(ShortInitialList);

            int curentSearchCountValue = SerchingName.Length;
            ObservableCollection<TheProducts> ProductsListCopy = new ObservableCollection<TheProducts>();
            if (curentSearchCountValue > searchCountValue)
            {
                ProductsListCopy = new ObservableCollection<TheProducts>(ProductsList);
            }
            else
            {
                loadToProductlist(PreviousList.ToList());

                ProductsListCopy = new ObservableCollection<TheProducts>(InitialList);
            }

            searchCountValue = curentSearchCountValue;

            foreach (var product in ProductsListCopy)
            {
                var iD = product.Id;
                bool ItemExist = ShortInitialList.Any(id => id.Id == iD);
                if (!ItemExist)
                {
                    ProductsList.Remove(product);
                }
            }
            if (curentSearchCountValue > searchCountValue)
            {
                ProductsListCopy = new ObservableCollection<TheProducts>(ProductsList);
            }
            /* ProductsList.Clear();*/

            if (String.IsNullOrEmpty(SerchingName))
            {
                loadToProductlist(InitialList.ToList());
            }
        }
    }
}