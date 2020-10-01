# AsyncConverter

This is an implementation of a ReSharper Plugin that converts your synchronous code to its asynchronous version and helps you to write your own asynchronous applications.

# Convert Any Method to Its Async Implementation

AsyncConverter can:

1. Replace a returning type with generic or non-generic `Task<T>` or `Task`
2. Rename a hierarchy of overridden methods from _&lt;MethodName&gt;_ to _&lt;MethodName&gt;Async_
3. Add the `System.Threading.Tasks` to a usings declaration
4. Analyze a method body and replace the every synchronous call with its `async` implementation if exists.
5. Analyze a method body and replace the every `.Result` call with the `await` call.
6. Analyze usage of a processed method. If the method is called from `async` context the AsyncConverter will replace its call with the `await` expression, otherwise it will just call `.Result` or `.Wait()`

<details>
    <summary>Converter method to async demo</summary>

    ![Converter method to async](ReadMe/MethodToAsyncConverter.gif)
</details>

# Highlightings

## Convert `Wait()` and `Result` to `await`

Under `async` method replace `Wait()` and `Result` to `await`.

<details>
    <summary>Replace wait to await demo</summary>

    ![Replace wait to await](ReadMe/ReplaceWait.gif)
</details>

<details>
    <summary>Replace result to await demo</summary>

    ![Replace result to await](ReadMe/ReplaceResult.gif)
</details>

## Return `null` as `Task`

If expected returning type is `Task` or `Task<T>` but null is returned instead, AsyncConverter warn you that execution point can await expected 'Task' and get `NullReferenceException`.

<details>
    <summary>Return null as task demo</summary>

    ![Return null as task](ReadMe/ReturnNullAsTask.gif)
</details>

## Async suffix in a method name

AsyncConverter will suggest you to add the `Async` suffix to an asynchronous method name in all cases except:

1. Classes inherited from `Controller` or `ApiController`
2. Methods of test classes. NUnit, XUnit and MsUnit are supported. This may be turn off in _Resharper &rarr; Options &rarr; Code Inspection &rarr; Async Converter &rarr; Async Suffix_

<details>
    <summary>Suggesting method name with Async suffix demo</summary>

    ![Suggesting method name with Async suffix](ReadMe/Naming.gif)
</details>

## Suggesting to configure an every await expression with ConfigureAwait

<details>
    <summary>Suggesting ConfigureAwait demo</summary>

    ![Suggesting ConfigureAwait](ReadMe/ConfigureAwait.gif)
</details>

## Suggesting to use the async method if exists

If a synchronous method is called in the async context and its asynchronous implementation exists (e.g method has same signature `Async` suffix and `Task` or `Task<T>` as the returning type) AsyncConverter will suggest you to use this asynchronous implementation.

Do not suggest to use obsolete async methods.

<details>
    <summary>Suggesting method name with Async suffix demo</summary>

    ![Suggesting method name with Async suffix](ReadMe/CanBeUseAsyncMethod.gif)
</details>

## Async/await ignoring

An `await` expression can be ignored if this `await` expression is the single in a method and awaited value is returned from a method.

<details>
    <summary>Async/await ignoring demo</summary>

    ![Async/await ignoring](ReadMe/AsyncAwaitMayBeElided.gif)
</details>
