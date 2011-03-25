Cryptophage
===========

A .NET interface wrapper for GPG.

Cryptophage requires an existing GnuPG installation, and communicates with the gpg.exe
process in "batch" mode, rather than directly linking against libraries. In this sense,
it is probably not entirely optimal but it does make it much easier to write, debug, 
and test.

There are two main API entry points: 

* The `Gpg` static class is used for simple cryptographic operations with only basic options.
* The `GpgCommandExecutor` is a lower-level API that allows arbitrary GPG commands to be run.

The `Gpg` static class contains a nested `Async` class exposing an asynchronous version of the
operations.

Instances of `GpgCommand` classes can be created using the static factory methods on the class,
and options for the command can be configured using a fluent syntax.


Todo List
---------

* Test Fixtures ?
* Key generation
* Key maintenance
* A more usable client API