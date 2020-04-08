# Itemtypes
Itemtypes allow different types of items to be defined, which require different amounts of information to 
## Definition
An itemtype can be defined like this:
```
itemtype <name>(<formal_parameters>);
```
<name> is the name of the item type
<formal_parameters> is a comma-separated list of n

## Invocation
For default items, the syntax looks like this:
```
<shuffle-type>[ "[" <dungeon-list> "]" ] <itemtype>(<parameters>)
```
<shuffle-type> is one of the shuffle types (see below)
<dungeon-list> is an optional list of dungeons the item can be placed in
<itemtype> is the name of a previously defined itemtype
<parameters> is the list of parameters required for that itemtype

## Shuffle types
A shuffle type can be `major`, `nice`, `minor`, or `unshuffled`.
`major` items will be shuffled with full logic checks, and will always be reachable without beatable-only logic.
`nice` items will be shuffled with partial logic checks; they will always be reachable but cannot affect logic themselves.
`minor` items will not have any logic checks at all, and may be placed wherever
`unshuffled` items will not be moved from their starting position.

# Nodes
Nodes are the main structures of the randomizer, used both to connect locations and optionally contain items.
A node can be connected to other nodes to form logic. Nodes that have address types that can contain items can have a default

## Definitions
A node can be defined like so:
```node <name> <address> [: <default-contents>] [: <dungeon-list>];
```
<name> 
<address> must be an instance of a previously defined addresstype
<default-contents> must be an instance of a previously defined itemtype
<dungeon-list> is a comma-separated list of dungeons. "*" is a wildcard and means that items without a dungeon can be placed there.

Examples:
```
node ReachFortress NoAddress();
node FortressClonePuzzle ItemAddress(0E1E8B, 0E1E8C) : FoWSmallKey : "*", "Fortress";
unshuffled node DropletsFirstIceblock ItemAddress(098C1A, 098C1C) : ToDBigKey;
```

# Connections
## Creation
To connect two nodes, use this syntax:
```
connect <node-one> <node-two> [: <conditions>];
```
<node-one> must be the name of a previously defined node
<node-two> is the name of the previously defined node you want to connect to <node-one>
<conditions> is the list of item instances or helpers required to traverse the connection

Examples:
```
connect DeepwoodBlueWarpRoom DeepwoodWiggler : DwSSmallKey | Item(Lantern);
connect AccessMinishWoods MinishWoodsBarrel
```

## Conditions
Conditions are formed of either variables, item instances, or helpers, separated by | or &.
Expressions like `DwSSmallKey | Item(Lantern)

# Advanced
## Tokens
This is a list of tokens, and regexes that define each of them.
Note: keywords don't need a non-word check (?:\W) before, as the character before is definitely part of the previous token
* `<major_keyword> ::= major(?:\W)`
* `<nice_keyword> ::= nice(?:\W)`
* `<minor_keyword> ::= minor(?:\W)`
* `<unshuffled_keyword> ::= unshuffled(?:\W)`
* `<node_keyword> ::= node(?:\W)`
* `<identifier> ::= [a-zA-Z]\w*`
* `<colon> ::= :`
* `<semicolon> ::= ;`
* `<comma> ::= ,`
* `<open_paren> ::= (`
* `<close_paren> ::= )`
* `<open_bracket> ::= [`
* `<close_bracket> ::= ]`
* `<string_constant> ::= "(?:(?=(\\?))\2.)*?"
   Credit to https://stackoverflow.com/questions/171480/regex-grabbing-values-between-quotation-marks for this regex

Whitespace is entirely ignored by the interpreter, as are comments.

## Formal grammar
The logic is parsed using this formal grammar (written in something that vaguely resembles BNF). `[ ]` mean a portion is optional, `{ }` means a portion can be repeated 0 or more times
```
<line> ::= <node_definition>
<node_definition> ::= [<unshuffled_keyword>] <node_keyword> <identifier> <address_instance> [<colon> <shuffle_specifier> <item_instance>] [<colon> <dungeon_list>] <semicolon>
<shuffle_specifier> ::= <shuffle_type> [<open_bracket> <dungeon_list> <close_bracket>]
<dungeon_list> ::= <string_constant> {<comma> <string_constant>}
```