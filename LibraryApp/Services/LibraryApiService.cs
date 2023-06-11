using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using LibraryApp.Models;
using Newtonsoft.Json;
using WebApi.Models;

namespace LibraryApp.Services
{


    public class LibraryApiServices
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7031";

        public LibraryApiServices()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Books>> GetBooks()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/books/GetBooks");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Books>>(content);
            }

            throw new Exception("Failed to retrieve books from the API.");
        }

        public async Task<List<Books>> ModifyBook(Books newBook)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Books/{title}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Books>>(content);
            }

            throw new Exception("Failed to retrieve books from the API.");
        }

        public async Task<List<Books>> AddBooks(Books newBook)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/books");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Books>>(content);
            }

            throw new Exception("Failed to retrieve books from the API.");
        }

        public async Task DeleteBook(int bookId)
        {
            var requestUrl = $"https://localhost:7031/api/Books/ByTitle/";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(requestUrl);
                response.EnsureSuccessStatusCode();
            }
        }


        //about borrowers
        public async Task<List<Borrowers>> GetBorrowers()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Borrowers/GetBorrowers");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Borrowers>>(content);
            }

            throw new Exception("Failed to retrieve borrowers from the API.");
        }
        public async Task<List<Borrowers>> AddBorrowers(Borrowers newBook)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Borrowers");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Borrowers>>(content);
            }

            throw new Exception("Failed to retrieve books from the API.");
        }

        public async Task DeleteBorrowers(string Firstname)
        {
            //var requestUrl = $"https://localhost:7031/api/Borrowers/{firstname}/{lastname}";     i comment this because i had an error but its the right use
            var requestUrl = $"https://localhost:7031/api/Borrowers/";   //optional use
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(requestUrl);
                response.EnsureSuccessStatusCode();
            }
        }


        //about loan

        public async Task<List<Loan>> AddLoan(Loan newLoan)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Loan/BorrowBooks");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Loan>>(content);
            }

            throw new Exception("Failed to retrieve books from the API.");
        }
        public async Task<List<Loan>> GetLoans()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Loan/all loans");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Loan>>(content);
            }

            throw new Exception("Failed to retrieve borrowers from the API.");
        }

        public async Task<List<Loan>> GetLoansOfToday()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Loan/returns-today");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Loan>>(content);
            }

            throw new Exception("Failed to retrieve borrowers from the API.");
        }

        internal void ModifyBook(Borrowers borrowersToModify)
        {
            throw new NotImplementedException();
        }

        internal static Task GetReturnData()
        {
            throw new NotImplementedException();
        }
    }
}
