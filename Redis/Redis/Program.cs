using StackExchange.Redis;
using System;

namespace Redis
{
  
    class Program
    {
        private static string Channel { get; set; }
        private static string userName  { get; set; }

        static void Main()
        {
            var configuration = new ConfigurationOptions();
            configuration.EndPoints.Add("localhost", 6379);
            var chat = ConnectionMultiplexer.Connect(configuration);
            var dateBase = chat.GetDatabase(1);

            Console.WriteLine("Enter your name: ");
            userName = Console.ReadLine();

            Console.WriteLine("Enter channel name to join: ");
            Channel = Console.ReadLine();

            var subscriber = chat.GetSubscriber();
  
            subscriber.Subscribe(Channel,
                  (channel, message) => Console.WriteLine(message));

            var command = "";

            while (command != "exit")
            {
                var message = "";
                Console.WriteLine("Enter command: ");
                command = Console.ReadLine();
                if (command == "send")
                {
                    Console.WriteLine("Enter your message: ");
                    message = Console.ReadLine();
                    subscriber.Publish(Channel, $"{userName}: {message}");
                    dateBase.StringSet(userName, message);
                }
            }
        }

        
        

    }
}
