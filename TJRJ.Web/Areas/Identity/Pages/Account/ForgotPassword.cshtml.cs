using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TJRJ.Web.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using TJRJ.Web.Repository;
using TJRJ.Web.Entities;

namespace TJRJ.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BaseRepository<Configuracao> _configuracaoRepository;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IWebHostEnvironment hostingEnvironment, BaseRepository<Configuracao> configuracaoRepository)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
            _configuracaoRepository = configuracaoRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                //template email
                //configurar templates na tabela configuração
                //Configuracao.TemplateConfirmacaoRegistro
                var templatePath = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Templates/Email/")).Where(x => x.Contains("ChangePassword")).FirstOrDefault();
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
                        {"usuarioEmail", user.Email },
                        {"urlConfirRegistration", callbackUrl },
                    };

                template = template.ReplaceTemplate(tagsReplace);
                var configs = (await _configuracaoRepository.SelectAsync()).FirstOrDefault();
                var _emailSender = new EmailSender();
                await _emailSender.SendEmailAsync(configs,
                                                  Input.Email,
                                                  "TJRJ TJRJ - Redefinição de Senha",
                                                  template);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
