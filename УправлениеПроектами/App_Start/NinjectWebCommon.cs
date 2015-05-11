using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

using Ninject;
using Ninject.Web.Common;

using NHibernate;
using NHConfiguration = NHibernate.Cfg.Configuration;
using МенеджерБД;
using МенеджерБД.Домен;
using УправлениеПроектами.NHibernate;
using УправлениеПроектами.Global.Auth;

[assembly: WebActivator.PreApplicationStartMethod(typeof(УправлениеПроектами.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(УправлениеПроектами.App_Start.NinjectWebCommon), "Stop")]
namespace УправлениеПроектами.App_Start
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // устанавливаем сессию NHibernate
            NHConfiguration configuration = new NHConfiguration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Проект).Assembly);
            NHelpers.Instance.SessionFactory = configuration.BuildSessionFactory();

            kernel.Bind<ISessionFactory>().ToMethod(
                c => NHelpers.Instance.SessionFactory);
            kernel.Bind<IМенеджерБД>().To<NHМенеджерБД>().InRequestScope();
            kernel.Bind<IАутентификация>().To<АутентификацияПользователя>().InRequestScope();
        }        
    }

    
}
