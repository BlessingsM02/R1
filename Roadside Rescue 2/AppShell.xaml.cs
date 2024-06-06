using Roadside_Rescue_2.Views;
namespace Roadside_Rescue_2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private readonly static Type[] _routablePageTypes =
        [
            typeof(RegisterPage),
            typeof(HistoryPage),
            typeof(ProfilePage)

        ];

        private static void RegisterRoutes()
        {
            foreach (var pageType in _routablePageTypes)
            {
                Routing.RegisterRoute(pageType.Name, pageType);
            }
        }
    }
}
