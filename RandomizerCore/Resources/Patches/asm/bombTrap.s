.thumb
push	{lr}
ldr	r0,=#0x3001160
mov	r1,#0x20
mov	r2,#0
ldr	r3,=#0x80A21A4
mov	lr,r3
mov	r3,#0
.short	0xF800
pop	{pc}
