.thumb
mov	r2,#0
ldr	r3,=#0x80A217C
mov	lr,r3
.short	0xF800
mov	r5,r0
ldr	r3,=#0x809C9E6
ldrb	r3,[r3]
strb	r3,[r5,#0x0A]
ldr	r3,=#0x809C9E8
ldrb	r3,[r3]
strb	r3,[r5,#0x0B]
mov	r3,#0xFF
strb	r3,[r5,#0x0E]
ldr	r3,=#0x809C84D
bx	r3
