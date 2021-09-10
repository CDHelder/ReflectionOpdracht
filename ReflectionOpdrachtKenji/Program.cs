using System;
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

            //TODO: Spreek objecten aan en bewijs
        }
    }
}
