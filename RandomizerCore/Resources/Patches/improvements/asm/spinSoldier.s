.thumb
push	{r4,lr}
mov	r4,r0
ldrb	r0,[r4,#0x0C]
push	{r0-r3}
@check if we have spin
ldr	r0,=#0x2002B44
ldrb	r0,[r0]
mov	r1,#1
and	r0,r1
cmp	r0,#0
beq	end
@check if we have a sword
ldr	r0,=#0x2002B32
ldrh	r0,[r0]
ldr	r1,=#0x1154
and	r0,r1
cmp	r0,#0
beq	end
@set soldier flag
ldr	r0,=#0x2002CD5
ldrb	r1,[r0]
mov	r2,#8
orr	r1,r2
strb	r1,[r0]

end:
pop	{r0-r3}
cmp	r0,#0
bne	end2
ldr	r3,=#0x8059493
bx	r3

end2:
ldr	r3,=#0x80594BD
bx	r3
