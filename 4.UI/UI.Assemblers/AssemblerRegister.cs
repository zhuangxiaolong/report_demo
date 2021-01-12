using Autofac;

namespace UI.Assemblers
{

    public static class AssemblerRegister
    {
        public static void RegisterAssembler(this ContainerBuilder builder)
        {
            builder.RegisterType<UserAssembler>().AsImplementedInterfaces();
            builder.RegisterType<ReportAssembler>().AsImplementedInterfaces();
        }
    }

}