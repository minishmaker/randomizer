.thumb
push	{lr}
ldr	r0,=#0x2002EA4
ldr	r1,=#7
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
ldr	r3,=#0x804C6E5
bx	r3
