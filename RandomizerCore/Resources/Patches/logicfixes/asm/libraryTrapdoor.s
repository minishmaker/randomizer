.thumb
ldr	r0,=#0x2002CEA
ldrb	r1,[r0]
mov	r2,#0xCF
and	r1,r2
strb	r1,[r0]
mov	r0,#1
bx	lr
