.thumb
push	{r4,lr}

@check our custom nut flag
ldr	r0,=#0x2002EA4	@base offset
mov	r1,#0		@flag id to check
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800

@and return
ldr	r3,=#0x804C165
bx	r3
