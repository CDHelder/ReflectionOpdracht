using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionOpdrachtKenji
{
    public class AssemblyService
    {
        public List<string> AllAssemblyItems { get; set; }
        public List<Type> AllAssemblyTypes { get; set; }
        public Assembly Assembly { get; set; }

        public AssemblyService()
        {
            AllAssemblyItems = new();
        }

        public void GetAllAssemblyItems()
        {
            Assembly = Assembly.LoadFile(@"C:\Users\CDHel\Desktop\Reflection opdracht\ReflectThis");
            AllAssemblyTypes = Assembly.GetTypes().Where(type => type.IsClass).ToList();

            FormatToListString(AllAssemblyTypes);
        }

        private void FormatToListString(List<Type> items)
        {
            foreach (var item in items)
            {
                string allInfo = null;
                var classAccesModifier = item.Attributes.HasFlag(TypeAttributes.Public) ? "Public" : "Private";
                var baseType = item.BaseType.ToString().Replace("System.", "");

                allInfo += $"{classAccesModifier} Class: {item.Namespace} ({baseType}) {item.Name}: \n\n";

                if (item.GetFields().Length > 0)
                    allInfo += GetFields(item);

                if (item.GetConstructors().Length > 0)
                    allInfo += GetConstructors(item);

                if (item.GetProperties().Length > 0)
                    allInfo += GetProperties(item);
                if(item.GetMethods().Length > 0)
                    allInfo += GetMethods(item, allInfo);

                AllAssemblyItems.Add(allInfo);
            }
        }

        private string GetMethods(Type item, string allInfo)
        {
            string info = null;
            foreach (var methodInfo in item.GetMethods())
            {
                var accesModi = Accessmodifier(methodInfo);
                var parameters = Parameters(methodInfo);
                var returnType = methodInfo.ReturnType.ToString().Replace("System.", "");

                info += $"   {accesModi} {returnType} {methodInfo.Name}({parameters}) \n";
            }
            info += "\n";
            return info;
        }

        private string GetProperties(Type item)
        {
            string info = null;
            foreach (var propertyInfo in item.GetProperties())
            {
                var getAndSet = GetSetInformation(propertyInfo);
                var returnType = propertyInfo.PropertyType.ToString().Replace("System.", "");

                info += $"   {returnType} {propertyInfo.Name} {getAndSet};\n";
            }
            info += "\n";
            return info;
        }

        private string GetSetInformation(PropertyInfo propertyInfo)
        {
            var get = propertyInfo.GetGetMethod() != null ? $"{{{Accessmodifier(propertyInfo.GetGetMethod())} get;" : "{get;";
            var set = propertyInfo.GetSetMethod() != null ? $"{Accessmodifier(propertyInfo.GetSetMethod())} set;}}" : "set;}";
            return $"{get} {set}";
        }

        private string GetConstructors(Type item)
        {
            string info = null;
            foreach (var constructor in item.GetConstructors())
            {
                var parameters = Parameters(constructor);
                info += $"   {item.Name}({parameters})";
            }
            info += "\n";
            return info;
        }

        private string GetFields(Type item)
        {
            string info = null;
            foreach (var field in item.GetFields())
            {
                var accesModifier = Accessmodifier(field);
                var fieldType = field.FieldType.ToString().Replace("System.", "");

                info += $"   {accesModifier} {fieldType} {field.Name};";
            }
            info += "\n";
            return info;
        }

        private string Parameters(ConstructorInfo constructor)
        {
            string allParametersStringFormat = null;
            var allParameters = constructor.GetParameters();

            foreach (var parameter in allParameters)
            {
                var isOut = parameter.IsOut ? "out " : "";
                var hasDefaultValue = parameter.HasDefaultValue ? $"= {parameter.DefaultValue} " : "";
                var parameterType = parameter.ParameterType.ToString().Replace("System.", "");

                allParametersStringFormat += $"{isOut}{parameterType} {parameter.Name}{hasDefaultValue}, ";
            }

            return allParametersStringFormat;
        }

        public string Parameters(MethodInfo methodInfo)
        {
            string allParametersStringFormat = null;
            var allParameters = methodInfo.GetParameters();

            foreach (var parameter in allParameters)
            {
                var isOut = parameter.IsOut ? "out " : "";
                var hasDefaultValue = parameter.HasDefaultValue ? $"= {parameter.DefaultValue} " : "";
                var parameterType = parameter.ParameterType.ToString().Replace("System.", "");

                allParametersStringFormat += $"{isOut}{parameterType} {parameter.Name}{hasDefaultValue}, ";
            }

            return allParametersStringFormat;
        }

        public string Accessmodifier(MethodInfo methodInfo)
        {
            if (methodInfo.IsPrivate)
                return "Private";
            if (methodInfo.IsFamily)
                return "Protected";
            if (methodInfo.IsFamilyOrAssembly)
                return "ProtectedInternal";
            if (methodInfo.IsAssembly)
                return "Internal";
            if (methodInfo.IsPublic)
                return "Public";
            throw new ArgumentException("Did not find access modifier", "methodInfo");
        }

        public string Accessmodifier(FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPrivate)
                return "Private";
            if (fieldInfo.IsFamily)
                return "Protected";
            if (fieldInfo.IsFamilyOrAssembly)
                return "ProtectedInternal";
            if (fieldInfo.IsAssembly)
                return "Internal";
            if (fieldInfo.IsPublic)
                return "Public";
            throw new ArgumentException("Did not find access modifier", "methodInfo");
        }
    }
}
