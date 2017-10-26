namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using DomainModels.Entities;

    public sealed class RegisteredUsers
    {
        private static readonly Lazy<RegisteredUsers> Lazy =
            new Lazy<RegisteredUsers>(() => new RegisteredUsers(), true);

        /// <summary>
        ///     Search key is User's login
        /// </summary>
        public readonly ConcurrentDictionary<string, User> Users;

        public static RegisteredUsers Instance => Lazy.Value;

        private static readonly string XmlFile = "RegisteredUsers.xml";

        private static readonly object PadLock = new object();

        public void UpdateLocalStorage()
        {
            lock (PadLock)
            {
                if (File.Exists(XmlFile))
                {
                    File.Delete(XmlFile);
                }

                XElement root = new XElement("RegisteredUsers");

                foreach (KeyValuePair<string, User> pair in Users)
                {
                    User user = pair.Value;

                    root.Add(new XElement("User",
                        new XElement("Name", user.Name),
                        new XElement("Pass", user.Pass),
                        new XElement("Login", user.Login)));
                }

                root.Save(XmlFile, SaveOptions.None);
            }
        }


        #region CONSTRUCTORS

        private RegisteredUsers()
        {
            Users = new ConcurrentDictionary<string, User>();

            if (!File.Exists(XmlFile))
            {
                CreateXmlLocalStorage();
            }
            else
            {
                LoadPersistedData();
            }
        }

        private static void CreateXmlLocalStorage()
        {
            XElement root = new XElement("Users");
            root.Save(XmlFile, SaveOptions.None);
        }

        private void LoadPersistedData()
        {
            XElement root = XElement.Load(XmlFile, LoadOptions.PreserveWhitespace);

            foreach (XElement node in root.Descendants("User"))
            {
                string name = node.Element("Name").Value;
                string pass = node.Element("Pass").Value;
                string login = node.Element("Login").Value;

                Users.TryAdd(login, new User
                {
                    Login = login,
                    Name = name,
                    Pass = pass
                });
            }
        }

        #endregion


        public bool TryRegisterUser(User user)
        {
            if (Users.ContainsKey(user.Login))
            {
                return false;
            }

            bool success = Users.TryAdd(user.Login, user);

            if (success)
            {
                PersistUsersData();
            }

            return success;
        }

        private void PersistUsersData()
        {
            lock (PadLock)
            {
                XElement root = new XElement("Users");

                Users.Select(pair => pair.Value)
                    .ToList()
                    .ForEach(regUser =>
                    {
                        root.Add(
                            new XElement("User",
                                new XElement("Name", regUser.Name),
                                new XElement("Pass", regUser.Pass),
                                new XElement("Login", regUser.Login)));
                    });

                root.Save(XmlFile, SaveOptions.None);
            }
        }
    }
}