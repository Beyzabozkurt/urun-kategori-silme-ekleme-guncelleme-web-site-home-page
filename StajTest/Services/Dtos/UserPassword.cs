using System.ComponentModel.DataAnnotations;

namespace StajTest.Services.Dtos
{
    public class UserPassword : IValidatableObject
    {
        /// <summary>
        /// Old Password
        /// </summary>
        public string o { get; set; }

        /// <summary>
        ///  New Password
        /// </summary>
        public string n { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (o.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("Boş Olamaz!", new[] { "o" });
            }

            if (n.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("Boş Olamaz!", new[] { "n" });
                //yield return new ValidationResult("Şifre Boş Olamaz!", new[] { "Password" });
            }
        }
    }
}
