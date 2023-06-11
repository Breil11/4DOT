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
using System.Linq;

namespace LibraryApp.ViewModels
{
    public class BorrowersViewModel : PageViewModelBase

    {
        private readonly LibraryApiServices _apiService;
        private List<Borrowers> _borrowers;
        private string _selectedBorrowers;
        public ICommand AddCommand { get; private set; }
        public ICommand ModifyCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public BorrowersViewModel()
        {
            _apiService = new LibraryApiServices();
            LoadBorrowersAsync();
        }

        public List<Borrowers> Borrowers
        {
            get { return _borrowers; }
            set
            {
                if (_borrowers != value)
                {
                    _borrowers = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedBorrowers
        {
            get { return _selectedBorrowers; }
            set
            {
                if (_selectedBorrowers != value)
                {
                    _selectedBorrowers = value;
                    RaisePropertyChanged();
                }
            }
        }

        

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadBorrowersAsync()
        {
            var apiService = new LibraryApiServices();

            try
            {
                Borrowers = await apiService.GetBorrowers();

                if (Borrowers.Count > 0)
                {
                    SelectedBorrowers = Borrowers[0].Firstname;
                }
                else
                {
                    // Gérer le cas où aucun livre n'est retourné par l'API
                    // Par exemple, vous pouvez affecter une valeur par défaut à SelectedBook ou afficher un message d'erreur.
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion à l'API
                // Par exemple, vous pouvez afficher un message d'erreur ou effectuer d'autres actions appropriées.
            }
        }
        private void AddAction()
        {

            try
            {
                var newBorrowers = new Borrowers
                {
                    IdBorrowers = 5,
                    Firstname = "NewFirstname",
                    Lastname = "NewLastname",

                };

                _apiService.AddBorrowers(newBorrowers);

                Borrowers.Add(newBorrowers);

            }
            catch (Exception ex)
            {
                Console.WriteLine("one error during on adding book : " + ex.Message);
            }
        }


        private void ModifyAction()
        {
            if (string.IsNullOrEmpty(SelectedBorrowers))
            {
                return;
            }

            try
            {
                var borrowersToModify = Borrowers.FirstOrDefault(b => b.Firstname == SelectedBorrowers);

                if (borrowersToModify != null)
                {

                    _apiService.ModifyBook(borrowersToModify);


                    SelectedBorrowers = null; // i reinitialize
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("one error during on modifying book : " + ex.Message);
            }
        }


        private void DeleteAction()
        {
            if (string.IsNullOrEmpty(SelectedBorrowers))
            {
                return;
            }

            try
            {
                var borrowersToDelete = Borrowers.FirstOrDefault(b => b.Firstname == SelectedBorrowers);

                if (borrowersToDelete != null)
                {
                    _apiService.DeleteBorrowers(borrowersToDelete.Firstname);

                    if (Borrowers.Contains(borrowersToDelete))
                    {
                        Borrowers.Remove(borrowersToDelete);
                    }

                    SelectedBorrowers = null; //here i reinitialize the selected book
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("one error during on deleting book : " + ex.Message);
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
