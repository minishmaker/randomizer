.equ	returnTrue, returnFalse+4
.equ	glitchless, returnTrue+4
.thumb
push	{lr}
ldr	r2,=#0x2002AE8
ldrb	r3,[r0,#0x01]
ldrb	r0,[r2,#0x0C]

push	{r5-r6}
@check if we are checking for the boots
cmp	r3,#0x15
bne	noBoots

@check if the L button is pressed
ldr	r5,=#0x3004010
ldrh	r5,[r5]
ldr	r6,=#0x1000
and	r5,r6
cmp	r5,#0
beq	noBoots

@check if pegasus boots are disabled
ldr	r5,=#0x2002B32
ldrb	r5,[r5,#5]
mov	r6,#8
and	r6,r5
cmp	r6,#0
bne	noBoots

@check if pegasus boots are owned
mov	r6,#4
and	r6,r5
cmp	r6,#0
bne	shortcut

@check if glitchless
noBoots:
ldr	r5, glitchless
cmp	r5, #0
beq	end

@check if we are checking for the ocarina
cmp	r3, #0x17
bne	end

@check if the Select button is pressed
ldr	r5,=#0x3000FF0
ldrh	r5,[r5]
mov	r6,#4
and	r5,r6
cmp	r5,#0
beq	end

@check if ocarina is disabled
ldr	r5,=#0x2002B32
ldrb	r5,[r5,#5]
mov	r6,#0x80
and	r6,r5
cmp	r6,#0
bne	end

@check if ocarina is owned
mov	r6,#0x40
and	r6,r5
cmp	r6,#0
beq	end

@pretend we have the pegasus boots/ocarina in this button
shortcut:
pop	{r5-r6}
push	{r3}
ldr	r3,returnTrue
mov	lr,r3
pop	{r3}
.short	0xF800

end:
pop	{r5-r6}
push	{r3}
ldr	r3,returnFalse
mov	lr,r3
pop	{r3}
.short	0xF800

.align
.ltorg
returnFalse:
@POIN returnFalse
@POIN returnTrue
@WORD glitchless