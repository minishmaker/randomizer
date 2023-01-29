.thumb
push	{lr}
ldr	r0,=#0x3001160
mov	r1,#0x18
mov	r2,#0
ldr	r3,=#0x80A21A4
mov	lr,r3
mov	r3,#0
.short	0xF800

mov	r1,#1
strb	r1,[r0,#0xB]

ldr	r0,=#0x95
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
pop	{pc}
