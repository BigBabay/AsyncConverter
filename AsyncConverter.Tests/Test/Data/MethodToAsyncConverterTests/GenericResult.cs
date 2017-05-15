using System;
using System.Threading.Tasks;

namespace A
{
    public class Class
    {
        public string {caret}Test()
        {
            var method = Method<string>();
            return method;
        }

        private T Method<T>()
        {
            throw new NotImplementedException();
        }

        private Task<T> MethodAsync<T>()
        {
            throw new NotImplementedException();
        }
    }
}
