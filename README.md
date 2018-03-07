# DNX - .NET Execute

A tool that helps you run .NET tools without first having to install them.

.NET global tools are a new feature in .NET 2.1, which is currently in preview.


## Synopsis


```bash
dnx [options] <command> [--] [<arguments>]...
```

For example, for `dotnetsay`:

```bash
dnx dotnetsay Hello World!
```

The output will be exactly the same as if the tool was installed and then executed.

## Install


Install .NET Core CLI at least 2.1 from [microsoft.com](https://www.microsoft.com/net/download/all),
then run:

```bash
dotnet install tool -g dnx
```

### Parameters

Run `dnx` with `--help`  to see possible options. Here we document a few:

* `--rm` Removes the executable if it was installed. You can also set the environment variable `DNX_REMOVE_AFTER_RUN` to `true` so you don't have to set it every time.
* `--package` allows you to specify a package name different from the command name.
* `--package-version` allows you to specify a version different from latest.
* `--verbose` shows you logs about installing, execution and debuggin.

Use `--` to separate `dnx` parameters from tool parameters.

### Exit code

The tool will exit with 1 if installation failed of dotnet was not found.

If the installation is successful then the executed application exit code will be used as exit code.

## Testing install during development

Just cd to `src/dnx` and run `dotnet pack -C Release -o ../nupkg`.

Then cd to `src/nupkg` and run `dotnet install tool -g dnx`.

## Maintainers/Core team

* [Giovanni Bassi](http://blog.lambda3.com.br/L3/giovannibassi/), aka Giggio, [Lambda3](http://www.lambda3.com.br), [@giovannibassi](https://twitter.com/giovannibassi)

Contributors can be found at the [contributors](https://github.com/lambda3/dnx/graphs/contributors) page on Github.

## Contact

Use Twitter.

## License

This software is open source, licensed under the Apache License, Version 2.0.
See [LICENSE.txt](https://github.com/lambda3/dnx/blob/master/LICENSE.txt) for details.
Check out the terms of the license before you contribute, fork, copy or do anything
with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project, and agree to
mention clearly if do not agree to these terms. Your work will be licensed with the project at Apache V2, along the rest of the code.
