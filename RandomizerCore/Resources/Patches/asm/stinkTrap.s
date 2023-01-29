.thumb
push	{lr}
ldr	r0,=#0x3003F8F
mov	r1,#0xFF
strb	r1,[r0]

ldr	r0,=#0x3001160
mov	r1,#0x21
mov	r2,#2
ldr	r3,=#0x80A21A4
mov	lr,r3
mov	r3,#0
.short	0xF800

ldr	r0,=#0xEA
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
pop	{pc}
