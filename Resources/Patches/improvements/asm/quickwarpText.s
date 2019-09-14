.thumb
strb	r0,[r4,#0x13]
ldrh	r0,[r2,#0x0A]
mov	r1,r13
ldr	r3,=#0x80FBFE8
cmp	r2,r3
bne	normal
ldr	r3,=#0x3001002
ldrb	r3,[r3]
cmp	r3,#2
bne	normal
mov	r0,#2

normal:
ldr	r3,=#0x805EEF4
mov	lr,r3
.short	0xF800
ldr	r3,=#0x80520B3
bx	r3
