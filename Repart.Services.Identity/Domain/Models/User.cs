using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Repart.Common.Exceptions;
using Repart.Services.Identity.Domain.Services;

namespace Repart.Services.Identity.Domain.Models
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public string Name { get; protected set; }
        public List<Guid> Roles { get; protected set;}
        public DateTime CreatedAt { get; protected set; }

        protected User(){}

        public User(string email, string name, IEnumerable<Guid> roles)
        {
            if(string.IsNullOrEmpty(email))
                throw new RepartException("empty_user_email",
                    $"L'adresse courriel ne peut pas être vide.");
            if(!Regex.IsMatch(email, 
                @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + 
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", 
                RegexOptions.IgnoreCase))
                throw new RepartException("invalid_user_email",
                    $"L'adresse courriel n'est pas dans un format valide.");
            
            if(string.IsNullOrEmpty(name))
                throw new RepartException("empty_user_name",
                    $"Le nom de l'utilisateur ne peut pas être vide.");

            Id = Guid.NewGuid();
            Email = email.ToLowerInvariant();
            Name = name;
            Roles = new List<Guid>(roles);
            CreatedAt = DateTime.UtcNow;
        }

        public User AddRole(Guid role)
        {
            if(role == null)
                throw new RepartException("user_role_invalid",
                    $"Aucun role mentionné.");

            if (Roles.Contains(role))
                throw new RepartException("user_already_have_role",
                    $"L'utilisateur détient déjà le role : {role}.");

            Roles.Add(role);

            return this;
        }

        public User RemoveRole(Guid role)
        {
            if(role == null)
                throw new RepartException("user_role_invalid",
                    $"Aucun role mentionné.");

            if (Roles.Contains(role))
                throw new RepartException("user_not_have_role",
                    $"L'utilisateur ne détient pas le role : {role}.");

            Roles.Remove(role);

            return this;
        }

        public void SetPassword(string password, IEncrypter encrypter)
        {
            if(string.IsNullOrEmpty(password))
                throw new RepartException("empty_user_password",
                    $"Le mot de passe ne peut pas être vide.");

            Salt = encrypter.GetSalt(password);
            Password = encrypter.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, IEncrypter encrypter)
            => Password.Equals(encrypter.GetHash(password, Salt));
    }
}
