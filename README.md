MPRV
====

Model-Process-Regulator-View framework for .NET

This framework is a sister to the framework of the same name that I've been working on at Casting Networks, but this is a reimplementation of MPRV after having worked with it for about two years. These concepts are not purely my own, but also have been incubated and matured by Kraig van der Klomp, Harlan North, Ryan Davis, and Denny Ferrassoli.

That said, this is entirely different code.

What is MPRV?
=============

MPRV is essentially a form of MVC. The two largely share the concepts of Models and Views, but MPRV breaks up MVC's Controller into Processes and Regulators.

The result is cleaner separation of functionality.

In practice, we (the development team at Casting Networks) have been able to write a library of processes that are (mostly) decoupled from any specific view layer. We are able to use a process in a web app and in a console app, and there are different view layers for each.
