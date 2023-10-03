
## General
A `PasswordSequence` is a part or the whole password. (password snippet)

## Settings
There must be some properties to adjust to define a password snippet.
### Length
Defines the length of a snippet.
Decide if it's the actual length in characters or in charset elements.
Maybe implement min and max length.
### Charset
A pre defined charset
- latin letters lower \[a-z]
- latin letters upper \[A-Z]
- latin letters mixed \[a-z,A-Z]
- special characters \[%,&,#,...]
- numbers \[0,1,2,...]
A custom charset
- list of chars
- list of strings
	- this allows to include words or word snippets

### Min / Max Chars
The minimum and/or maximum number of chars each charset should be ocurring in the password.
Need to define what happens, when the numbers are more than the length.
- throw error
- just look at specific charsets
- get % values of the actual length with the selected length

#ise #passwordgenerator