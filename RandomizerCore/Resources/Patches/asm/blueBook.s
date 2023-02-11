.thumb
push	{lr}
ldr	r0,=#0x2002EA4
ldr	r1,=#6
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	loadBook

noBook:
ldr	r3,=#0x804E613
bx	r3

loadBook:
ldr	r3,=#0x804E60D
bx	r3
