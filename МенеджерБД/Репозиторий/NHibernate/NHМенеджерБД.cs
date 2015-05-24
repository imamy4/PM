using NHibernate;
using NHibernate.Criterion;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Безопасность;

namespace МенеджерБД
{
    using Домен;

    public partial class NHМенеджерБД : IМенеджерБД
    {
        [Inject]
        public ISessionFactory ФабрикаСессийNHibernate { set; get; }

        public IEnumerable<T> Записи<T>() where T : class, IЗаписьБД
        {
            using (ISession session = ФабрикаСессийNHibernate.OpenSession())
            {
                var записи = session
                    .CreateCriteria(typeof(T))
                    .List<T>();

                return записи;
            }
        }

        public IEnumerable<T> Записи<T>(Func<T, bool> критерийОтбора) where T : class, IЗаписьБД
        {
            return Записи<T>().Where(критерийОтбора);
        }

        public bool СуществуетЗапись<T>(Func<T, bool> критерийОтбора) where T : class, IЗаписьБД
        {
            return Записи<T>(критерийОтбора).Count() > 0;
        }

        public bool СоздатьЗаписьБД<T>(T запись) where T : class, IЗаписьБД
        {
            using (ISession session = ФабрикаСессийNHibernate.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    session.Save(запись);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // логгируем ошибку
                    return false;
                }
            }

            return true;
        }

        public bool ОбновитьЗаписьБД<T>(T запись) where T : class, IЗаписьБД
        {
            using (ISession session = ФабрикаСессийNHibernate.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    session.Update(запись);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // логгируем ошибку
                    return false;
                }
            }

            return true;
        }

        public bool УдалитьЗаписьБД<T>(int idЗаписи) where T : class, IЗаписьБД
        {
            return УдалитьЗаписьБД<T>(ПолучитьЗаписьБДПоId<T>(idЗаписи));
        }

        public bool УдалитьЗаписьБД<T>(T запись) where T : class, IЗаписьБД
        {
            using (ISession session = ФабрикаСессийNHibernate.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    session.Delete(запись);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // логгируем ошибку
                    return false;
                }
            }

            return true;
        }

        public T ПолучитьЗаписьБДПоId<T>(int idЗаписи) where T : class, IЗаписьБД
        {
            T запись = null;
            using (ISession session = ФабрикаСессийNHibernate.OpenSession())
            {
                запись = session.Get<T>(idЗаписи);
            }

            return запись;
        }

        public T ПолучитьПервуюЗаписьБД<T>(Func<T, bool> критерийОтбора) where T : class, IЗаписьБД
        {
            return Записи<T>(критерийОтбора).FirstOrDefault();
        }


        public Пользователь Авторизоваться(string email, string пароль)
        {
            using (ISession session = ФабрикаСессийNHibernate.OpenSession())
            {
                var пользователь = session
                    .CreateCriteria(typeof(Пользователь))
                    .Add(Restrictions.Eq("Email", email))
                    .Add(Restrictions.Eq("ХэшПароля", Шифрование.ПолучитьХешMD5Строкой(пароль)))
                    .UniqueResult<Пользователь>();

                return пользователь;
            }
        }


        //public IEnumerable<T> Записи<T>(Dictionary<string, object> критерийОтбора) where T : class, IЗаписьБД
        //{
        //    using (ISession session = ФабрикаСессийNHibernate.OpenSession())
        //    {
        //        var criteria = session
        //            .CreateCriteria(typeof(T));

        //        foreach (var kvp in критерийОтбора)
        //        {
        //            criteria.Add(Restrictions.Eq(kvp.Key, kvp.Value));
        //        }

        //        return criteria.List<T>();
        //    }
        //}

        //public IEnumerable<T> Записи<T>(IEnumerable<Критерий> критерийОтбора) where T : class, IЗаписьБД
        //{
        //    using (ISession session = ФабрикаСессийNHibernate.OpenSession())
        //    {
        //        var criteria = session
        //            .CreateCriteria(typeof(T));

        //        foreach (var критерий in критерийОтбора)
        //        {
        //            switch (критерий.ТипСравнения)
        //            {
        //                case ТипыСравнения.Равно:

        //                    criteria.Add(Restrictions.Eq(критерий.Ключ, критерий.Значение));
        //                    break;

        //                default:
        //                    throw new NotImplementedException();
        //            }
        //        }

        //        return criteria.List<T>();
        //    }
        //}

        //public T ПолучитьПервуюЗаписьБДПоУсловию<T>(Dictionary<string, object> критерийОтбора) where T : class, IЗаписьБД
        //{
        //    using (ISession session = ФабрикаСессийNHibernate.OpenSession())
        //    {
        //        var criteria = session
        //            .CreateCriteria(typeof(T));

        //        foreach (var kvp in критерийОтбора)
        //        {
        //            criteria.Add(Restrictions.Eq(kvp.Key, kvp.Value));
        //        }

        //        return criteria.UniqueResult<T>();
        //    }
        //}

        //public T ПолучитьПервуюЗаписьБДПоУсловию<T>(IEnumerable<КритерийОтбора.Критерий> критерийОтбора) where T : class, IЗаписьБД
        //{
        //    throw new NotImplementedException();
        //}
    }
}
