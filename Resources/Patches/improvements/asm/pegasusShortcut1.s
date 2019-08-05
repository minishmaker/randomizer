.equ	ReturnTrue, ReturnFalse+4
.equ	buttonTable, ReturnTrue+4
.thumb
push	{r4-r6,lr}
mov	r4,r0
mov	r6,r1

push	{r5-r6}
@check if we are checking for the A button
cmp	r6,#1
bne	NoBoots

@check if the L button is pressed
ldr	r5,=#0x3004010
ldrh	r5,[r5]
ldr	r6,=#0x1000
and	r5,r6
cmp	r5,#0
beq	NoBoots

@check if pegasus boots are disabled
ldr	r5,=#0x2002B32
ldrb	r5,[r5,#5]
mov	r6,#8
and	r6,r5
cmp	r6,#0
bne	NoBoots

@check if pegasus boots are owned
mov	r6,#4
and	r6,r5
cmp	r6,#0
beq	NoBoots

@pretend we have the pegasus boots in this button
Boots:
mov	r4,#0x15
pop	{r5-r6}
lsl	r0,r4,#1
add	r0,r4
lsl	r0,#2
ldr	r1,buttonTable
ldr	r1,[r1]
add	r5,r0,r1
ldr	r3,ReturnTrue
bx	r3

NoBoots:
pop	{r5-r6}
sub	r0,r4,#1
ldr	r3,ReturnFalse
bx	r3

.align
.ltorg
ReturnFalse:
@POIN ReturnFalse
@POIN ReturnTrue
@POIN buttonTable
