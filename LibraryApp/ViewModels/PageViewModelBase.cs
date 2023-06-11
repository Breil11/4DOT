using LibraryApp.ViewModels;
using LibraryApp.ViewModels;

namespace LibraryApp.ViewModels
{
    
    public abstract class PageViewModelBase : ViewModelBase
    {
        
        public abstract bool CanNavigateNext { get; protected set; }

        
        public abstract bool CanNavigatePrevious { get; protected set; }
    }
}
