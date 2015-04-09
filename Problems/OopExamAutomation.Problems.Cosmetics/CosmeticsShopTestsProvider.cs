namespace OopExamAutomation.Problems.CosmeticsShop
{
    using System.Collections.Generic;
    using System.Linq;

    using OopExamAutomation.Engine;
    using System;
    using System.Reflection;

    public class CosmeticsShopTestsProvider : ITestsProvider
    {
        private const string ICategory = "ICategory";
        private const string IProduct = "IProduct";
        private const string IShampoo = "IShampoo";
        private const string IToothpaste = "IToothpaste";
        private const string IShoppingCart = "IShoppingCart";

        public IEnumerable<ITest> GetTests()
        {
            return new List<ITest>()
            {
                // Category
                new PredicateTest("Added class Category", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterface(ICategory);
                    }),
                new PredicateTest("Category has private set Name", 1M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(ICategory, "Name");
                    }),
                new PredicateTest("Category Name cannot be null", 1M, assembly => 
                    {
                        return this.CheckConstructorParameterValidation(assembly, ICategory, null);
                    }),
                new PredicateTest("Category Name cannot be less than 2 symbols", 1M, assembly => 
                    {
                        return this.CheckConstructorParameterValidation(assembly, ICategory, "a");
                    }),
                new PredicateTest("Category Name cannot be more than 15 symbols", 1M, assembly => 
                    {
                        return this.CheckConstructorParameterValidation(assembly, ICategory, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                    }),
                new PredicateTest("Category cannot add null cosmetics", 1M, assembly => 
                    {
                        return this.CheckMethodInvokeValidation(assembly, ICategory, new[] { "aaaaaaaa" }, "AddCosmetics", null);
                    }),
                new PredicateTest("Category cannot remove null cosmetics", 1M, assembly => 
                    {
                        return this.CheckMethodInvokeValidation(assembly, ICategory, new[] { "aaaaaaaa" }, "RemoveCosmetics", null);
                    }),

                // Product
                new PredicateTest("Added class Product", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterface(IProduct);
                    }),
                new PredicateTest("Class Product is abstract", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IProduct).IsAbstract;
                    }),
                new PredicateTest("Product has private set Name", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(IProduct, "Name");
                    }),
                 new PredicateTest("Product Name cannot be null", 1M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, null, "test", 15.0m, genderInstance, new List<string> { "Ivaylo" });
                    }),
                new PredicateTest("Product Name cannot be less than 3 symbols", 1M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "a", "test", 15.0m, genderInstance, new List<string> { "Ivaylo" });
                    }),
                new PredicateTest("Product Name cannot be more than 10 symbols", 1M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "aaaaaaaaaaaaaaaaaa", "test", 15.0m, genderInstance, new List<string> { "Ivaylo" });
                    }),
                new PredicateTest("Product has private set Brand", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(IProduct, "Brand");
                    }),
                new PredicateTest("Product Brand cannot be null", 1M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "test", null, 15.0m, genderInstance, new List<string> { "Ivaylo" });
                    }),
                new PredicateTest("Product Brand cannot be less than 2 symbols", 1M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "test", "a", 15.0m, genderInstance, new List<string> { "Ivaylo" });
                    }),
                new PredicateTest("Product Brand cannot be more than 10 symbols", 1M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "test", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 15.0m, genderInstance, new List<string> { "Ivaylo" });
                    }),
                new PredicateTest("Product has non public set Price", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(IProduct, "Price");
                    }),
                new PredicateTest("Product has non public set Gender", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(IProduct, "Gender");
                    }),

                // Shampoo
                new PredicateTest("Added class Shampoo", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IShampoo).GetInterfaces().All(i => i.Name.Contains(IProduct) || i.Name.Contains(IShampoo));
                    }),
                new PredicateTest("Class Shampoo inherits abstract IProduct", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IShampoo).BaseType.IsAbstract;
                    }),
                new PredicateTest("Class Shampoo invokes base constructor", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IShampoo).BaseType.GetConstructor(Type.EmptyTypes) == null;
                    }),
                new PredicateTest("Product has private set Milliliters", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(IShampoo, "Milliliters");
                    }),
                new PredicateTest("Product has private set Usage", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterfaceWithPrivateProperty(IShampoo, "Usage");
                    }),

                // Toothpaste
                new PredicateTest("Added class Toothpaste", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IToothpaste).GetInterfaces().All(i => i.Name.Contains(IProduct) || i.Name.Contains(IToothpaste));
                    }),
                new PredicateTest("Class Toothpaste inherits abstract IProduct", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IToothpaste).BaseType.IsAbstract;
                    }),
                new PredicateTest("Class Toothpaste invokes base constructor", 2M, assembly => 
                    {
                        return assembly.GetClassImplementingInterface(IToothpaste).BaseType.GetConstructor(Type.EmptyTypes) == null;
                    }),
                new PredicateTest("Class Toothpaste ingridients cannot be less than 4 symbols", 2M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "test", "test", 15.0m, genderInstance, new List<string> { "i" });
                    }),
                new PredicateTest("Class Toothpaste ingridients cannot be more than 12 symbols", 2M, assembly => 
                    {
                        var genderEnum = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("GenderType"));
                        var genderInstance = Activator.CreateInstance(genderEnum);
                        return this.CheckConstructorParameterValidation(assembly, IToothpaste, "test", "test", 15.0m, genderInstance, new List<string> { "ivayloooooooooooooooooooo" });
                    }),

                // Shopping cart
                new PredicateTest("Added class ShoppingCart", 2M, assembly => 
                    {
                        return assembly.HasClassImplementingInterface(IShoppingCart);
                    }),
                new PredicateTest("ShoppingCart cannot add null cosmetics", 1M, assembly => 
                    {
                        return this.CheckMethodInvokeValidation(assembly, IShoppingCart, null, "AddProduct", new object[] {null});
                    }),
                new PredicateTest("ShoppingCart cannot remove null cosmetics", 1M, assembly => 
                    {
                        return this.CheckMethodInvokeValidation(assembly, IShoppingCart, null, "RemoveProduct", new object[] { null });
                    }),
            };
        }

        private bool CheckConstructorParameterValidation(Assembly assembly, string className, params object[] constructorParams)
        {
            var type = assembly.GetClassImplementingInterface(className);
            try
            {
                var categoryInstance = Activator.CreateInstance(type, constructorParams);
            }
            catch
            {
                return true;
            }

            return false;
        }

        private bool CheckMethodInvokeValidation(Assembly assembly, string className, object[] constructorParameters, string methodName, object[] methodParameters)
        {
            var type = assembly.GetClassImplementingInterface(className);
            try
            {
                var typeInstance = Activator.CreateInstance(type, constructorParameters);
                type.GetMethod(methodName).Invoke(typeInstance, methodParameters);
            }
            catch
            {
                return true;
            }

            return false;
        }
    }
}
