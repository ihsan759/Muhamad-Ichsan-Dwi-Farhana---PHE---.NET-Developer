using Client.Utilities.Handlers;
using Client.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace Client.Contracts.Data
{
    public interface IAccountRepository
    {
        Task<ResponseHandler<string>> Login(LoginVM loginVM);
        Task<ResponseHandler<GetProfileVM>> Get();
        Task<ResponseHandler<GetProfileVM>> Update([FromForm] GetProfileVM updateAccountVM);
        Task<ResponseHandler<RegisterVM>> Register([FromForm] RegisterVM registerVM);
    }
}
