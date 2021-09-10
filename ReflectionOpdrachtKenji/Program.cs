using System;
using System.Dynamic;
using System.Linq;

namespace ReflectionOpdrachtKenji
{
    public class Program
    {
        // ((System.Reflection.RuntimeAssembly[])AppDomain.CurrentDomain.GetAssemblies())[1]
        public static void Main(string[] args)
        {
            AssemblyService AS = new AssemblyService();

            AS.GetAllAssemblyItems();

            foreach (var item in AS.AllAssemblyItems)
            {
                Console.WriteLine(item);
            }

            TestingUserService(AS);

            TestingUserManagerService(AS);
        }

        private static void TestingUserManagerService(AssemblyService AS)
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~ Testing UserManager Service ~~~~~~~~~~~~~~~~~~");

            var UserManager = AS.Assembly.GetTypes().Where(a => a.Name == "UserManager").FirstOrDefault();
            var instanceUserManager = Activator.CreateInstance(UserManager);

            var getUserCount = UserManager.GetMethod("get_UserCount");
            var logOn = UserManager.GetMethod("Logon");
            var logOff = UserManager.GetMethod("LogOff");

            var usern = "henk";
            var passw = "henk123";

            object[] login = new object[] { usern, passw };
            var resultLogOn = logOn.Invoke(instanceUserManager, login);
            Console.WriteLine($"Entered username = {usern}");
            Console.WriteLine($"Entered password = {passw}");
            Console.WriteLine($"Login = {resultLogOn}");
            Console.WriteLine("");

            Console.WriteLine($"User count = {getUserCount.Invoke(instanceUserManager, null)}");
            Console.WriteLine("");

            object[] logoff = new object[] { usern };
            var resultLoglogoff = logOff.Invoke(instanceUserManager, logoff);
            Console.WriteLine($"Entered username = {usern}");
            Console.WriteLine($"Result Logoff = {resultLoglogoff}");
            Console.WriteLine("");

            Console.WriteLine($"User count = {getUserCount.Invoke(instanceUserManager, null)}");
        }

        private static void TestingUserService(AssemblyService AS)
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~ Testing User Service ~~~~~~~~~~~~~~~~~~");

            var User = AS.Assembly.GetTypes().Where(a => a.Name == "User").FirstOrDefault();
            var instanceUser = Activator.CreateInstance(User);

            var setUsername = User.GetMethod("set_Username");
            var setPassword = User.GetMethod("set_Password");
            var getUsername = User.GetMethod("get_Username");
            var getPassword = User.GetMethod("get_Password");

            var usern = "henk";
            var passw = "henk123";

            Console.WriteLine($"Entered username = {usern}");
            Console.WriteLine($"Entered password = {passw}");
            Console.WriteLine("");

            object[] username = new object[] { usern };
            object[] password = new object[] { passw };

            setUsername.Invoke(instanceUser, username);
            setPassword.Invoke(instanceUser, password);

            var testUsernameGet = getUsername.Invoke(instanceUser, null);
            var testPasswordGet = getPassword.Invoke(instanceUser, null);

            Console.WriteLine($"Recieved username = {testUsernameGet}");
            Console.WriteLine($"Recieved password = {testPasswordGet}");
            Console.WriteLine("");

            if (usern == testUsernameGet.ToString())
                Console.WriteLine("Username is Correct!");
            else
                Console.WriteLine("Incorrect username!");
            if (passw == testPasswordGet.ToString())
                Console.WriteLine("Password is Correct!");
            else
                Console.WriteLine("Incorrect password!");

            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
