.thumb
mov	r2,#1
strb	r2,[r5,#0x18]
str	r1,[r5,#0x54]
ldr	r2,=#0x3003D90
ldr	r4,[r2,#4]
ldr	r3,=#0x80A1C91
bx	r3
