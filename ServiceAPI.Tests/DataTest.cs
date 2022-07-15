namespace ServiceAPI.Tests;

public class DataTest
{
    public static List<Service> TestData()
    {
        return new List<Service>
        {
            new Service()
            {
                Id = Guid.Parse("1643b1ae-cf06-4af3-bd01-9d2ae1ea0529"),
                Message = "Message 1",
                Status = "Processed",
                CreatedDate = DateTime.Now
            }
        };
    }

    public static Service PostMessageData()
    {
        return new Service()
        {
            Id = Guid.NewGuid(),
            Message = "qwoiejqwoiej ",
            Status = "In Process",
            CreatedDate = DateTime.Now
        };
    }

    public static Service ExpiredData()
    {
        return new Service()
        {
            Message = "Message 1",
            Status = "Просрочено",
            CreatedDate = new DateTime(2021, 07, 06, 14, 00, 00)

        };
    }

    public static List<Service> ProcessingTripleMessage()
    {
        return new List<Service>
        {
            new Service()
            {
                Id = Guid.Parse("1643b1ae-cf06-4af3-bd01-9d2ae1ea0529"),
                Message = "Message 1",
                Status = "Processed",
                CreatedDate = DateTime.Now
            },
            new Service()
            {
                Id = Guid.Parse("2643b1ae-cf06-4af3-bd01-9d2ae1ea0529"),
                Message = "Message 2",
                Status = "Processed",
                CreatedDate = DateTime.Now
            },
            new Service()
            {
                Id = Guid.Parse("3643b1ae-cf06-4af3-bd01-9d2ae1ea0529"),
                Message = "Message 3",
                Status = "Processed",
                CreatedDate = DateTime.Now
            }
        };
    }
}
