.thumb
ldr	r0,=#0x2002EA4
mov	r1,#0x02
ldrb	r0,[r0]
and	r0,r1
cmp	r0,#0
beq	False

True:
ldr	r3,=#0x809A611
bx	r3

False:
pop	{r4,pc}
