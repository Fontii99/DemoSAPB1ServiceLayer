using Newtonsoft.Json;
using B1SLayer;
using DemoSAPB1ServiceLayer.Entities;
using System.Reflection;
using DemoSAPB1ServiceLayer.Attributes;
using System.Data;

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
            LResponse.Text = "";


            Client client = new Client
            {
                CardCode = TBCardCode.Text,
                CardName = TBCardName.Text,
                CardType = CBCardType.Text
            };

            LResponse.Text += client.ToString();

            foreach (DataGridViewRow row in DGItem.Rows)
            {
                if (row.IsNewRow || (string.IsNullOrEmpty(row.Cells[0].Value?.ToString()) && string.IsNullOrEmpty(row.Cells[1].Value?.ToString())))
                {
                    continue;
                }

                Item item = new Item
                {
                    ItemCode = row.Cells[0].Value?.ToString(),
                    ItemName = row.Cells[1].Value?.ToString(),
                    ItemStock = row.Cells[3].Value != null ? Convert.ToInt32(row.Cells[3].Value) : 0
                };

                LResponse.Text += item.ToString();

                await post(item);
            }

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

                T slResponse = await serviceLayer
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

                T client = await serviceLayer
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
