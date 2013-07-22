BefSharp
========

Befunge interpreter and EXE compiler in C#

Features
--------

* Interpreter with step-by-step execution
* "Compiler" which creates exe files.
    * Note: see "Compilation" heading below for more informations
* Text and numeric input
* fully support for the **p** and **g** command, including wrapping around edges
* GUI based, no silly console.
* Stack viewer
* Very fast interpretation in run mode.
* Run mode can be cancelled with the ESC key.
* Code changes with **p** instruction are instantly visible.
* Stack with up to 2G storage
* Numbers in stack can be bigger than 255 (uses int instead of char or byte)

Usage
-----

The precompiled binary in this project is digitally signed.
Do not overwrite it if you want it to stay this way.
After you have written/loaded/changed your code,
hit F6 to reset the compiler/interpreter to the default state.
Either hit F5 to run or F8 to step.

Purpose
-------

This application is a fully featured befunge interpreter.
You can easily step through the code or run throug it.
This thing has some neat additions, first of all,
in "run" mode it is faster than many other interpreters written
in a high level language.

Compilation
-----------

To be honest, this thing does not compile. It allows to create an EXE file
(use **File > Save As...** menu for this and select EXE as filetype).
This generates an exe file containing the code.