using Autofac;
using MeetingManagementServer.Services;
using MeetingManagementServer.Services.Interfaces;

namespace MeetingManagementServer
{
    internal class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfTransactionFactory>()
                 .As<ITransactionFactory>()
                 .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfRepository<>))
                 .As(typeof(IRepository<>))
                 .InstancePerLifetimeScope();

            builder.RegisterType<MeetingManager>()
                 .As<IMeetingManager>()
                 .InstancePerLifetimeScope();
        }
    }
}