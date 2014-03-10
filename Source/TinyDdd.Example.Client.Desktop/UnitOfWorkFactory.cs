using System;
using System.Configuration;
using StructureMap;

namespace TinyDdd.Example.Client.Desktop
{
    public static class UnitOfWorkFactory
    {
        internal static UnitOfWork CreateUnitOfWork()
        {
            return (UnitOfWork) ObjectFactory.GetInstance(DefaultUnitOfWorkType);
        }

        internal static Type DefaultUnitOfWorkType {get { return Type.GetType(ConfigurationManager.AppSettings["DefaultUnitOfWorkType"]); }}
    }
}
