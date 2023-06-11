using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LibraryApp.Services;
using LibraryApp.ViewModels;
using LibraryApp.Models;
using System.Windows.Input;
using System.Linq;

namespace LibraryApp.ViewModels
{
    public class BooksViewModel : PageViewModelBase
    {
        private readonly LibraryApiServices _apiService;
        private List<Books> _books;
        private string _selectedBook;
        public ICommand AddCommand { get; private set; }
        public ICommand ModifyCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public BooksViewModel()
        {
            _apiService = new LibraryApiServices();
            LoadBooksAsync();

            AddCommand = new RelayCommand(AddAction);
            ModifyCommand = new RelayCommand(ModifyAction);
            DeleteCommand = new RelayCommand(DeleteAction);
        }

        public List<Books> Books
        {
            get { return _books; }
            set
            {
                if (_books != value)
                {
                    _books = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                if (_selectedBook != value)
                {
                    _selectedBook = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                Books = await _apiService.GetBooks();

                if (Books.Count > 0)
                {
                    SelectedBook = Books[0].Title;
                }
                else
                {
                    SelectedBook = "no book available";

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
                var newBook = new Books
                {
                    ISBN = 5,
                    Title = "NewTitle",
                    NameAuthor = "NewAuthor",
                    Price = 10,
                    YearPublication = "2023",
                    NumberPages = 100,
                    Stock = 10,
                    Type = "NewType"
                };

                _apiService.AddBooks(newBook);

                Books.Add(newBook);

            }
            catch (Exception ex)
            {
                Console.WriteLine("one error during on adding book : " + ex.Message);
            }
        }


        private void ModifyAction()
        {
            if (string.IsNullOrEmpty(SelectedBook))
            {
                return;
            }

            try
            {
                var bookToModify = Books.FirstOrDefault(b => b.Title == SelectedBook);

                if (bookToModify != null)
                {

                    _apiService.ModifyBook(bookToModify);

                    
                    SelectedBook = null; // i reinitialize
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("one error during on modifying book : " + ex.Message);
            }
        }


        private void DeleteAction()
        {
            if (string.IsNullOrEmpty(SelectedBook))
            {
                return;
            }

            try
            {
                var bookToDelete = Books.FirstOrDefault(b => b.Title == SelectedBook);

                if (bookToDelete != null)
                {
                    _apiService.DeleteBook(bookToDelete.ISBN);

                    if (Books.Contains(bookToDelete))
                    {
                        Books.Remove(bookToDelete);
                    }

                    SelectedBook = null; //here i reinitialize the selected book
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

        public override bool CanNavigatePrevious
        {
            get => false;
            protected set => throw new NotSupportedException();
        }
    }
}
