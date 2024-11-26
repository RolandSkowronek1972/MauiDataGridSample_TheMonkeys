using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ToCIVP_II
{
    // Plugin repo: https://github.com/akgulebubekir/Maui.DataGrid
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new();

        public bool IsRefreshing { get; set; }
        public ObservableCollection<TheProducts> ProductsList { get; set; } = new();
        public ObservableCollection<TheProducts> InitialList { get; set; } = new();

        public Command RefreshCommand { get; set; }
        

        public MainPage()
        {
            RefreshCommand = new Command(async () =>
            {
                // Simulate delay
                await Task.Delay(2000);

                await LoadProductsList();

                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
            });

            BindingContext = this;

            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
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
                InitialList.Add(product);   


            }
          
        }
    }
}