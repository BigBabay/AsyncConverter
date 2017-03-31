# AsyncConverter
Plugin for ReSharper, for converting code to `async`.

## Replacing Value
If expect type `Task<int>`, but real type is `int`, you may wrap it to `Task.FromResult()`

## Convert Any Method to Async Implementation
1. Replace return type to `Task` or `Task<T>`
2. Rename method and overrides and base and interface from &lt;MethodName&gt; to &lt;MethodName&gt;Async
3. Add using on `System.Threading.Tasks`
4. Analyze body and replace all calls to another method to `async` version if it exists.
5. Analyze body and replace all calls to `.Result` with `await` call.
6. Analyze using of this method. If method call is from `async` context, then replace it to `await`. If the method calls from sync context then replace calls to `.Result` or `.Wait()`
