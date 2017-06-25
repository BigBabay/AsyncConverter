using System;
using System.Threading.Tasks;

namespace A
{
    public class Class
    {
        public string {caret}Test()
        {
            var a = Method("input");
            return a;
        }

        private string Method<T>(T input)
        {
            throw new NotImplementedException();
        }

        private Task<string> MethodAsync<T>(T input)
        {
            throw new NotImplementedException();
        }
    }
}
