.equ	returnOffset, progressiveTable+4
.equ	extraProgressive, returnOffset+4
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
mov	r7,r1	@sub ID
ldr	r5,progressiveTable

@check if this is the extra item
cmp	r4, #0x05
bne	notExtra
ldr	r0,extraProgressive
ldrb	r4, [r0, r7]
notExtra:

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
ldr	r3,=#0x2002B32	@location of flags
@check how many flags are met
checkFlag:
ldrb	r0,[r6]
cmp	r0,#0xFF
beq	doneFlags
ldrb	r1,[r6,#1]
ldrb	r0,[r3,r0]
and	r0,r1
cmp	r0,#0
beq	doneFlags
add	r6,#2
add	r4,#1
b	checkFlag
doneFlags:
@check how many items there are in the table
mov	r3,#0
ammountLoop:
ldrb	r0,[r5,r3]
cmp	r0,#0xFF
beq	gotAmmount
add	r3,#1
b	ammountLoop
gotAmmount:
@check if we went overboard
cmp	r4,r3
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
@check if what we got was a sword with clones
cmp	r4, #3
beq	oneClone
cmp	r4, #4
beq	twoClones
cmp	r4, #6
beq	threeClones
b	notSword

@update clone limit if the new sword has a higher one
oneClone:
mov	r3, #1 @new count
@get current clone count
ldr	r0, =#0x203FE00+(10*2)
ldrh	r0, [r0]
lsl	r0, #32 - 2
lsr	r0, #32 - 2
@if we have never had clones, update
cmp	r0, #0
beq	storeClones
b	notSword

twoClones:
mov	r3, #2 @new count
@get current clone count
ldr	r0, =#0x203FE00+(10*2)
ldrh	r0, [r0]
lsl	r0, #32 - 2
lsr	r0, #32 - 2
@if we have never had clones, update
cmp	r0, #0
beq	storeClones
@if using three clones, ignore
cmp	r0, #3
beq	notSword
@check if four sword unlocked
ldr	r0, =#0x2002B32
ldrb	r0, [r0, #1]
mov	r1, #0x30
and	r0, r1
beq	storeClones
@four sword is unlocked, so we leave the one clone setting alone
b	notSword

threeClones:
mov	r3, #3 @new count
@get current clone count
ldr	r0, =#0x203FE00+(10*2)
ldrh	r0, [r0]
lsl	r0, #32 - 2
lsr	r0, #32 - 2
@if we have never had clones, update
cmp	r0, #0
beq	storeClones
@if we are using two clones, update
cmp	r0, #2
beq	storeClones
@check if blue sowrd unlocked
ldr	r0, =#0x2002B32
ldrb	r0, [r0, #1]
mov	r1, #0x03
and	r0, r1
beq	storeClones
@blue sword is unlocked, so we leave the one clone setting alone
b	notSword

storeClones:
ldr	r0, =#0x203FE00+(10*2)
ldrh	r1, [r0]
lsr	r1, #2
lsl	r1, #2
orr	r1, r3
strh	r1, [r0]

notSword:
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
@POIN extraProgressive
