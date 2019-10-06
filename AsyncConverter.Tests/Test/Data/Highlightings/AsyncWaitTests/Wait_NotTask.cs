using System;
using System.Threading.Tasks;

namespace AsyncConverter.Tests.Test.Data.FixReturnValueToTaskTests
{
    public class Class
    {
        public async Task<int> TestAsync()
        {
            var waitable = new Waitable();
            waitable.Wait();
            
            var wrapper = new WaitableWrapper();
            wrapper.Value.Wait();
            
            var factory = new WaitableFactory();
            factory.Create().Wait();
            
            Wait();

            StaticWaiter.Wait();
            
            await Task.Delay(1000).ConfigureAwait(false);
            Console.WriteLine(x);
            return 0;
        }

        private void Wait()
        {
            x++;
        }

        private int x;
    }

    public class WaitableFactory
    {
        public Waitable Create()
        {
            return new Waitable();
        }
    }
    
    public class WaitableWrapper
    {
        public Waitable Value => new Waitable();
    }

    public class Waitable
    {
        public void Wait()
        {
        }
    }

    public class StaticWaiter
    {
        public static void Wait()
        {
        }
    }
}