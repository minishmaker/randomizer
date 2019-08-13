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
beq	EndFalse

@check if select has been pressed
ldr	r0,=#0x3000FF0
ldrh	r2,[r0,#2]
mov	r0,#4
and	r0,r2
cmp	r0,#0
beq	EndFalse

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

.align
.ltorg
progressiveSwapTable:
@POIN progressiveSwapTable
@POIN returnOffsetFalse
@POIN returnOffsetTrue
@WORD quickSwap
