namespace DemoSAPB1ServiceLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableNameAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute { }
}
