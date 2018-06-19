using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleChannel
{
    public class SingleChannel
    {
        private IList<Func<RequestDelegate, RequestDelegate>> middlewares = new List<Func<RequestDelegate, RequestDelegate>>();
        public RequestDelegate Build()
        {
            RequestDelegate seed = context => Task.Run(() => {

            });
            return middlewares.Reverse().Aggregate(seed, (next, current) => current(next));
        }
        public SingleChannel Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }

        public SingleChannel Use(IMiddleware middleware)
        {
            Func<RequestDelegate, RequestDelegate> mymiddleware = next =>
            {
                return context =>
                {
                    middleware.Invoke(context);
                    return next(context);
                };
            };
            middlewares.Add(mymiddleware);
            return this;
        }
    }

    public class MyContext
    {
        public string Value { get; set; }
    }

    public delegate Task RequestDelegate(MyContext context);

    public interface IMiddleware
    {
        void Invoke(MyContext context);
    }
}
