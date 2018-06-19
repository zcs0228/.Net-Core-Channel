using System;
using System.Threading;
using System.Threading.Tasks;

namespace SingleChannel
{
    class Program
    {
        public static string test = String.Empty;
        static void Main(string[] args)
        {
            SingleChannel pip = new SingleChannel();
            Func<RequestDelegate, RequestDelegate> action1 = next =>
            {
                return context =>
                {
                    context.Value += "a";
                    return next(context);
                };
            };
            pip.Use(action1);
            Func<RequestDelegate, RequestDelegate> action2 = next =>
            {
                return context =>
                {
                    context.Value += "b";
                    return next(context);
                };
            };
            pip.Use(action2);
            pip.Use(new MyMiddleware());
            pip.Use(new MyMiddleware1());
            MyContext mycontext = new MyContext();
            Task task = pip.Build()(mycontext);
            task.Wait();
            Thread.Sleep(3000);
            Console.WriteLine("Hello World!" + mycontext.Value);
            Console.ReadKey();
        }
    }

    public class MyMiddleware : IMiddleware
    {
        public void Invoke(MyContext context)
        {
            context.Value += "cdefg";
        }
    }
    public class MyMiddleware1 : IMiddleware
    {
        public void Invoke(MyContext context)
        {
            context.Value += "ABCD";
        }
    }
}
