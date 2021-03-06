﻿using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using OnRadio.App.Installers;
using OnRadio.App.ViewModels;
using OnRadio.BL.Services;

namespace OnRadio.App.Common
{
    public class ViewModelLocator
    {
        private static readonly IContainer Container;

        public RadioListViewModel RadioList => Container.Resolve<RadioListViewModel>();

        public PlayerViewModel Player => Container.Resolve<PlayerViewModel>();

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<IoCInstaller>();
            builder.RegisterModule<ViewModelInstaller>();

            Container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}