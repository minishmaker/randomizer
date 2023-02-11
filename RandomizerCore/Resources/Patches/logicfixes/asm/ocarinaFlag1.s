.thumb
mov	r2,#0x80
lsl	r2,#0x15
orr	r1,r2
str	r1,[r0,#0x40]

@set flag and return
ldr	r3,=#0x2002EA4
mov	r2,#0x02
ldrb	r1,[r3]
orr	r1,r2
strb	r1,[r3]
ldr	r3,=#0x809C9FD
bx	r3
