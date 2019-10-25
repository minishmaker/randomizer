.thumb
mov	r2,#0
ldr	r0,=#0x3001002
mov	r3,#4
strb	r3,[r0]
strb	r2,[r0,#1]
strb	r2,[r0,#2]
mov	r3,#1
strb	r3,[r1,#6]
mov	r4,#0
mov	r0,#0x78
ldr	r3,=#0x80532FF
