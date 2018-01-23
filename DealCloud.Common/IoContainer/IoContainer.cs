using System;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Unity;
using Unity.Resolution;

namespace DealCloud.Common
{
    public static class IoContainer
    {
        private static readonly UnityContainer Container;

        static IoContainer()
        {
            Container = new UnityContainer();

            var section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;

            section?.Configure(Container);
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T ResolveWithParamsOverride<T>(params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(overrides);
        }

        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            Container.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        ///     Registers type with object. object will be sinleton for all Resolve<> calls
        /// </summary>
        public static void RegisterSingleton(Type type, object instance)
        {
            Container.RegisterInstance(type, instance);
        }

        /// <summary>
        ///     register instance as singeton. will returned by Resolve<> calls
        /// </summary>
        public static void RegisterSingleton<TInterface>(TInterface instance)
        {
            Container.RegisterInstance(instance);
        }
    }
}