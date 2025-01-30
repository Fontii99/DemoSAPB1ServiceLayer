using Newtonsoft.Json;
using B1SLayer;
using DemoSAPB1ServiceLayer.Entities;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;

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
            LResponse.Text = "";

            var batchClient = new SLBatchRequest(HttpMethod.Post, "BusinessPartners",
                new
                {
                    CardCode = TBCardCode.Text,
                    CardName = TBCardName.Text,
                    CardType = CBCardType.Text
                });
            batchClient.ContentID = batchCounter;
            batchRequests.Add(batchClient);
            batchCounter++;

            foreach (DataGridViewRow row in DGItem.Rows)
            {
                if (row.IsNewRow || (string.IsNullOrEmpty(row.Cells[0].Value?.ToString()) && string.IsNullOrEmpty(row.Cells[1].Value?.ToString())))
                {
                    continue;
                }
                var batchItem = new SLBatchRequest(HttpMethod.Post, "Items", new
                {
                    ItemCode = row.Cells[0].Value?.ToString(),
                    ItemName = row.Cells[1].Value?.ToString(),
                    DefaultWarehouse = "01"
                });
                batchItem.ContentID = batchCounter;
                batchRequests.Add(batchItem);
                batchCounter++;

                decimal quantity;
                decimal price;
                int discount;
                Item item = new Item
                {
                    ItemCode = row.Cells[0].Value?.ToString(),
                    ItemName = row.Cells[1].Value?.ToString(),
                    DefaultWarehouse = "01",
                    Table = "Items"
                };
                if (decimal.TryParse(row.Cells[3].Value?.ToString(), out quantity))
                {
                    item.QuantityOnStock = quantity;
                }
                if (decimal.TryParse(row.Cells[2].Value?.ToString(), out price))
                {
                    item.Price = price;
                }
                if (int.TryParse(row.Cells[4].Value?.ToString(), out discount))
                {
                    item.Discount = discount;
                }
                items.Add(item);

            }

            var batchInvoice = new SLBatchRequest(HttpMethod.Post,
                                            "Invoices", new
            {
                Data = new
                {
                    CardCode = TBCardCode.Text,
                    DocType = "dDocument_Items",
                    DocDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    DocumentLines = items.Select(item => new
                    {
                        ItemCode = item.ItemCode,
                        Quantity = item.QuantityOnStock,
                        UnitPrice = item.Price,
                        DiscountPercent = item.Discount
                    }).ToArray()
                }
            });
            batchInvoice.ContentID = batchCounter;
            batchRequests.Add(batchInvoice);
            HttpResponseMessage[] batchResult = await serviceLayer.PostBatchAsync(batchRequests.ToArray());

            for (int i = 0; i < batchResult.Length; i++)
            {
                if (!batchResult[i].IsSuccessStatusCode)
                {
                    if (batchResult[i].Content != null && batchResult[i].Content.Headers.ContentType.MediaType == "application/json")
                    {
                        string jsonString = await batchResult[i].Content.ReadAsStringAsync();
                        SLResponseError error = JsonConvert.DeserializeObject<SLResponseError>(jsonString);
                        throw new Exception(error.Error.Message.Value);
                    }
                    else
                    {
                        throw new Exception("Error desconocido");
                    }
                }
                else
                {
                    if (batchResult[i].Content != null &&
                        batchResult[i].StatusCode == HttpStatusCode.Created &&
                        batchResult[i].Content.Headers.ContentType.MediaType == "application/json")
                    {
                        string jsonString = await batchResult[i].Content.ReadAsStringAsync();
                        var json = JObject.Parse(jsonString);
                    }
                }
            }

        }
    }
}
