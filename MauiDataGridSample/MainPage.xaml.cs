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

        public Command RefreshCommand { get; set; }
        

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

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await LoadProductsList();
        }

       
        private async Task LoadProductsList()
        {
            var productsList = await httpClient.GetFromJsonAsync<TheProducts[]>("https://api.jsonsilo.com/public/d1395a15-a054-44dd-bae3-7160968e1c19");
            int i = 0;
            ProductsList.Clear();
            foreach (var product in productsList) 
            {
                i++;
                ProductsList.Add(product);
                InitialList.Add(product);
            if (i == 10)
                    break;

            }
          
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
            SearchBar searchBar = (SearchBar)sender;
            var SerchingName = searchBar.Text;
            
            List<TheProducts> ShortInitialList = InitialList.Where(x => x.Name.Contains(SerchingName)).ToList();
            int ilosc = ShortInitialList.Count;

            ProductsList.Clear();

            if (String.IsNullOrEmpty(SerchingName))
            {
                loadToProductlist(InitialList.ToList());
            }
            else
            {
                loadToProductlist(ShortInitialList);
            }
        }
    }
}