using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ServiceAPI
{
    public class MessageBusWrapper : IMessageBusWrapper
    {
        //private readonly ApplicationDbContext _context;
        private readonly IRabbitMqService _service;

        public MessageBusWrapper(IRabbitMqService service)
        {
            _service = service;
        }

        public string Received(Guid id)
        {
            try
            {
                using var context = new ApplicationDbContext();
                var list = context.Messages.ToList();

                var status = list
                        .FirstOrDefault(x => x.Id == id)
                        .Status;

                return status;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public Guid Send(Service service)
        {
            using var context = new ApplicationDbContext();

            var data = new Service()
            {
                Id = Guid.NewGuid(),
                Message = service.Message,
                Status = "Processed",
                CreatedDate = DateTime.Now,
            };

            context.Add(data);
            context.SaveChanges();

            _service.ConvertToJson(data);

            return data.Id;
        }

        public void ExceptionHandler(string message, string error)
        {
            var service = JsonSerializer.Deserialize<Service>(message);
            using var context = new ApplicationDbContext();

            var data = new Service()
            {
                Id = service.Id,
                Message = service.Message,
                Status = error,
                CreatedDate = service.CreatedDate,
            };

            context.Update(data);
            context.SaveChanges();
        }
    }
}
