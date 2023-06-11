using System.Threading.Tasks;
using LibraryApp.Services;
using ReactiveUI;

namespace LibraryApp.ViewModels
{
    public class ReturnViewModel : ReactiveObject
    {
        private string _returnName;
        public string ReturnName
        {
            get => _returnName;
            set => this.RaiseAndSetIfChanged(ref _returnName, value);
        }

        private string _returnGreeting;
        public string ReturnGreeting
        {
            get => _returnGreeting;
            set => this.RaiseAndSetIfChanged(ref _returnGreeting, value);
        }

        public ReturnViewModel()
        {
            LoadReturnDataAsync();
        }

        private async Task LoadReturnDataAsync()
        {

            //var returnData = await LibraryApiServices.GetReturnData();
            //ReturnName = returnData.Name;
            //ReturnGreeting = returnData.Greeting;
        }
    }
}
