using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LibraryApp.Services;
using LibraryApp.ViewModels;
using LibraryApp.Models;
using WebApi.Models;

namespace LibraryApp.ViewModels
{
    public class HomescreenViewModel : PageViewModelBase
    {
        private readonly LibraryApiServices _apiService;
        private List<Loan> _loans;
        private Loan _selectedLoan;

        public event PropertyChangedEventHandler? PropertyChanged;

        public HomescreenViewModel()
        {
            _apiService = new LibraryApiServices();
            LoadLoansAsync();
        }

        public List<Loan> Loans
        {
            get { return _loans; }
            set
            {
                if (_loans != value)
                {
                    _loans = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Loan SelectedLoan
        {
            get { return _selectedLoan; }
            set
            {
                if (_selectedLoan != value)
                {
                    _selectedLoan = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadLoansAsync()
        {
            var apiService = new LibraryApiServices();

            try
            {
                Loans = await apiService.GetLoansOfToday();

                if (Loans.Count > 0)
                {
                    SelectedLoan = Loans[0];
                }
                else
                {
                    // Gérer le cas où aucun prêt n'est retourné par l'API
                    // Par exemple, vous pouvez affecter une valeur par défaut à SelectedLoan ou afficher un message d'erreur.
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion à l'API
                // Par exemple, vous pouvez afficher un message d'erreur ou effectuer d'autres actions appropriées.
            }
        }

        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        // You cannot go back from this page
        public override bool CanNavigatePrevious
        {
            get => false;
            protected set => throw new NotSupportedException();
        }
    }
}
