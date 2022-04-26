using System;
using System.Linq;
using System.Text;
using IUGOCare.Application.Common.Interfaces;

namespace IUGOCare.Infrastructure.Services
{
    public class ActivationCodeService : IActivationCode
    {
        public string GenerateNewActivationCode()
        {
            StringBuilder urlsafe = new StringBuilder(62);
            Random rand = new Random();

            // Loop through the ranges of numerical values where the URL safe characters are located and add that to the urlsafe where character is a letter or digit
            Enumerable.Range(48, 75)
              .Where(i => char.IsLetterOrDigit((char)i))
              .OrderBy(o => rand.Next())
              .ToList()
              .ForEach(i => urlsafe.Append(Convert.ToChar(i))); // Store each char into urlsafe

            // Generate a random string of 20 characters long
            return urlsafe.ToString().Substring(rand.Next(0, urlsafe.Length - 20), 20);
        }
    }
}
