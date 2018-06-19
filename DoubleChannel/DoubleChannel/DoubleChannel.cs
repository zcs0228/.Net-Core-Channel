using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleChannel
{
    public class DoubleChannel
    {
        private IList<Func<RequestDelegate, RequestDelegate>> middlewares = new List<Func<RequestDelegate, RequestDelegate>>();
        public RequestDelegate Build()
        {
            RequestDelegate seed = context => { };
            return middlewares.Reverse().Aggregate(seed, (next, current) => current(next));
        }
        public DoubleChannel Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }

        public DoubleChannel Use(BaseMiddleware middleware)
        {
            Func<RequestDelegate, RequestDelegate> mymiddleware = next =>
            {
                return context =>
                {
                    middleware.BeforeNextInvoke(context);
                    next(context);
                    middleware.AfterNextInvoke(context);
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

    public delegate void RequestDelegate(MyContext context);

    public abstract class BaseMiddleware
    {
        public virtual void BeforeNextInvoke(MyContext context)
        {
            return;
        }

        public virtual void AfterNextInvoke(MyContext context)
        {
            return;
        }
    }
}
