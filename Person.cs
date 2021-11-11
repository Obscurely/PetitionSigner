using System;
using System.Text;
using System.Globalization;

namespace PetitionSpammer
{
    public class Person
    {
        private Random _rand;
        private string[] _firstNames;
        private string[] _lastNames;
        private string[] _emailDomains = new string[] { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "protonmail.com" };
        private string[] _emailSeparators = new string[] { "", ".", "-", "-" };
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly string _county;
        
        private Random Rand { get { return _rand; } set { _rand = value; } }
        private string[] FirstNames { get { return _firstNames; } }
        private string[] LastNames { get { return _lastNames; } }
        private string[] EmailDomains { get { return _emailDomains; } }
        private string[] EmailSeparators { get { return _emailSeparators; } }
        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public string Email { get { return _email; } }
        public string County { get { return _county; } }

        /// <summary>
        /// Creates a person object by choosing a random firstName, lastName, a county and a generated email, based on the information in the given lists.
        /// An email would look like this firstName/lastName$SEPARATORlastName/firstName(opt->)$SEPARATORrandomNumber(in range 0-9999)@randomDomain where firstName and lastName
        /// have each a 10% to be cut in half, a 50% chance for firstName to be first and 50% for lastName to be first, a 50% to include a number, $SEPARATOR
        /// can be one of these { "", ".", "-", "-" } each having a 25% chance and a randomDomain from these { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "protonmail.com" }
        /// each having 20% chance to be in the email.
        /// The firstName and lastName have each a 20% chance of being written in lower case and a 33.33% (1 in 3) chance to have diacritics if they are written with.
        /// The county is simply selected random from the list.
        /// This object can be used to fill in forms with information that looks legit and not spam.
        /// </summary>
        /// <param name="firstNames">A list of first names you want to use.</param>
        /// <param name="lastNames">A list of last names you want to use.</param>
        /// <param name="counties">A list of counties you want to use.</param>
        public Person(string[] firstNames, string[] lastNames, string[] counties)
        {
            _firstNames = firstNames;
            _lastNames = lastNames;

            Rand = new Random(Guid.NewGuid().GetHashCode()); // init new random with strong seed generation.
            bool useDiacritics = Rand.Next(3) == 1;
            bool smallLetterFirstName = Rand.Next(5) == 1;
            bool smallLetterLastName = Rand.Next(5) == 1;

            if (!useDiacritics)
            {
                if (smallLetterFirstName && smallLetterLastName)
                {
                    _firstName = RemoveDiacritics(GetRandomFirstName()).ToLower();
                    _lastName = RemoveDiacritics(GetRandomLastName()).ToLower();
                }
                else if (smallLetterFirstName && !smallLetterLastName)
                {
                    _firstName = RemoveDiacritics(GetRandomFirstName()).ToLower();
                    _lastName = RemoveDiacritics(GetRandomLastName());
                }
                else if (!smallLetterFirstName && smallLetterLastName)
                {
                    _firstName = RemoveDiacritics(GetRandomFirstName());
                    _lastName = RemoveDiacritics(GetRandomLastName()).ToLower();
                }
                else 
                {
                    _firstName = RemoveDiacritics(GetRandomFirstName());
                    _lastName = RemoveDiacritics(GetRandomLastName());
                }
            }
            else
            {
                if (smallLetterFirstName && smallLetterLastName)
                {
                    _firstName = GetRandomFirstName().ToLower();
                    _lastName = GetRandomLastName().ToLower();
                }
                else if (smallLetterFirstName && !smallLetterLastName)
                {
                    _firstName = GetRandomFirstName().ToLower();
                    _lastName = GetRandomLastName();
                }
                else if (!smallLetterFirstName && smallLetterLastName)
                {
                    _firstName = GetRandomFirstName();
                    _lastName = GetRandomLastName().ToLower();
                }
                else 
                {
                    _firstName = GetRandomFirstName();
                    _lastName = GetRandomLastName();
                }
            }

            _email = GenEmail(_firstName, _lastName);
            _county = counties[Rand.Next(counties.Length)];
        }

        /// <summary>
        /// Removes any diacritics, no matter the language
        /// </summary>
        /// <param name="text">The text to remove the diacritics from.</param>
        /// <returns>The given text with the diacritics removed.</returns>
        private static string RemoveDiacritics(string text) 
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Gets a random first name from the first name list.
        /// </summary>
        /// <returns>A random first name from the first name list.</returns>
        private string GetRandomFirstName()
        {
            Rand = new Random(Guid.NewGuid().GetHashCode()); // init new random with strong seed generation.
            return FirstNames[Rand.Next(FirstNames.Length)];
        }

