using System;

namespace Domain.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbFKAttribute : Attribute
    {

    }
}