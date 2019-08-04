.thumb
ldr	r0,=#0x2002CA7
ldrb	r1,[r0]
mov	r2,#1
orr	r1,r2
strb	r1,[r0]
mov	r0,#1
bx	lr
