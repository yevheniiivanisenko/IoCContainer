namespace IoCContainer.Container.Tests.Entities
{
    [ImportConstructor]
    public class CustomerBLL
    {
        public CustomerBLL(ICustomerDAL dal, Logger logger) { }
    }
}
