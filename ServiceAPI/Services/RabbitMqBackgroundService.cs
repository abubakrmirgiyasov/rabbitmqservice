namespace ServiceAPI
{
    public class RabbitMqBackgroundService : RabbitMqCronoService
    {
        private const int TIMER = 3;

        public RabbitMqBackgroundService(IScheduleConfig<RabbitMqBackgroundService> config)
            : base(config.CronExpression, config.TimeZone) { }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Started... [{DateTime.Now:hh:mm:ss}]");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{DateTime.Now:hh:mm:ss}]");

            CheckDataBase();

            return base.DoWork(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stopping... [{DateTime.Now:hh:mm:ss}]");

            return base.StopAsync(cancellationToken);
        }

        public string CheckDataBase()
        {
            using var db = new ApplicationDbContext();

            var messages = db.Messages.ToList();

            var list = messages
                .Where(x => x.Status == "In process" && (DateTime.Now - x.CreatedDate) > new TimeSpan(0, TIMER, 0))
                .Select(x => x.Id);

            foreach (var item in list)
            {
                Console.WriteLine($"Просрочено. \nID: {item}");

                return FindStatusWithId(item, db);
            }

            return "";
        }

        public string FindStatusWithId(Guid id, ApplicationDbContext db)
        {
            var item = db.Messages.FirstOrDefault(x => x.Id == id);

            item.Message = item.Message;
            item.Status = "Просрочено";
            item.CreatedDate = DateTime.Now;

            db.SaveChanges();

            return item.Status;
        }
    }
}
