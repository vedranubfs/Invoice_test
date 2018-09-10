using System.Web.Mvc;
using Test.Data;
using Test.Data.Interfaces;
using Unity;
using Unity.Mvc5;

namespace Test
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}