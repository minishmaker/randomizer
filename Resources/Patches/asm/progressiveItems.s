.equ	returnOffset, progressiveTable+4
.thumb
push	{r4-r7,lr}
push	{r1-r7}
@set up the data
mov	r4,r0	@item ID
ldr	r5,progressiveTable

@first we need to check if this item is progressive
tableLoop:
ldr	r0,[r5]
mov	r1,#0
sub	r1,#1
@if there are no more tables, end
cmp	r0,r1
beq	End
@check if the item is in the table
itemIDLoop:
ldrb	r1,[r0]
cmp	r1,r4
@if the id matches, go find out what item to give
beq	tableMatch
@if the table is over, end
cmp	r1,#0xFF
beq	nextTable
add	r0,#1
b	itemIDLoop
nextTable:
add	r5,#8
b	tableLoop

@we found a table that has the item so we are going use it
tableMatch:
mov	r4,#0		@number of flags met
ldr	r6,[r5,#4]	@flags table
ldr	r5,[r5]		@item id table
ldr	r7,=#0x2002B32	@location of flags
@check how many flags are met
checkFlag:
ldrb	r0,[r6]
cmp	r0,#0xFF
beq	doneFlags
ldrb	r1,[r6,#1]
ldrb	r0,[r7,r0]
and	r0,r1
cmp	r0,#0
beq	doneFlags
add	r6,#2
add	r4,#1
b	checkFlag
doneFlags:
@check how many items there are in the table
mov	r7,#0
ammountLoop:
ldrb	r0,[r5,r7]
cmp	r0,#0xFF
beq	gotAmmount
add	r7,#1
b	ammountLoop
gotAmmount:
@check if we went overboard
cmp	r4,r7
bhs	overboardItem
ldrb	r4,[r5,r4]
b	End

overboardItem:
@get the overboard item id
mov	r0,#0
overboardLoop:
ldrb	r4,[r5,r0]
cmp	r4,#0xFF
beq	gotOverboard
add	r0,#1
b	overboardLoop
gotOverboard:
add	r0,#1
ldrb	r4,[r5,r0]
b	End

End:
mov	r0,r4
pop	{r1-r7}
mov	r5,r0
mov	r6,r1
mov	r7,r2
ldr	r3,returnOffset
bx	r3

.align
.ltorg
progressiveTable:
@POIN progressiveTable
@POIN returnOffset