        /// <summary>
        /// Gets a random last name from the last name list.
        /// </summary>
        /// <returns>A random last name from the last name list.</returns>
        private string GetRandomLastName()
        {
            Rand = new Random(Guid.NewGuid().GetHashCode()); // init new random with strong seed generation.
            return LastNames[Rand.Next(LastNames.Length)];
        }

        /// <summary>
        /// Gets a random email separator from this list { "", ".", "-", "-" }.
        /// </summary>
        /// <param name="random">A Random object instance.</param>
        /// <returns>A random email separator.</returns>
        private string GetEmailSep(Random random)
        {
            return EmailSeparators[random.Next(EmailSeparators.Length)];
        }

        /// <summary>
        /// Gets a random email domain from this list { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "protonmail.com" }.
        /// </summary>
        /// <param name="random">A Random object instance.</param>
        /// <returns>A random email domain.</returns>
        private string GetEmailDomain(Random random)
        {
            return EmailDomains[random.Next(EmailDomains.Length)];
        }

        /// <summary>
        /// Generates a email that tries to look as legit as possible using the given first name and last name.
        /// An email would look like this firstName/lastName$SEPARATORlastName/firstName(opt->)$SEPARATORrandomNumber(in range 0-9999)@randomDomain where firstName and lastName
        /// have each a 10% to be cut in half, a 50% chance for firstName to be first and 50% for lastName to be first, a 50% to include a number, $SEPARATOR
        /// can be one of these { "", ".", "-", "-" } each having a 25% chance and a randomDomain from these { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "protonmail.com" }
        /// each having 20% chance to be in the email.
        /// </summary>
        /// <param name="firstName">The first name to use to generate the email.</param>
        /// <param name="lastName">The last name to use to generate the email.</param>
        /// <returns>A generated email trying to look as legit as possible.</returns>
        private string GenEmail(string firstName, string lastName)
        {
            Rand = new Random(Guid.NewGuid().GetHashCode());

            firstName = RemoveDiacritics(firstName).ToLower();
            lastName = RemoveDiacritics(lastName).ToLower();

            string emailDomain = EmailDomains[Rand.Next(EmailDomains.Length)];
            bool useDigits = Rand.Next(2) == 1;
            bool firstNameFirst = Rand.Next(2) == 1;
            bool cutHalfOfFirstName = Rand.Next(10) == 1;
            bool cutHalfOfLastName = Rand.Next(10) == 1;

            string email = "";
            if (!cutHalfOfFirstName && !cutHalfOfLastName)
            {
                if (useDigits && firstNameFirst)
                {
                    email = firstName + GetEmailSep(Rand) + lastName + GetEmailSep(Rand) + Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (useDigits && !firstNameFirst)
                {
                    email = lastName + GetEmailSep(Rand) + firstName + GetEmailSep(Rand) + Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && firstNameFirst)
                {
                    email = firstName + GetEmailSep(Rand) + lastName + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && !firstNameFirst)
                {
                    email = lastName + GetEmailSep(Rand) + firstName + "@" + GetEmailDomain(Rand);
                }
            }
            else if (cutHalfOfFirstName && !cutHalfOfLastName)
            {
                if (useDigits && firstNameFirst)
                {
                    email = firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + lastName + GetEmailSep(Rand) + Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (useDigits && !firstNameFirst)
                {
                    email = lastName + GetEmailSep(Rand) + firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && firstNameFirst)
                {
                    email = firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + lastName + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && !firstNameFirst)
                {
                    email = lastName + GetEmailSep(Rand) + firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + "@" + GetEmailDomain(Rand);
                }
            }
            else if (!cutHalfOfFirstName && cutHalfOfLastName)
            {
                if (useDigits && firstNameFirst)
                {
                    email = firstName + GetEmailSep(Rand) + lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (useDigits && !firstNameFirst)
                {
                    email = lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + firstName + GetEmailSep(Rand) + Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && firstNameFirst)
                {
                    email = firstName + GetEmailSep(Rand) + lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && !firstNameFirst)
                {
                    email = lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + firstName + "@" + GetEmailDomain(Rand);
                }
            }
            else if (cutHalfOfFirstName && cutHalfOfLastName)
            {
                if (useDigits && firstNameFirst)
                {
                    email = firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + 
                                Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (useDigits && !firstNameFirst)
                {
                    email = lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + 
                                Rand.Next(9999 + 1) + "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && firstNameFirst)
                {
                    email = firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + 
                                "@" + GetEmailDomain(Rand);
                }
                else if (!useDigits && !firstNameFirst)
                {
                    email = lastName.Split(lastName[lastName.Length / 2])[Rand.Next(2)] + GetEmailSep(Rand) + firstName.Split(firstName[firstName.Length / 2])[Rand.Next(2)] + 
                                "@" + GetEmailDomain(Rand);
                }
            }

            return email;
        }
    }
}