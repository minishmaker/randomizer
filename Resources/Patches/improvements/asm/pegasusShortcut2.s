.equ	ReturnTrue, ReturnFalse+4
.thumb
push	{lr}
ldr	r2,=#0x2002AE8
ldrb	r3,[r0,#0x01]
ldrb	r0,[r2,#0x0C]

push	{r5-r6}
@check if we are checking for the boots
cmp	r3,#0x15
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
pop	{r5-r6}
push	{r3}
ldr	r3,ReturnTrue
mov	lr,r3
pop	{r3}
.short	0xF800


NoBoots:
pop	{r5-r6}
push	{r3}
ldr	r3,ReturnFalse
mov	lr,r3
pop	{r3}
.short	0xF800

.align
.ltorg
ReturnFalse:
@POIN ReturnFalse
@POIN ReturnTrue
