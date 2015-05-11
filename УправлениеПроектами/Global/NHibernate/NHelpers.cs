using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace УправлениеПроектами.NHibernate
{
    /// <summary>
    /// Синглтон класс для хранения NHibernate Session Factory
    /// </summary>
    public sealed class NHelpers
    {
        private static readonly NHelpers mInstance = new NHelpers();
        private static ISessionFactory mIsessionFactory;

        public static NHelpers Instance
        {
            get { return mInstance; }
        }

        public ISessionFactory SessionFactory
        {
            get { return mIsessionFactory; }
            set { mIsessionFactory = value; }
        }
    }
}