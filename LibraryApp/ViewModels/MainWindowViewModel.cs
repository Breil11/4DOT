using LibraryApp.ViewModels;
using DynamicData;
using ReactiveUI;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public HomescreenViewModel HomescreenViewModel { get; } = new HomescreenViewModel();


        // Add our BooksViewModel
        public BooksViewModel BooksViewModel { get; } = new BooksViewModel();

        // Add our BorrowersViewModel
        public BorrowersViewModel BorrowersViewModel { get; } = new BorrowersViewModel();

        // Add our LoansViewModel
        public LoansViewModel LoansViewModel { get; } = new LoansViewModel();

        // Add our ReturnViewModel
        public ReturnViewModel ReturnViewModel { get; } = new ReturnViewModel();


        public MainWindowViewModel()
        {
            // Set current page to first on start up
            _CurrentPage = Pages[0];

            // Create Observables which will activate to deactivate our commands based on CurrentPage state
            var canNavNext = this.WhenAnyValue(x => x.CurrentPage.CanNavigateNext);
            var canNavPrev = this.WhenAnyValue(x => x.CurrentPage.CanNavigatePrevious);

            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);
        }

        // A read.only array of possible pages
        private readonly PageViewModelBase[] Pages =
        {
            new HomescreenViewModel(),
            new BooksViewModel(),
            new BorrowersViewModel()
        };

        // The default is the first page
        private PageViewModelBase _CurrentPage;

        /// <summary>
        /// Gets the current page. The property is read-only
        /// </summary>
        public PageViewModelBase CurrentPage
        {
            get { return _CurrentPage; }
            private set { this.RaiseAndSetIfChanged(ref _CurrentPage, value); }
        }

        /// <summary>
        /// Gets a command that navigates to the next page
        /// </summary>
        public ICommand NavigateNextCommand { get; }

        private void NavigateNext()
        {
            // get the current index and add 1
            var index = Pages.IndexOf(CurrentPage) + 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = Pages[index];
        }

        /// <summary>
        /// Gets a command that navigates to the previous page
        /// </summary>
        public ICommand NavigatePreviousCommand { get; }

        private void NavigatePrevious()
        {
            // get the current index and subtract 1
            var index = Pages.IndexOf(CurrentPage) - 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = Pages[index];

        }
    }
}