// See https://aka.ms/new-console-template for more information

public class Program
{
    public static void Main(string[] args)
    {
        IEnumerable<Type> demos = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(t => t.GetInterfaces().Contains(typeof(IDemoSheet)));

        foreach (Type demoClass in demos)
        {
            IDemoSheet? demo = Activator.CreateInstance(demoClass) as IDemoSheet;
            demo?.RunDemo();
        }
    }
}
