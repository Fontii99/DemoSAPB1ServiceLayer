using Newtonsoft.Json;
using B1SLayer;
using DemoSAPB1ServiceLayer.Entities;
using System.Reflection;
using DemoSAPB1ServiceLayer.Attributes;

namespace DemoSAPB1ServiceLayer

{
    public partial class MainView : Form
    {
        public bool SearchMode = false;
        private readonly SLConnection serviceLayer;
        public string response = "";
        public MainView(SLConnection Sl)
        {
            serviceLayer = Sl;
            InitializeComponent();
        }

        private void BSearch_Click(object sender, EventArgs e)
        {
            TBCardCode.Text = "";
            TBCardName.Text = "";
            CBCardType.Text = "";
            response = "";
            TBCardCode.BackColor = Color.LightGoldenrodYellow;
            TBCardName.BackColor = Color.LightGoldenrodYellow;
            CBCardType.BackColor = Color.LightGoldenrodYellow;
            SearchMode = true;
        }

        private void BAdd_Click(object sender, EventArgs e)
        {
            TBCardCode.Text = "";
            TBCardName.Text = "";
            CBCardType.Text = "";
            response = "";
            TBCardCode.BackColor = Color.White;
            TBCardName.BackColor = Color.White;
            CBCardType.BackColor = Color.White;
            SearchMode = false;
        }

        private async void BAction_Click(object sender, EventArgs e)
        {
            Client client = new Client
            {
                CardCode = TBCardCode.Text,
                CardName = TBCardName.Text,
                CardType = CBCardType.Text
            };
            //Item item = new Item
            //{
            //    ItemCode = 
            //};
            LResponse.Text = "";

            if (SearchMode)
            {
                await get(client);
                LResponse.Text += $"\nCustomer found: {response}";
            }
            else
            {
                await post(client);

                LResponse.Text += $"\nCustomer created: {response}";
            }
        }
        private async Task get<T>(T itemToGet) where T : class
        {
            try
            {
                var properties = typeof(T).GetProperties();

                var tableProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<TableNameAttribute>() != null);
                var keyProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null);

                string table = tableProperty.GetValue(itemToGet)?.ToString();
                string primaryKey = keyProperty.GetValue(itemToGet)?.ToString();

                var slResponse = await serviceLayer
                    .Request(table, primaryKey)
                    .GetAsync<T>();

                response = JsonConvert.SerializeObject(slResponse);
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }

        }
        private async Task post<T>(T itemToPost)
        {
            try
            {

                var properties = typeof(T).GetProperties();

                var tableProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<TableNameAttribute>() != null);

                string table = tableProperty.GetValue(itemToPost)?.ToString();

                var client = await serviceLayer
                    .Request(table)
                    .PostAsync<T>(itemToPost);

                response = JsonConvert.SerializeObject(client);
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }
        }
    }
}
