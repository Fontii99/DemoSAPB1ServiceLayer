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

        public Item()
        {
            Table = "Items";
        }
        public Item(string itemCode, string itemName)
        {
            ItemCode = itemCode;
            ItemName = itemName;
            Table = "Items";
        }

        public override string ToString()
        {
            return $"ItemCode: {ItemCode}, ItemName: {ItemName}";
        }
    }
}
