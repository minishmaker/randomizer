.thumb
ldr	r0,=#0x2002EA4
mov	r1,#0x02
ldrb	r0,[r0]
and	r0,r1
cmp	r0,#0
beq	False

True:
pop	{pc}

False:
ldr	r3,=#0x804CD5D
bx	r3
