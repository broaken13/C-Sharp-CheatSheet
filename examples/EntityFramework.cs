
using Microsoft.EntityFrameworkCore;

public class EFDemo : IDemoSheet
{
    public void RunDemo()
    {
        Console.WriteLine("For more information, visit https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli");

        var db = new BloggingContext();

        Console.WriteLine($"DB path: {db.DBPath}");

        Console.WriteLine("Adding to database...");
        db.Add(new Blog { Url = "https://www.stevejgordon.co.uk/" });
        db.SaveChanges();

        Console.WriteLine("And now retrieving...");
        var blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();

        Console.WriteLine($"Retrieved blog with url {blog.Url}");

        Console.WriteLine("Updating url....");
        blog.Url = "https://devblogs.microsoft.com/dotnet";
        blog.Posts.Add(
                new Post
                {
                    Title = "New Blog Post!",
                    Content = "Here is the content of my new blog post"
                });
        db.SaveChanges();


        Console.WriteLine("And now to delete...");
        db.Remove(blog);
        db.SaveChanges();

    }
}

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public string DBPath { get; }
    public BloggingContext()
    {
        DBPath = Path.Join(Path.GetFullPath("."), "blogging.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DBPath}");
}

public class Blog
{
    public int BlogId { get; set; }
    public string? Url { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
