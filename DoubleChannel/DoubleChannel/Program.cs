using System;
using System.Threading;
using System.Threading.Tasks;

namespace DoubleChannel
{
    class Program
    {
        public static string test = String.Empty;
        static void Main(string[] args)
        {
            DoubleChannel pip = new DoubleChannel();
            Func<RequestDelegate, RequestDelegate> action1 = next =>
            {
                return context =>
                {
                    context.Value += "a";
                    next(context);
                    context.Value += "A";
                };
            };
            //pip.Use(action1);
            Func<RequestDelegate, RequestDelegate> action2 = next =>
            {
                return context =>
                {
                    context.Value += "b";
                    next(context);
                    context.Value += "B";
                };
            };
            //pip.Use(action2);
            Func<RequestDelegate, RequestDelegate> action3 = next =>
            {
                return context =>
                {
                    context.Value += "c";
                    next(context);
                    context.Value += "C";
                };
            };
            //pip.Use(action3);
            pip.Use(new MyMiddleware());
            pip.Use(new MyMiddleware1());
            MyContext mycontext = new MyContext();
            pip.Build()(mycontext);
            Console.WriteLine("Hello World!" + mycontext.Value);
            Console.ReadKey();
        }
    }

    public class MyMiddleware : BaseMiddleware
    {
        public override void AfterNextInvoke(MyContext context)
        {
            context.Value += "a";
        }

        public override void BeforeNextInvoke(MyContext context)
        {
            context.Value += "A";
        }
    }
    public class MyMiddleware1 : BaseMiddleware
    {
        public override void AfterNextInvoke(MyContext context)
        {
            context.Value += "b";
        }

        public override void BeforeNextInvoke(MyContext context)
        {
            context.Value += "B";
        }
    }
}
