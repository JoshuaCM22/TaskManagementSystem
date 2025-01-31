using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaskManagementSystem.Models.BusinessLogicLayer;
using TaskManagementSystem.Models.DataAccessLayer;
using TaskManagementSystem.Models.Interfaces;
using Unity;
using Unity.AspNet.Mvc;

namespace TaskManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           // UnityConfig.RegisterComponents();

            // Register the Unity container and configure dependency injection
            IUnityContainer container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            // Register your interfaces and their corresponding implementations

            /*
                  INSTALL THESE TWO LIBRARIES FIRST IN NUGET PACKAGE MANAGER.
                  1) UnityContainer = Unity (Library)
                  2) UnityDependencyResolver = Unity.MVC (Library)
            */

   
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<ITaskService, TaskService>();
            container.RegisterType<ITaskRepository, TaskRepository>();
        }


    }
}
