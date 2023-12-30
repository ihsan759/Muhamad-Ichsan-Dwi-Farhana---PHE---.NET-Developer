using Client.Contracts.Data;
using Client.Utilities.Handlers;
using Client.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Repositories.Data
{
    public class AccountRepository : IAccountRepository
    {
        protected readonly string _request;
        protected readonly HttpClient _httpClient;
        protected readonly IHttpContextAccessor _contextAccessor;

        public AccountRepository(string request = "Account/")
        {
            _contextAccessor = new HttpContextAccessor();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7124/api/")
            };
            this._request = request;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext?.Session.GetString("JWToken"));
        }

        public async Task<ResponseHandler<string>> Login(LoginVM loginVM)
        {
            ResponseHandler<string> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");
            using (var response = _httpClient.PostAsync(_request + "Login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<string>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<GetProfileVM>> Get()
        {
            ResponseHandler<GetProfileVM> entityVM = null;

            using (var response = await _httpClient.GetAsync(_request + "Get"))
            {
                string responseApi = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<GetProfileVM>>(responseApi);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<GetProfileVM>> Update([FromForm] GetProfileVM updateAccountVM)
        {
            ResponseHandler<GetProfileVM> entityVM = null;

            using (var formData = new MultipartFormDataContent())
            {
                if (updateAccountVM.Image != null)
                {
                    var streamContent = new StreamContent(updateAccountVM.Image.OpenReadStream())
                    {
                        Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "Image", FileName = updateAccountVM.Image.FileName } }
                    };
                    formData.Add(streamContent, "Image", updateAccountVM.Image.FileName);
                }

                formData.Add(new StringContent(updateAccountVM.Id), "Id");
                formData.Add(new StringContent(updateAccountVM.Email), "Email");
                formData.Add(new StringContent(updateAccountVM.Name), "Name");
                formData.Add(new StringContent(updateAccountVM.PhoneNumber), "PhoneNumber");
                if (!string.IsNullOrEmpty(updateAccountVM.Password))
                {
                    formData.Add(new StringContent(updateAccountVM.Password), "Password");
                }
                if (!string.IsNullOrEmpty(updateAccountVM.rePassword))
                {
                    formData.Add(new StringContent(updateAccountVM.rePassword), "rePassword");
                }

                using (var response = await _httpClient.PutAsync(_request + "Update", formData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entityVM = JsonConvert.DeserializeObject<ResponseHandler<GetProfileVM>>(apiResponse);
                }
            }

            return entityVM;
        }

        public async Task<ResponseHandler<RegisterVM>> Register([FromForm] RegisterVM registerVM)
        {
            ResponseHandler<RegisterVM> entityVM = null;

            using (var formData = new MultipartFormDataContent())
            {
                if (registerVM.Image != null)
                {
                    var streamContent = new StreamContent(registerVM.Image.OpenReadStream())
                    {
                        Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "Image", FileName = registerVM.Image.FileName } }
                    };
                    formData.Add(streamContent, "Image", registerVM.Image.FileName);
                }
                if (!string.IsNullOrEmpty(registerVM.Email))
                {
                    formData.Add(new StringContent(registerVM.Email), "Email");
                }
                if (!string.IsNullOrEmpty(registerVM.Name))
                {
                    formData.Add(new StringContent(registerVM.Name), "Name");
                }
                if (!string.IsNullOrEmpty(registerVM.PhoneNumber))
                {
                    formData.Add(new StringContent(registerVM.PhoneNumber), "PhoneNumber");
                }

                if (!string.IsNullOrEmpty(registerVM.Password))
                {
                    formData.Add(new StringContent(registerVM.Password), "Password");
                }
                if (!string.IsNullOrEmpty(registerVM.ConfirmPassword))
                {
                    formData.Add(new StringContent(registerVM.ConfirmPassword), "rePassword");
                }

                using (var response = await _httpClient.PostAsync(_request + "Register", formData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entityVM = JsonConvert.DeserializeObject<ResponseHandler<RegisterVM>>(apiResponse);
                }
            }

            return entityVM;
        }
    }
}
