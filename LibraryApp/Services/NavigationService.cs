using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;


namespace LibraryApp.Services
{
    public class NavigationService
    {
        public RoutingState Router { get; }

        public NavigationService()
        {
            Router = new RoutingState();
        }

        public void NavigateTo(string page)
        {
            Router.Navigate.Execute(page);
        }
    }
}
