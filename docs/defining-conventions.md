If you want to define your own conventions, it is really easy. 

### Step 1
Create a new class, inheriting from `IConvention<TData>`, lets use the **must have default constructor** convention as an example

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public void Execute(Types data, IConventionResult result)
        {
        
        }
    }
    
### Step 2
Write the convention logic

    var typesWithoutDefaultCtor = data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false);
    result.Is("Types must have a default constructor", typesWithoutDefaultCtor);
    
### Final result

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public void Execute(Types data, IConventionResult result)
        {
            var typesWithoutDefaultCtor = data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false);
            result.Is("Types must have a default constructor", typesWithoutDefaultCtor);
        }
    }
    
## IConventionResult
Currently convention tests supports two types of convention results, the one we have just seen is a normal result. The other type is a symmetric result, which is the result for a symmetric convention.

An example of a symmetric convention is `ClassTypeHasSpecificNamespace`. It can verify a particular class type (dto, domain object, event handler) lives in a certain namespace, but it will also verify that ONLY that class type lives in that namespace. `new ClassTypeHasSpecificNamespace(t => t.Name.EndsWith("Dto"), "TestAssembly.Dtos", "Dto")`

The result will look like this:

    result.IsSymmetric(
        string.Format("{0}s must be under the '{1}' namespace", classType, namespaceToCheck),
        string.Format("Non-{0}s must not be under the '{1}' namespace", classType, namespaceToCheck),
        classIsApplicable,
        TypeLivesInSpecifiedNamespace,
        data.TypesToVerify);

See [Symmetric Conventions](SymmetricConventions.html) for more information.    