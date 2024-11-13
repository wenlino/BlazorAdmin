namespace AdminSenyun.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddServiceAttribute : Attribute
    {
        public AddServiceAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; set; }
    }
}
