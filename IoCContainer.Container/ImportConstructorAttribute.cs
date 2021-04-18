using System;

namespace IoCContainer.Container
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImportConstructorAttribute : Attribute { }
}
