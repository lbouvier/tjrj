using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TJRJ.Web.Data;
using TJRJ.Web.Entities;
using TJRJ.Web.Extensions;
using TJRJ.Web.Intefaces;
using TJRJ.Web.Repository;
using TJRJ.Web.Services;
using TJRJ.Web.Validations;

namespace TJRJ.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly BaseRepository<Configuracao> _repositorioConfiguracao;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClienteRepositorio _clienteRepositorio;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IWebHostEnvironment hostingEnvironment,
            BaseRepository<Configuracao> repositorioConfiguracao,
            IClienteRepositorio clienteRepositorio)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _repositorioConfiguracao = repositorioConfiguracao;
            _clienteRepositorio = clienteRepositorio;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

               
                var user = new Cliente(Input.Name, Input.LastName, Input.Email, Input.CpfCnpj, Input.TelefoneCelular, Input.TelefoneResidencial);

                _userManager.Options.SignIn.RequireConfirmedEmail = true;

                var roles = (await _repositorioConfiguracao.SelectAsync()).FirstOrDefault();
                var role = "Usuario";
                //validar o servico de crud e vincular role e usuario
                if (roles.EmailsAdministradores.Contains(Input.Email))
                {
                    role = "Administrador";
                }

                var existeCpf = await  _clienteRepositorio.BuscaCpfCnpj(user.CpfCnpj);
                if (existeCpf != null)
                {
                    ModelState.AddModelError("CpfCnpj", "CPF / CNPJ já cadastrado.");
                    return Page();
                }

                var existeEmail = await _clienteRepositorio.BuscaEmail(user.Email);
                if (existeEmail != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado.");
                    return Page();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var addingRole = await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //template email
                    //configurar templates na tabela configuração
                    //Configuracao.TemplateConfirmacaoRegistro
                    var templatePath = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Templates/Email/")).Where(x => x.Contains("ConfirmRegistration")).FirstOrDefault();
                    var imgLogoPath = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "img/")).Where(x => x.Contains("logo-TJRJ")).FirstOrDefault();
                    var template = "";
                    if (!string.IsNullOrEmpty(templatePath))
                    {
                        using (var reader = new StreamReader(templatePath))
                        {
                            template = reader.ReadToEnd();
                        }
                    }

                    byte[] imageArray = System.IO.File.ReadAllBytes(imgLogoPath);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    var tagsReplace = new Dictionary<string, string>
                    {
                        {"logo", base64ImageRepresentation},
                        {"usuarioNome", user.Nome },
                        {"urlConfirRegistration", callbackUrl },
                    };

                    template = template.ReplaceTemplate(tagsReplace);

                    var _emailSender = new EmailSender();
                    await _emailSender.SendEmailAsync(roles,
                                                      Input.Email,
                                                      "TJRJ TJRJ - Confirmação de Email",
                                                      template);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }

    
    public class InputModel
    {

        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "CPF / CNPJ")]
        [CpfCnpj]
        [StringLength(14, ErrorMessage = "O {0} precisa conter no minino {2} e no máximo {1} caracteres.", MinimumLength = 11)]
        public string CpfCnpj { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Celular (DDD)")]
        [StringLength(11, ErrorMessage = "O {0} telefone precisa conter {2} digitos, incluindo o DDD {1}.", MinimumLength = 11)]
        public string TelefoneCelular { get; set; }
        [Display(Name = "Telefone Residencial")]
        public string TelefoneResidencial { get; set; }
        [Display(Name = "Telefone Comercial")]
        public string TelefoneComercial { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} precisa conter entre {2} e no maximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "A confirmação de senha não está igual. Por favor verifique")]
        public string ConfirmPassword { get; set; }
    }
}
