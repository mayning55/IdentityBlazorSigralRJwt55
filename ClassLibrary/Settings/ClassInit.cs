using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ClassLibrary.Settings
{
    /// <summary>
    /// 通过托管服务初始化Admin用户
    /// </summary>
    public class ClassInit : BackgroundService
    {
        private readonly IServiceScope iServiceScope;
        public ClassInit(IServiceScopeFactory iServiceScopeFactory)
        {
            this.iServiceScope = iServiceScopeFactory.CreateScope();
        }
        public override void Dispose()
        {
            this.iServiceScope.Dispose();//释放非托管资源 
            base.Dispose();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var addClassServer = iServiceScope.ServiceProvider.GetRequiredService<InitAdmin>();
            await addClassServer.Manage();
            return;

        }
    }

}
