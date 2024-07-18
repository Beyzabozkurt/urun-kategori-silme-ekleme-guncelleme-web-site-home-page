using System.ComponentModel.DataAnnotations;

namespace StajTest.Services.Dtos
{
    public class LoginInput : IValidatableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        

        /// <summary>
        ///  
        /// </summary>
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Username.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("Kullanıcı Boş Olamaz!", new[] { "Username" });
            }

            if (Password.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("Şifre Boş Olamaz!", new[] { "Password" });
                //yield return new ValidationResult("Şifre Boş Olamaz!", new[] { "Password" });
            }
        }
    }
}
