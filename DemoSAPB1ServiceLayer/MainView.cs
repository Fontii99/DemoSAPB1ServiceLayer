using Newtonsoft.Json;
using B1SLayer;
using DemoSAPB1ServiceLayer.Entities;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using DemoSAPB1ServiceLayer.Attributes;
using System.Reflection;

namespace DemoSAPB1ServiceLayer

{
    public partial class MainView : Form
    {
        public bool SearchMode = false;
        private readonly SLConnection serviceLayer;
        public string response = "";
        private List<Item> items = new();
        List<SLBatchRequest> batchRequests = new List<SLBatchRequest>();
        int batchCounter = 1;
        public MainView(SLConnection Sl)
        {
            serviceLayer = Sl;
            InitializeComponent();
        }

        private void BSearch_Click(object sender, EventArgs e)
        {
            BAction.Text = "Search";
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
            BAction.Text = "Add";
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
            try
            {
                batchRequests.Clear();
                items.Clear();
                LResponse.Text = "";
                batchCounter = 1;

                // Check if client exists
                var client = new Client
                {
                    CardCode = TBCardCode.Text,
                    CardName = TBCardName.Text,
                    CardType = CBCardType.Text,
                    Table = "BusinessPartners"
                };

                bool clientExists = true;
                try
                {
                    await get(client);
                    if (response.Contains("Exception"))
                        clientExists = false;
                }
                catch
                {
                    clientExists = false;
                }

                // Add client to batch if it doesn't exist
                if (!clientExists)
                {
                    var batchClient = new SLBatchRequest(HttpMethod.Post, "BusinessPartners",
                        new
                        {
                            CardCode = TBCardCode.Text,
                            CardName = TBCardName.Text,
                            CardType = CBCardType.Text
                        });
                    batchClient.ContentID = batchCounter++;
                    batchRequests.Add(batchClient);
                }

                // Process items and check if they exist
                var orderLines = new List<dynamic>();

                foreach (DataGridViewRow row in DGItem.Rows)
                {
                    if (row.IsNewRow || (string.IsNullOrEmpty(row.Cells[0].Value?.ToString()) &&
                        string.IsNullOrEmpty(row.Cells[1].Value?.ToString())))
                    {
                        continue;
                    }

                    var item = new Item
                    {
                        ItemCode = row.Cells[0].Value?.ToString(),
                        ItemName = row.Cells[1].Value?.ToString(),
                        DefaultWarehouse = "01",
                        Table = "Items"
                    };

                    bool itemExists = true;
                    try
                    {
                        await get(item);
                        if (response.Contains("Exception"))
                            itemExists = false;
                    }
                    catch
                    {
                        itemExists = false;
                    }

                    // Add item to batch if it doesn't exist
                    if (!itemExists)
                    {
                        var batchItem = new SLBatchRequest(HttpMethod.Post, "Items", new
                        {
                            ItemCode = item.ItemCode,
                            ItemName = item.ItemName,
                            DefaultWarehouse = item.DefaultWarehouse
                        });
                        batchItem.ContentID = batchCounter++;
                        batchRequests.Add(batchItem);
                    }

                    // Add to order lines regardless of whether item exists or not
                    decimal quantity = 0;
                    decimal price = 0;
                    int discount = 0;

                    decimal.TryParse(row.Cells[3].Value?.ToString(), out quantity);
                    decimal.TryParse(row.Cells[2].Value?.ToString(), out price);
                    int.TryParse(row.Cells[4].Value?.ToString(), out discount);

                    item.QuantityOnStock = quantity;
                    item.Price = price;
                    item.Discount = discount;
                    items.Add(item);

                    orderLines.Add(new
                    {
                        ItemCode = item.ItemCode,
                        Quantity = quantity,
                        UnitPrice = price,
                        DiscountPercent = discount
                    });
                }

                // Only create order if we have items and either the client exists or will be created
                if (orderLines.Any() && (clientExists || batchRequests.Any(r => r.Resource == "BusinessPartners")))
                {
                    var batchOrder = new SLBatchRequest(HttpMethod.Post, "Orders", new
                    {
                        CardCode = TBCardCode.Text,
                        DocType = "dDocument_Items",
                        DocDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        DocDueDate = DateTime.Now.AddDays(30),
                        DocumentLines = orderLines.ToArray()
                    });
                    batchOrder.ContentID = batchCounter;
                    batchRequests.Add(batchOrder);
                }

                // Only execute batch if there are requests
                if (batchRequests.Count > 0)
                {
                    var batchResult = await serviceLayer.PostBatchAsync(batchRequests.ToArray());
                    foreach (var line in batchResult)
                    {
                        var result = await line.Content.ReadAsStringAsync();
                        LResponse.Text += result + Environment.NewLine;
                    }
                }
                else
                {
                    LResponse.Text = "No operations needed - all records exist";
                }
            }
            catch (Exception ex)
            {
                LResponse.Text = $"Error: {ex.Message}";
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
    }
}
