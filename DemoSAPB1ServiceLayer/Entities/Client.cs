using DemoSAPB1ServiceLayer.Attributes;

namespace DemoSAPB1ServiceLayer.Entities
{
    public class Client
    {
        [TableName]
        public string Table { get; set; }

        [PrimaryKey]
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }

        public Client(string cardCode, string cardName, string cardType)
        {
            CardCode = cardCode;
            CardName = cardName;
            CardType = cardType;
            Table = "BusinessPartners";
        }
        public Client()
        {

        }
        public override string ToString()
        {
            return $"CardCode: {CardCode}, CardName: {CardName}, CardType: {CardType}\n";
        }
    }
}