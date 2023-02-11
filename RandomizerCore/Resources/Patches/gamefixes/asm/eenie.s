.thumb
push	{lr}
@check if fusion was successful
ldrb	r1,[r1,#20]
cmp	r1,#1
beq	True

False:
pop	{pc}

True:
mov	r1,r0
ldrb	r0,[r1,#10]
cmp	r0,#0
ldr	r3,=#0x806B745
bx	r3
