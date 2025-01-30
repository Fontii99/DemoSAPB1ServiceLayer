using DemoSAPB1ServiceLayer.Attributes;

namespace DemoSAPB1ServiceLayer.Entities
{
    public class Item
    {
        [TableName]
        public string Table { get; set; }

        [PrimaryKey]
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal QuantityOnStock { get; set; }
        public string DefaultWarehouse { get; set; }

        public decimal Price { get; set; }
        public int Discount { get; set; }

        public Item(string itemCode, string itemName, decimal itemStock, string defaultWarehouse, decimal price, int discount)
        {
            ItemCode = itemCode;
            ItemName = itemName;
            QuantityOnStock = itemStock;
            DefaultWarehouse = defaultWarehouse;
            Table = "Items";
            Price = price;
            Discount = discount;
        }
        public Item()
        {

        }

        public override string ToString()
        {
            return $"ItemCode: {ItemCode}, ItemName: {ItemName}\n";
        }
    }
}
