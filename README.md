# AsyncConverter
Plugin for resharper, for converting code to async.

## Replacing value
If expect type Task<int>, but real type is int, you may wrap it to Task.FromResult

## Convert any method to async implimentation.
1. Replace return type to Task or Task<T>
2. Rename method and overrides and base and interface from <MethodName> to <MethodName>Async
3. Add using on System.Threading.Tasks
4. Analize body and replace all call to another method to async version if it exists.
5. Analize body and replace all call with .Result to await call.
6. Analize using of this method, if it call from async context then replace it to await. If it calls from sync context then replace to .Result or .Wait()
