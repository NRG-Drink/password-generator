## PasswordGenerator
Generate the most secure passwords or just use it as a lumpy string builder.
Config the `PasswordGenerator` with
- Fluent builder pattern
- Template string

## Core Functions
There are three core functions to build your password string.
### Append Sequence
Define a string and append it to at the end of the existing sequences.
`existing` + `new` = `existingnew`
### Insert Sequence
Define a string and insert it at a certain position of the append sequences.
insert at char 2: `existingnew` + `insert` = `exinsertistingnew`
insert at char 2: `exinsertistingnew` + `123` = `ex123insertistingnew`
### Fill Until
Define a string to fill your appended and inserted sequences to a certain amount of chars.
fill till 21: `exinsertistingnew`\[17] + `fill`\[4] = `exinsertistingnewfill`\[21] 

## Core Functions Hierarchy
1. Append all sequences in config order
2. Insert all sequences
	1. A second insert can insert in the first insert
3. Fill the sequence until limit

## Pre Defined Charsets
| key | description |
| --- | --- |
| n | one digit numbers |
| s | common special characters |
| c | lower case latin letters |
| C | upper case latin lettsers |
| a | all charsets above (Ccns) |
| x | custom charset |

## How To Use It Template String
### Example
`(c,3)(2sn,4)[4,a,3]f(n,15)`

| statement | func | description |
| --- | --- | --- |
| `(c)` | append | 1x any lower case letter |
| `(2sn,4)` | append | 4x any special or numer with at least 2 special |
| `[4,a,3]` | insert | 3x any pre defined chars at position 4 |
| `f(n,10)` | fill | any number until length 15 |

Possible output:
`y85/fRu!40`
`g/?06kp467`
`o1!%0wK727`
`z7?/5b7712`
`x?5!2zb388`

### Example With Custom Charset
charset0: 'Password', 'Generator'
charset1: ' is ', 's are '
charset2: 'fantastic', 'great', 'awesome'
`(x)(x^1)(x^2)f('!',25)`

| statement | func | description |
| --- | --- | --- |
| `(x)` | append | 1x any element of charset 0 |
| `(x^1)` | append | 1x  any element of charset 1 |
| `(x^2)` | append | 1x  any element of charset 2 |
| `f('!',25)` | fill | fill with ! until length 25 |

Possible output:
`Password is fantastic!!!!`
`Generator is fantastic!!!`
`Generators are fantastic!`
`Password is great!!!!!!!!`
`Generator is awesome!!!!!`

### Syntax
#### Append Sequence
A append statement is surrounded with round brackets. `( )`
It has to have at least one charset and a length (in elements).
See the `charset` section read more about the charsets and the `min occurrences`.
```c#
"(*min occurrences*charsets,length)"
```
#### Insert Sequence
A insert statement is surrounded with angle brackets. `[ ]`
It has to have at least a position, one charset and a length (in elements).
See the `charset` section read more about the charsets and the `min occurrences`.
```c#
"[position,*min occurrences*charsets,length]"
```
#### Fill Until
A fill statement is surrounded with angle brackets. `( )`
It has to have at least a until number, one charset.
See the `charset` section read more about the charsets and the `min occurrences`.
```c#
"f(*min occurrences*charsets,until)"
```