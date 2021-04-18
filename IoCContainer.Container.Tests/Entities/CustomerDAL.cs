namespace IoCContainer.Container.Tests.Entities
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL { }
}
