using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LibraryApp.Services;
using LibraryApp.ViewModels;
using LibraryApp.Models;
using WebApi.Models;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    //public class LoansViewModel : INotifyPropertyChanged
    public class LoansViewModel : PageViewModelBase
    {
        private readonly LibraryApiServices _apiService;
        private List<Loan> _loans;
        private List<Loan> _loansToday;
        private string _selectedloan;

        public ICommand AddCommand { get; private set; }
        public ICommand ModifyCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public LoansViewModel()
        {
            _apiService = new LibraryApiServices();
            LoadLoanAsync();
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

        public List<Loan> LoansToday
        {
            get { return _loansToday; }
            set
            {
                if (_loansToday != value)
                {
                    _loansToday = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedLoan
        {
            get { return _selectedloan; }
            set
            {
                if (_selectedloan != value)
                {
                    _selectedloan = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadLoanAsync()
        {
            var apiService = new LibraryApiServices();

            try
            {
                Loans = await apiService.GetLoans();

                if (Loans.Count > 0)
                {
                    SelectedLoan = Loans[0].ReturnDate;
                }
                else
                {
                    SelectedLoan = "no book available";
                }
                LoansToday = await apiService.GetLoansOfToday();
                if (LoansToday.Count > 0)
                {
                    SelectedLoan = LoansToday[0].ReturnDate;
                }
                else
                {
                    SelectedLoan = "no book available";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("one error in recuperation of book : " + ex.Message);
            }
        }

        private void AddAction()
        {

            try
            {
                var newLoan = new Loan
                {
                    IdLoan = 5,
                    IdBorrowers = 5,
                    ISBN = 5,

                };

                _apiService.AddLoan(newLoan);

                Loans.Add(newLoan);

            }
            catch (Exception ex)
            {
                Console.WriteLine("one error during on adding book : " + ex.Message);
            }
        }
        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }


        public override bool CanNavigatePrevious
        {
            get => false;
            protected set => throw new NotSupportedException();
        }

    }
}
