.equ	returnOffsetFalse, progressiveSwapTable+4
.equ	returnOffsetTrue, returnOffsetFalse+4
.equ	quickSwap, returnOffsetTrue+4
.thumb
ldr	r0,=#0x3000FF0
ldrh	r2,[r0,#2]
mov	r3,r0
push	{r0-r7}
ldr	r7,=#0x2000080

@check if we are hovering over save
ldrb	r0,[r7,#3]
cmp	r0,#0x10
bne	skipbranch1
b	EndFalse
skipbranch1:

@check if select has been pressed
ldr	r0,=#0x3000FF0
ldrh	r2,[r0,#2]
mov	r0,#4
and	r0,r2
cmp	r0,#0
bne	skipbranch2	
b	EndFalse
skipbranch2:

@check if the item we are hovering over is in the list
ldrb	r0,[r7,#3]
add	r0,#0x10
ldrb	r4,[r7,r0]	@Item ID
ldr	r5,progressiveSwapTable
findIDLoop:
ldrb	r0,[r5]
cmp	r0,r4
beq	matchedID
cmp	r0,#0xFF
beq	EndFalse
add	r5,#2
b	findIDLoop
matchedID:
ldrb	r5,[r5,#1]	@Item ID to swap for

@check if the item is bombs, if so then check that no bombs are being used
cmp	r5,#0x07
beq	isBomb
cmp	r5,#0x08
bne	noBomb
isBomb:
mov	r0,#8
mov	r1,#2
mov	r2,#2
bl	checkItemObject
cmp	r0,#0
bne	EndFalse
noBomb:

@check if the item is arrows, if so then check that no arrows are being used
cmp	r5,#0x09
beq	isBow
cmp	r5,#0x0A
bne	noBow
isBow:
mov	r0,#8
mov	r1,#4
mov	r2,#2
bl	checkItemObject
cmp	r0,#0
bne	EndFalse
noBow:

@check if the item is boomerang, same as the others
cmp	r5,#0x0B
beq	isBoom
cmp	r5,#0x0C
bne	noBoom
isBoom:
mov	r0,#8
mov	r1,#3
mov	r2,#2
bl	checkItemObject
cmp	r0,#0
bne	EndFalse
noBoom:

@check if the player owns both items
mov	r2,r4
lsr	r0,r2,#2
ldr	r1,=#0x2002B32
add	r0,r1
ldrb	r0,[r0]
mov	r1,#3
and	r2,r1
lsl	r2,#1
asr	r0,r2
and	r0,r1
cmp	r0,#0
beq	EndFalse
mov	r2,r5
lsr	r0,r2,#2
ldr	r1,=#0x2002B32
add	r0,r1
ldrb	r0,[r0]
mov	r1,#3
and	r2,r1
lsl	r2,#1
asr	r0,r2
and	r0,r1
cmp	r0,#0
beq	EndFalse

@set old item as unusable
mov	r3,r4
lsr	r6,r3,#2
ldr	r0,=#0x2002B32
add	r6,r0
mov	r0,#3
and	r3,r0
lsl	r3,#1
ldrb	r2,[r6]
lsl	r0,r3
and	r0,r2
eor	r2,r0
mov	r1,#2
lsl	r1,r3
orr	r2,r1
mov	r1,#1
lsl	r1,r3
mvn	r1,r1
and	r2,r1
strb	r2,[r6]

@and set new item as usable
mov	r3,r5
lsr	r6,r3,#2
ldr	r0,=#0x2002B32
add	r6,r0
mov	r0,#3
and	r3,r0
lsl	r3,#1
ldrb	r2,[r6]
lsl	r0,r3
and	r0,r2
eor	r2,r0
mov	r1,#1
lsl	r1,r3
orr	r2,r1
mov	r1,#2
lsl	r1,r3
mvn	r1,r1
and	r2,r1
strb	r2,[r6]

@change which item is in the inventory
ldrb	r0,[r7,#3]
add	r0,#0x10
strb	r5,[r7,r0]	@new Item ID

@check if quickSwap is on
ldr	r0,quickSwap
cmp	r0,#0
beq	EndTrue

@possibly change which item is being held in slot 1
ldr	r0,=#0x2002AC0
add	r0,#0x34
ldrb	r1,[r0]
cmp	r1,r4
bne	notSame1
strb	r5,[r0]
notSame1:

@possibly change which item is being held in slot 2
ldr	r0,=#0x2002AC0
add	r0,#0x35
ldrb	r1,[r0]
cmp	r1,r4
bne	notSame2
strb	r5,[r0]
notSame2:

@redraw item sprites
ldr	r3,=#0x80A4A4C
mov	lr,r3
.short	0xF800

@and play a sound
mov	r0,#0x6A
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800

EndTrue:
ldr	r3,returnOffsetTrue
mov	lr,r3
pop	{r0-r7}
cmp	r2,#0x20
.short	0xF800

EndFalse:
ldr	r3,returnOffsetFalse
mov	lr,r3
pop	{r0-r7}
cmp	r2,#0x01
.short	0xF800

checkItemObject:
push	{r4-r5,lr}
mov	r3,r0
lsl	r2,#3
ldr	r0,=#0x20369F0
ldr	r5,=#0x3003D70
add	r5,r2
ldr	r5,[r5]
add	r2,r0
ldr	r0,[r2,#4]
cmp	r0,r5
beq	noObject
objectLoop:
ldrb	r4,[r0,#8]
cmp	r3,r4
bne	nextObject
ldrb	r4,[r0,#9]
cmp	r1,r4
beq	returnObject
nextObject:
ldr	r0,[r0,#4]
cmp	r0,r5
bne	objectLoop
noObject:
mov	r0,#0
returnObject:
pop	{r4-r5,pc}

.align
.ltorg
progressiveSwapTable:
@POIN progressiveSwapTable
@POIN returnOffsetFalse
@POIN returnOffsetTrue
@WORD quickSwap
