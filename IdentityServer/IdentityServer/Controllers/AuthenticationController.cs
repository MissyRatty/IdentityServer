using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //sample returnUrl from OAuthServer: 
        //%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3DclientMvc_id%26redirect_uri%3Dhttps%253A%252F%252Flocalhost%253A44392%252Fsignin-oidc%26response_type%3Dcode%26scope%3Dopenid%2520profile%26code_challenge%3DNDQ5p60X3wabdq9W6hk5LmIVBPDHxzFE0N5-PDCLsM4%26code_challenge_method%3DS256%26response_mode%3Dform_post%26nonce%3D637307873873012452.MDFkYmNlMzMtZTMxOS00MmUyLTkxYTQtODk4MThkY2VkNzM2ZjY5ZmE3MzctNzRkYi00YTRlLTg2NGUtYzhkMDgzMDExOGIx%26state%3DCfDJ8CAN2oTbBBtMjnsGXsYbjRirnG2ylVux1TnrUqNqCAQ43lsWjE4n1-3EpMwj8cm1v60Xthy8Aw7XfyreGHXjBjKKv-90CbHCQVFsZDQNHZO5mioqqkJpLlq5KFZFgZVLgC1jTCXgbr-wkFqZnDvx3p2WmiWV74s5BLLB6UvIVlKKo3MEqr7M-vsibi6uv-I2UoXMWqdp3LTM2Np_MmLyfdHOlUvNbbGC8ouPnYrSjRFj1vDtDqUlFMxOh78mZ0MbfD7MCnXVNBsbz2dOf83uPRhgoZsPnqDNFocE0RQ4dLuAyJLWwLPIXogrCr1qjsDRuynZEBdB7Rz7XqkCpIXNvn5yjWjqEJ7KECzG_5xRnNX3Iu4_FbibMojbbowWODF0DA20mj3xWiWxOx8SpnZ-DIA%26x-client-SKU%3DID_NETSTANDARD2_0%26x-client-ver%3D5.5.0.0
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel() 
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                return Redirect(login.ReturnUrl);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = new IdentityUser(register.Username);
            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(register.ReturnUrl);
            }

            return View();
        }
    }
}
