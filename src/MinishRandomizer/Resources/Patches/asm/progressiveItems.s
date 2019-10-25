.equ	returnOffset, progressiveTable+4
.equ	dojo, returnOffset+4
.thumb
push	{r4-r7,lr}
@check if this is buy mode
cmp	r2,#2
bne	doneshop
@check if we are in stockwell shop
ldr	r4,=#0x3000BF0
ldrb	r5,[r4,#5]
cmp	r5,#0
bne	notshop
ldrb	r4,[r4,#4]
cmp	r4,#0x23
beq	doneshop
notshop:
mov	r2,#0
doneshop:

push	{r1-r7}
@set up the data
mov	r4,r0	@item ID
ldr	r5,progressiveTable

@check if we are in the biggoron room
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#5]
cmp	r1,#0
bne	notgoron
ldrb	r0,[r0,#4]
cmp	r0,#0x1A
beq	End
notgoron:

@run dojo progressive
mov	r0,r4
ldr	r3,dojo
mov	lr,r3
.short	0xF800
mov	r4,r0

@now need to check if this item is progressive
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
