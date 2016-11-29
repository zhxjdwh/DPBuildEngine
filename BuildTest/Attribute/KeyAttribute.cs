using System;

namespace BuildTest.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class KeyAttribute:System.Attribute
    {
    }
}
