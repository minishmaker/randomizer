# ColorzCore
A rewriting of Core.exe for [Event Assembler](https://github.com/TimoVesalainen/Event-Assembler).

# Language Overview
Event Assembler Language is a language for describing the writing of binary or formatted payloads with relative addressing.
A script is a series of statements; each statement is either a directive, a command, a label, or a raw.

If no offset changing commands are given, then the assembler continues writing data sequentially to the current offset.

## Directives

A few of the common directives are `#include`, `#define`, `#ifdef` (and `#ifndef`, `#else`, `#endif`), `#incbin`, `#incext`, and `#incextevent`.

`#include` and `#ifdef` function as they would in C. `#include` inserts the contents of another file at that location. `#ifdef` is for conditional parsing.

We may `#define` definitions or macros. Definitions are substituted as-is on a token level. They may only refer to previous definitions (as parsing is one-pass). Macros are akin to definitions, but take several parameters. Substitution is done at the token level. This is akin to how C does it.

`#incbin` writes the binary contents of the file provided at the current offset.

`#incext` calls the provided tool (in the `./Tools` folder) with the given parameters along with `--to-stdout`. It then writes the binary output from stdout to the current location.

`#incextevent` calls the provided tool with the given parameters along with `--to-stdout`, then treats the output from stdout as an event file, and effectively `#include`s it.

## Commands

These commands affect the current location: `ORG`, `ALIGN`, `PUSH`, `POP`.
`ORG` changes the current offset to the one given, `ALIGN` changes the current offset to the next multiple of the one given. `PUSH` stores the current offset for future use of `POP`, which returns the current offset to what was `PUSH`ed.

These commands report assembler state to the appropriate category: `MESSAGE`, `WARNING`, `ERROR`.
Any number of expressions can be given for evaluation, or a string may be provided (in quotes).

These commands ensure a certain state for the assembler: `PROTECT`, `ASSERT`.
`PROTECT start end` will error on any future write to anywhere in the range of offsets provided, and provide where the write occurred for debugging. `PROTECT offset` only protects the single offset.
`ASSERT` will error if the provided expression is negative.

## Raws

Raws are defined in a separate language, and specification for all raws are (by default) in the `./Language Raws` folder. Raws describe a pattern of binary data to be written, say for example, "the short 0x1234 followed by two one-byte coordinates". Raws may also be written with no assumption about the data (e.g. "a byte").

## Labels
Having `LabelName:` brings in `LabelName` as an identifier which evaluates to what the current offset was when the label was defined.

## Special Token(s)
`currentOffset` (case insensitive) always gets evaluated to the current offset of the assembler.

## Small Example

Assuming the raw `WORD` assembles the word(s) provided after it, the following script writes 0xDEADBEEF at 0x1000, and writes 0x08001000 at 0x2000
```
ORG 0x1000
MyLabel:
WORD 0xDEADBEEF

#define Pointer(Location) "WORD (Location | 0x08000000)"

ORG 0x2000
Pointer(MyLabel)
```

# Projects made with ColorzCore
[Snek-GBA](https://github.com/LeonarthCG/Snek-GBA), by Leonarth. A full, independently made GBA game. ColorzCore was used as the linker and build tool.

[VBA: Blitz Tendency](https://github.com/FireEmblemUniverse/VBA-Blitz-Tendency/), by Crazycolorz5. An extensive, community mod of Fire Emblem: The Sacred Stones. Considering the large number of contributors, ColorzCore was used to make sequences of binary writes version controllable. It was also used as the build tool.
